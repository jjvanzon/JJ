//
//  Circle.Graphics.Objects.PictureStyle
//
//      Author: Jan-Joost van Zon
//      Date: 15-07-2011 - 16-07-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Events;
using System.Drawing;
using Circle.Client.WinForms.Helpers;

namespace Circle.Graphics.Objects
{
    public class PictureStyle
    {
        // Image

        public Image Image
        {
            get { return ImageEvents.Value; }
            set { ImageEvents.Value = value; }
        }

        public readonly Events<Image> ImageEvents = new Events<Image>();

        // Opacity

        public float Opacity
        {
            get { return OpacityEvents.Value; }
            set { OpacityEvents.Value = value; }
        }

        public readonly Events<float> OpacityEvents = new Events<float>();

        // Offset

        public RectangleD Offset
        {
            get { return OffsetEvents.Value; }
            set { OffsetEvents.Value = value; }
        }

        public readonly Events<RectangleD> OffsetEvents = new Events<RectangleD>();

        // Output Image

        public Image OutputImage
        {
            get
            {
                // TODO: Put operator objects on class-level, and delegte PictureStyle properties to it.

                ImageIn imageIn = new ImageIn()
                {
                    Image = Image
                };

                Scale scale = new Scale()
                {
                    Input = imageIn,
                    Opacity = Opacity,
                    Height = (int)Offset.Height,
                    Width = (int)Offset.Width
                };

                return scale.Output;
            }

        }

        /// <summary>
        /// Image with scaling and opacity applied, but not the offset shift.
        /// </summary>
        /*public Image OutputImage
        {
            get
            {
                if (_outputImage == null) CacheOutputImage();
                return _outputImage;
            }
            private set
            {
                _outputImage = value;
            }
        }

        private Image _outputImage;

        private void CacheOutputImage()
        {
            // The difficulty is in the fact that the origin of the coordinates keep changing.
            // Offset becomes the origin of the cached image.
            Rectangle offset =
                Offset != null ?
                new Rectangle(0, 0, (int)Offset.Width, (int)Offset.Height):
                default(Rectangle);

            OutputImage = GraphicsHelper.ScaleImage(Image, offset, Opacity);
        }*/
    }
}
