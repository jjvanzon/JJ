//
//  Circle.Graphics.Objects.Picture
//
//      Author: Jan-Joost van Zon
//      Date: 15-07-2011 - 16-07-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Circle.Code.Conditions;
using Circle.Code.Events;
using Circle.Code.Objects;
using Circle.Client.WinForms.Helpers;

namespace Circle.Graphics.Objects
{
    public class Picture
    {
        // Constructor

        public Picture()
        {
            InitializeRectangle();
            InitializeStyle();
        }

        // Rectangle

        /// <summary> Not nullable </summary>
        public RectangleD Rectangle
        {
            get { return RectangleEvents.Value; }
            set { RectangleEvents.Value = value; }
        }

        public readonly Events<RectangleD> RectangleEvents = new Events<RectangleD>();

        private NotNull<RectangleD> RectangleNotNull;

        private void InitializeRectangle()
        {
            RectangleNotNull = new NotNull<RectangleD>(RectangleEvents);
        }

        // Style

        /// <summary> Not nullable </summary>
        public PictureStyle Style
        {
            get { return StyleEvents.Value; }
            set { StyleEvents.Value = value; }
        }

        public readonly Events<PictureStyle> StyleEvents = new Events<PictureStyle>();

        private NotNull<PictureStyle> StyleNotNull;

        private void InitializeStyle()
        {
            StyleNotNull = new NotNull<PictureStyle>(StyleEvents);
        }

        // Output Image

        public Image OutputImage
        {
            get
            {
                ImageIn imageIn = new ImageIn()
                {
                    Image = Style.OutputImage
                };

                Crop crop = new Crop
                {
                    Input = imageIn,
                    Rectangle = Rectangle,
                    OffsetX = Style.Offset.X1,
                    OffsetY = Style.Offset.Y1
                };

                return crop.Output;
            }
        }

        /// <summary>
        /// Image with scaling, opacity and offset applied
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
            // Rectangle becomes the origin of the cached image.
            RectangleF offset =
                Style.Offset != null ?
                new RectangleF(
                    (float)(Style.Offset.X1 - Rectangle.X1), 
                    (float)(Style.Offset.Y1 - Rectangle.Y1), 
                    (float)(Style.Offset.Width), (float)(Style.Offset.Height)):
                default(RectangleF);

            Rectangle rectangle = new Rectangle(
                0, 0,
                (int)Rectangle.Width, (int)Rectangle.Height);

            OutputImage = GraphicsHelper.ScaleImageWithOffset(Style.OutputImage, rectangle, offset: offset);
        }*/
    }
}
