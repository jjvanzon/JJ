﻿using JJ.Framework.Presentation.Svg.Models.Styling;
using JJ.Framework.Reflection.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JJ.Framework.Presentation.Svg.Helpers
{
    public static class TextHelper
    {
        /// <summary>
        /// Returns an approximate width of the string according to the specified font.
        /// </summary>
        public static float ApproximateTextWidth(string text, Font font, float averageAspectRatioOfCharacter = 0.8f)
        {
            if (font == null) throw new NullException(() => font);

            if (String.IsNullOrEmpty(text))
            {
                return 0f;
            }

            float charHeight = font.Size;
            float charWidth = charHeight * averageAspectRatioOfCharacter;
            float textWidth = charWidth * text.Length;
            return textWidth;
        }
    }
}
