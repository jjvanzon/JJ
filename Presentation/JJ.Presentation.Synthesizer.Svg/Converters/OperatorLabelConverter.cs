﻿using JJ.Framework.Presentation.Svg.Models.Elements;
using JJ.Framework.Reflection.Exceptions;
using JJ.Presentation.Synthesizer.Svg.Helpers;
using JJ.Presentation.Synthesizer.ViewModels.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JJ.Presentation.Synthesizer.Svg.Converters
{
    internal class OperatorLabelConverter
    {
        public Label ConvertToOperatorLabel(OperatorViewModel sourceOperatorViewModel, Rectangle destOperatorRectangle)
        {
            if (sourceOperatorViewModel == null) throw new NullException(() => sourceOperatorViewModel);
            if (destOperatorRectangle == null) throw new NullException(() => destOperatorRectangle);

            Label destLabel = destOperatorRectangle.Children.OfType<Label>().FirstOrDefault(); // First instead of Single will result in excessive ones being cleaned up.
            if (destLabel == null)
            {
                destLabel = new Label();
                destLabel.Diagram = destOperatorRectangle.Diagram;
                destLabel.Parent = destOperatorRectangle;
            }

            destLabel.Text = sourceOperatorViewModel.Name;
            destLabel.Width = StyleHelper.DEFAULT_WIDTH;
            destLabel.Height = StyleHelper.DEFAULT_HEIGHT;
            destLabel.TextStyle = StyleHelper.TextStyle;

            return destLabel;
        }
    }
}