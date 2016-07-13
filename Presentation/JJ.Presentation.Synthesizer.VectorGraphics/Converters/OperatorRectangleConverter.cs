﻿using JJ.Business.Synthesizer.Enums;
using JJ.Framework.Presentation.VectorGraphics.Gestures;
using JJ.Framework.Presentation.VectorGraphics.Helpers;
using JJ.Framework.Presentation.VectorGraphics.Models.Elements;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.VectorGraphics.Gestures;
using JJ.Presentation.Synthesizer.VectorGraphics.Helpers;
using JJ.Presentation.Synthesizer.ViewModels.Items;
using System.Collections.Generic;
using System.Linq;
using System;

namespace JJ.Presentation.Synthesizer.VectorGraphics.Converters
{
    internal class OperatorRectangleConverter
    {
        private readonly Dictionary<int, Rectangle> _destOperatorRectangleDictionary = new Dictionary<int, Rectangle>();

        private readonly Diagram _diagram;
        private readonly MoveGesture _moveGesture;
        private readonly SelectOperatorGesture _selectOperatorGesture;
        private readonly DoubleClickGesture _doubleClickOperatorGesture;

        public OperatorRectangleConverter(
            Diagram diagram,
            MoveGesture moveGesture,
            SelectOperatorGesture selectOperatorGesture,
            DoubleClickGesture doubleClickOperatorGesture)
        {
            if (diagram == null) throw new NullException(() => diagram);
            if (moveGesture == null) throw new NullException(() => moveGesture);
            if (selectOperatorGesture == null) throw new NullException(() => selectOperatorGesture);
            if (doubleClickOperatorGesture == null) throw new NullException(() => doubleClickOperatorGesture);

            _diagram = diagram;
            _moveGesture = moveGesture;
            _selectOperatorGesture = selectOperatorGesture;
            _doubleClickOperatorGesture = doubleClickOperatorGesture;
        }

        public Rectangle ConvertToOperatorRectangle(OperatorViewModel sourceOperatorViewModel, Diagram destDiagram)
        {
            if (sourceOperatorViewModel == null) throw new NullException(() => sourceOperatorViewModel);
            if (destDiagram == null) throw new NullException(() => destDiagram);

            Rectangle destOperatorRectangle = TryGetOperatorRectangle(sourceOperatorViewModel.ID);
            if (destOperatorRectangle == null)
            {
                destOperatorRectangle = new Rectangle
                {
                    Diagram = destDiagram,
                    Parent = destDiagram.Background,
                    Tag = VectorGraphicsTagHelper.GetOperatorTag(sourceOperatorViewModel.ID)
                };

                _destOperatorRectangleDictionary.Add(sourceOperatorViewModel.ID, destOperatorRectangle);
            }

            destOperatorRectangle.Position.Width = GetOperatorWidth(sourceOperatorViewModel);
            destOperatorRectangle.Position.Height = GetOperatorHeight(sourceOperatorViewModel);
            destOperatorRectangle.Position.X = sourceOperatorViewModel.CenterX - destOperatorRectangle.Position.Width / 2f;
            destOperatorRectangle.Position.Y = sourceOperatorViewModel.CenterY - destOperatorRectangle.Position.Height / 2f;

            if (sourceOperatorViewModel.IsSelected)
            {
                destOperatorRectangle.Style.BackStyle = StyleHelper.BackStyleSelected;
                destOperatorRectangle.Style.LineStyle = StyleHelper.BorderStyleSelected;
            }
            else
            {
                destOperatorRectangle.Style.LineStyle = StyleHelper.BorderStyle;
                destOperatorRectangle.Style.BackStyle = StyleHelper.GetGradedBackStyle(sourceOperatorViewModel.StyleGrade);
            }

            destOperatorRectangle.Gestures.Clear();
            destOperatorRectangle.Gestures.Add(_moveGesture);
            destOperatorRectangle.Gestures.Add(_selectOperatorGesture);
            destOperatorRectangle.Gestures.Add(_doubleClickOperatorGesture);

            return destOperatorRectangle;
        }

