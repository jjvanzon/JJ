//
//  Circle.Diagram.Engine.PositionErrorMessages
//
//      Author: Jan-Joost van Zon
//      Date: 2011-05-22 - 2011-05-22
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Diagram.Entities;

namespace Circle.Diagram.Engine
{
    public class PositionErrorMessages : StepWithDiagram
    {
        private double Spacing = 0.0125;
        private double BarHeight = 0.05;

        protected override void OnExecute()
        {
            // For now only show the first 6 error messages.
            // TODO: make a more generic way to position error messages.

            List<ErrorMessage> errorMessages = Diagram.ErrorMessages.Take(6).ToList();

            double width = (1.0 - Spacing * 4) / 3;

            if (errorMessages.Count > 0)
            {
                ErrorMessage errorMessage = errorMessages[0];
                errorMessage.Left = Spacing;
                errorMessage.Right = Spacing + width;
                errorMessage.Bottom = 1 - Spacing - BarHeight - Spacing;
                errorMessage.Top = 1 - Spacing - BarHeight - Spacing - BarHeight;
            }

            if (errorMessages.Count > 1)
            {
                ErrorMessage errorMessage = errorMessages[1];
                errorMessage.Left = Spacing + width + Spacing;
                errorMessage.Right = Spacing + width + Spacing + width;
                errorMessage.Bottom = 1 - Spacing - BarHeight - Spacing;
                errorMessage.Top = 1 - Spacing - BarHeight - Spacing - BarHeight;
            }

            if (errorMessages.Count > 2)
            {
                ErrorMessage errorMessage = errorMessages[2];
                errorMessage.Left = Spacing + width + Spacing + width + Spacing;
                errorMessage.Right = Spacing + width + Spacing + width + Spacing + width;
                errorMessage.Bottom = 1 - Spacing - BarHeight - Spacing;
                errorMessage.Top = 1 - Spacing - BarHeight - Spacing - BarHeight;
            }

            if (errorMessages.Count > 3)
            {
                ErrorMessage errorMessage = errorMessages[3];
                errorMessage.Left = Spacing;
                errorMessage.Right = Spacing + width;
                errorMessage.Bottom = 1 - Spacing;
                errorMessage.Top = 1 - Spacing - BarHeight - Spacing - BarHeight;
            }

            if (errorMessages.Count > 4)
            {
                ErrorMessage errorMessage = errorMessages[4];
                errorMessage.Left = Spacing + width + Spacing;
                errorMessage.Right = Spacing + width + Spacing + width;
                errorMessage.Bottom = 1 - Spacing;
                errorMessage.Top = 1 - Spacing - BarHeight - Spacing - BarHeight;
            }

            if (errorMessages.Count > 5)
            {
                ErrorMessage errorMessage = errorMessages[5];
                errorMessage.Left = Spacing + width + Spacing + width + Spacing;
                errorMessage.Right = Spacing + width + Spacing + width + Spacing + width;
                errorMessage.Bottom = 1 - Spacing;
                errorMessage.Top = 1 - Spacing - BarHeight - Spacing - BarHeight;
            }
        }
    }
}
