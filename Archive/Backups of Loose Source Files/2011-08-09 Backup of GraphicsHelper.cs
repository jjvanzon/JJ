//
//  Circle.Client.WinForms.Helpers.GraphicsHelper
//
//      Author: Jan-Joost van Zon
//      Date: 20-07-2011 - 20-07-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Circle.Client.WinForms.Helpers
{
    public static class GraphicsHelper
    {
        public static Image ScaleImage(
            Image sourceImage, 
            Rectangle rectangle, 
            float opacity = 1, 
            RectangleF offset = default(RectangleF))
        {
            Image scaledImage = new Bitmap(rectangle.Width, rectangle.Height);
            Graphics graphics = Graphics.FromImage(scaledImage);
            DrawImage(graphics, sourceImage, rectangle, opacity, offset);
            return scaledImage;
        }

        /// <summary>
        /// TODO: Use SourceRectangle.
        /// </summary>
        public static void DrawImage(
            System.Drawing.Graphics graphics, 
            Image image, 
            Rectangle rectangle = default(Rectangle), 
            float opacity = 1, 
            RectangleF offset = default(RectangleF))
        {
            // Resolve defaults

            if (rectangle == default(Rectangle)) rectangle = new Rectangle(0, 0, image.Width, image.Height);
            if (offset == default(RectangleF)) offset = new RectangleF(0, 0, rectangle.Width, rectangle.Height);

            // Unscaled, no clipping

            if (rectangle.Width == image.Width && 
                rectangle.Height == image.Height && 
                offset.X == 0 && offset.Y == 0 &&
                offset.Width == rectangle.Width && 
                offset.Height == rectangle.Height && 
                opacity == 1)
            {
                graphics.DrawImageUnscaled(image, rectangle);
                return;
            }

            // Unscaled and clipped bottom-right

            if (
                // Rectangle and image size not equal and
                offset.X == 0 && offset.Y == 0 &&
                offset.Width == image.Width && 
                offset.Height == image.Height && 
                opacity == 1) 
            {
                graphics.DrawImageUnscaledAndClipped(image, rectangle);
                return;
            }

            RectangleF sourceRectangle = 
                OffsetRectangleToSourceRectangle(
                    offset, 
                    rectangle,
                    image.Width, 
                    image.Height);

            // Scaled or clipped top-left, no opacity

            if (
                // Rectangle and image size not equal and
                // offset and image size not equal and
                opacity == 1)
            {
                graphics.DrawImage(
                    image, 
                    rectangle, 
                    (float)sourceRectangle.X,
                    (float)sourceRectangle.Y, 
                    (float)sourceRectangle.Width, 
                    (float)sourceRectangle.Height,
                    GraphicsUnit.Pixel);
                return;
            }

            // With opacity

            ColorMatrix colorMatrix = new ColorMatrix (new float[][] {
                new float[] {1, 0, 0, 0, 0},
                new float[] {0, 1, 0, 0, 0},
                new float[] {0, 0, 1, 0, 0},
                new float[] {0, 0, 0, opacity, 0},
                new float[] {0, 0, 0, 0, 1}});

            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            graphics.DrawImage(
                image,
                rectangle,
                (float)sourceRectangle.X,
                (float)sourceRectangle.Y,
                (float)sourceRectangle.Width,
                (float)sourceRectangle.Height,
                GraphicsUnit.Pixel,
                imageAttributes);
        }

        /// <summary>
        /// I have the Offset Rectangle, which says where the full image is placed.
        /// The Target Rectangle will cut a portion out of this.
        /// Now I have to calculate which portion of the source image this is.
        /// First we make the Target Rectangle's coordinates relative to the Offset.
        /// Then we divide it by the offset's width and height, so it becomes based on 1.
        /// Then we multiply it by the image's width and height, 
        /// so it becomes the rectangle relative to the image.
        /// </summary>

        public static RectangleF OffsetRectangleToSourceRectangle(
            RectangleF offsetRectangle, 
            Rectangle targetRectangle, 
            int sourceWidth, 
            int sourceHeight)
        {
            return new RectangleF()
            {
                X = (targetRectangle.X - offsetRectangle.X) / offsetRectangle.Width * sourceWidth, 
                Y = (targetRectangle.Y - offsetRectangle.Y) / offsetRectangle.Height * sourceHeight, 
                Width = targetRectangle.Width / offsetRectangle.Width * sourceWidth, 
                Height = targetRectangle.Height / offsetRectangle.Height * sourceHeight
            };
        }
    }
}
