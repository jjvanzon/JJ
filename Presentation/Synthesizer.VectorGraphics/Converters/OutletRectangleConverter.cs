﻿using System.Collections.Generic;
using System.Linq;
using JJ.Framework.Presentation.VectorGraphics.Gestures;
using JJ.Framework.Presentation.VectorGraphics.Models.Elements;
using JJ.Framework.Exceptions;
using JJ.Presentation.Synthesizer.VectorGraphics.Gestures;
using JJ.Presentation.Synthesizer.VectorGraphics.Helpers;
using JJ.Presentation.Synthesizer.ViewModels.Items;

namespace JJ.Presentation.Synthesizer.VectorGraphics.Converters
{
    internal class OutletRectangleConverter
    {
        private readonly Dictionary<int, Rectangle> _destOutletRectangleDictionary = new Dictionary<int, Rectangle>();
        private readonly HashSet<Rectangle> _destOutletRectangleHashSet = new HashSet<Rectangle>();
        private readonly GestureBase _dragLineGesture;
        private readonly ToolTipGesture _outletToolTipGesture;

        public OutletRectangleConverter(DragLineGesture dragLineGesture, ToolTipGesture outletToolTipGesture)
        {
            if (dragLineGesture == null) throw new NullException(() => dragLineGesture);
            if (outletToolTipGesture == null) throw new NullException(() => outletToolTipGesture);
            
            _dragLineGesture = dragLineGesture;
            _outletToolTipGesture = outletToolTipGesture;
        }

        public IList<Rectangle> ConvertToOutletRectangles(OperatorViewModel sourceOperatorViewModel, Rectangle destOperatorRectangle)
        {
            if (sourceOperatorViewModel == null) throw new NullException(() => sourceOperatorViewModel);
            if (destOperatorRectangle == null) throw new NullException(() => destOperatorRectangle);

            IList<OutletViewModel> sourceOutletViewModelsToConvert =
                sourceOperatorViewModel.Outlets
                                       .Where(outlet => outlet.Visible)
                                       .ToArray();

            if (sourceOutletViewModelsToConvert.Count == 0)
            {
                return new Rectangle[0];
            }

            IList<Rectangle> destOutletRectangles = new List<Rectangle>(sourceOutletViewModelsToConvert.Count);

            float outletWidth = destOperatorRectangle.Position.Width / sourceOutletViewModelsToConvert.Count;
            float rowHeight = destOperatorRectangle.Position.Height / 4;
            const float heightOverflow = StyleHelper.INLET_OUTLET_RECTANGLE_HEIGHT_OVERFLOW_IN_PIXELS;
            float x = 0;
            float y = rowHeight * 3;

            foreach (OutletViewModel sourceOutletViewModel in sourceOutletViewModelsToConvert)
            {
                Rectangle destOutletRectangle = ConvertToOutletRectangle(sourceOutletViewModel, destOperatorRectangle);

                destOutletRectangle.Position.X = x;
                destOutletRectangle.Position.Y = y;
                destOutletRectangle.Position.Width = outletWidth;
                destOutletRectangle.Position.Height = rowHeight + heightOverflow;

                destOutletRectangles.Add(destOutletRectangle);

                x += outletWidth;
            }

            return destOutletRectangles;
        }

        /// <summary> Converts everything but its coordinates. </summary>
        private Rectangle ConvertToOutletRectangle(OutletViewModel sourceOutletViewModel, Rectangle destOperatorRectangle)
        {
            int outletID = sourceOutletViewModel.ID;

            Rectangle destOutletRectangle;
            if (!_destOutletRectangleDictionary.TryGetValue(outletID, out destOutletRectangle))
            {
                destOutletRectangle = new Rectangle
                {
                    Diagram = destOperatorRectangle.Diagram,
                    Parent = destOperatorRectangle,
                    Tag = VectorGraphicsTagHelper.GetOutletTag(outletID)
                };

                destOutletRectangle.Style.BackStyle = StyleHelper.BackStyleInvisible;
                destOutletRectangle.Style.LineStyle = StyleHelper.BorderStyleInvisible;

                _destOutletRectangleDictionary.Add(outletID, destOutletRectangle);
                _destOutletRectangleHashSet.Add(destOutletRectangle);
            }

            destOutletRectangle.Gestures.Clear();
            destOutletRectangle.Gestures.Add(_dragLineGesture);
            destOutletRectangle.Gestures.Add(_outletToolTipGesture);
            destOutletRectangle.MustBubble = false; // So drag does not result in a move.

            return destOutletRectangle;
        }

        public void TryRemove(Rectangle destElement)
        {
            if (!_destOutletRectangleHashSet.Contains(destElement))
            {
                return;
            }

            int outletID = VectorGraphicsTagHelper.GetOutletID(destElement.Tag);

            _destOutletRectangleDictionary.Remove(outletID);
            _destOutletRectangleHashSet.Remove(destElement);

            destElement.Children.Clear();
            destElement.Parent = null;
            destElement.Diagram = null;
        }
    }
}