        private float GetOperatorHeight(OperatorViewModel sourceOperatorViewModel)
        {
            if (IsNumberOperator(sourceOperatorViewModel))
            {
                return StyleHelper.NUMBER_OPERATOR_HEIGHT;
            }
            else
            {
                return StyleHelper.OPERATOR_HEIGHT;
            }
        }

        private static float GetOperatorWidth(OperatorViewModel sourceOperatorViewModel)
        {
            if (IsNumberOperator(sourceOperatorViewModel))
            {
                return GetNumberOperatorWidth(sourceOperatorViewModel);
            }
            else
            {
                return GetOtherOperatorWidth(sourceOperatorViewModel);
            }
        }

        private static float GetOtherOperatorWidth(OperatorViewModel sourceOperatorViewModel)
        {
            float textWidth = TextHelper.ApproximateTextWidth(sourceOperatorViewModel.Caption, StyleHelper.DefaultFont);
            float minimumWidth = GetOtherOperatorMinimumWidth(sourceOperatorViewModel);

            float width = textWidth + StyleHelper.SpacingTimes2;

            if (width < minimumWidth)
            {
                width = minimumWidth;
            }

            return width;
        }

        private static float GetOtherOperatorMinimumWidth(OperatorViewModel sourceOperatorViewModel)
        {
            float minimumWidth;

            // Apply a minimum inlet and outlet height.
            int inletOrOutletCount = Math.Max(sourceOperatorViewModel.Outlets.Count, sourceOperatorViewModel.Inlets.Count);
            minimumWidth = inletOrOutletCount * StyleHelper.MINIMUM_INLET_OR_OUTLET_WIDTH_IN_PIXELS;

            // Apply minimum operator width
            if (minimumWidth < StyleHelper.OPERATOR_MINIMUM_WIDTH)
            {
                minimumWidth = StyleHelper.OPERATOR_MINIMUM_WIDTH;
            }

            return minimumWidth;
        }

        private static float GetNumberOperatorWidth(OperatorViewModel sourceOperatorViewModel)
        {
            float spacing = StyleHelper.Spacing;
            spacing *= 0.8f; // Use a smaller spacing for numbers.

            float textWidth = TextHelper.ApproximateTextWidth(sourceOperatorViewModel.Caption, StyleHelper.NumberOperatorFont);
            float width = textWidth + spacing + spacing;

            // Compensate for the fact that numbers are averagely wider than letters.
            width = width + spacing + spacing;

            // Apply minimum operator width
            if (width < StyleHelper.NUMBER_OPERATOR_MINIMUM_WIDTH)
            {
                width = StyleHelper.NUMBER_OPERATOR_MINIMUM_WIDTH;
            }

            return width;
        }

        private Rectangle TryGetOperatorRectangle(int id)
        {
            Rectangle destRectangle;
            if (!_destOperatorRectangleDictionary.TryGetValue(id, out destRectangle))
            {
                destRectangle = _diagram.Elements
                                        .OfType<Rectangle>()
                                        .Where(x => VectorGraphicsTagHelper.IsOperatorTag(x.Tag) &&
                                                    VectorGraphicsTagHelper.GetOperatorID(x.Tag) == id)
                                        .FirstOrDefault(); // First instead of Single will result in excessive ones being cleaned up.

                if (destRectangle != null)
                {
                    _destOperatorRectangleDictionary.Add(id, destRectangle);
                }
            }

            return destRectangle;
        }

        public void TryRemove(int operatorID)
        {
            Rectangle destElement;
            if (_destOperatorRectangleDictionary.TryGetValue(operatorID, out destElement))
            {
                _destOperatorRectangleDictionary.Remove(operatorID);

                destElement.Children.Clear();
                destElement.Parent = null;
                destElement.Diagram = null;
            }
        }

        private static bool IsNumberOperator(OperatorViewModel sourceOperatorViewModel)
        {
            return sourceOperatorViewModel.OperatorType.ID == (int)OperatorTypeEnum.Number;
        }
    }
}