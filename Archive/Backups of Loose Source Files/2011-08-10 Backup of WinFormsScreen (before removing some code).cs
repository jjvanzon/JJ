//
//  Circle.Controls.WinForms.WinFormsScreen
//
//      Author: Jan-Joost van Zon
//      Date: 31-05-2011 - 01-06-2011
//
//  -----

using System;
using System.Drawing;
using System.Windows.Forms;
using Circle.Code.Conditions;
using Circle.Controls.Classes;
using CircleControls = Circle.Controls.Classes;
using Circle.Graphics.Objects;
using Circle.Graphics.Objects.WinForms;
using Drawing = System.Drawing;
using Circle.Client.WinForms.Helpers;
using System.Drawing.Drawing2D;
using Windows = System.Windows;
using Circle.Code.Objects;
using Circle.Code.Events;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing.Imaging;

namespace Circle.Controls.WinForms
{
    public class WinFormsScreen : Classes.Screen, IDraw
    {
        // General

        public WinFormsScreen(Windows.Forms.Control winFormsControl)
        {
            base.Draw = this;

            WinFormsControl = winFormsControl;

            ApplyAutoRespond();
            ApplyAutoDraw();

            Buffer = new GraphicsBuffer(WinFormsControl);

            ApplyBufferMode();
        }

        private Windows.Forms.Control _winFormsControl;
        public Windows.Forms.Control WinFormsControl
        {
            get { return _winFormsControl; }
            set
            {
                Condition.NotNull(value, "WinFormsControl");
                _winFormsControl = value;
            }
        }

        // Interaction

        private bool _autoRespond = true;
        /// <summary>
        /// Makes Circle Controls respond to e.g. mouse clicks of the WinForms control.
        /// If set to false you would have to call e.g. Interaction.OnMouseDown
        /// to trigger responses as such.
        /// </summary>
        public bool AutoRespond
        {
            get { return _autoRespond; }
            set
            {
                if (_autoRespond == value) return;
                _autoRespond = value;
                ApplyAutoRespond();
            }
        }

        private void ApplyAutoRespond()
        {
            if (AutoRespond)
            {
                WinFormsControl.MouseDown += WinFormsControl_MouseDown;
                WinFormsControl.MouseMove += WinFormsControl_MouseMove;
                WinFormsControl.MouseUp += WinFormsControl_MouseUp;
                WinFormsControl.KeyDown += WinFormsControl_KeyDown;
                WinFormsControl.KeyUp += WinFormsControl_KeyUp;
            }
            else
            {
                WinFormsControl.MouseDown -= WinFormsControl_MouseDown;
                WinFormsControl.MouseMove -= WinFormsControl_MouseMove;
                WinFormsControl.MouseUp -= WinFormsControl_MouseUp;
                WinFormsControl.KeyDown -= WinFormsControl_KeyDown;
                WinFormsControl.KeyUp -= WinFormsControl_KeyUp;
            }
        }

        private void WinFormsControl_MouseDown(object sender, Windows.Forms.MouseEventArgs e)
        {
            base.Interaction.OnMouseDown(new CircleControls.MouseEventArgs(e));
        }

        private void WinFormsControl_MouseMove(object sender, Windows.Forms.MouseEventArgs e)
        {
            base.Interaction.OnMouseMove(new CircleControls.MouseEventArgs(e));
        }

        private void WinFormsControl_MouseUp(object sender, Windows.Forms.MouseEventArgs e)
        {
            base.Interaction.OnMouseUp(new CircleControls.MouseEventArgs(e));
        }

        private void WinFormsControl_KeyUp(object sender, KeyEventArgs e)
        {
            base.Interaction.OnKeyUp(e);
        }

        private void WinFormsControl_KeyDown(object sender, KeyEventArgs e)
        {
            base.Interaction.OnKeyDown(e);
        }

        // Paint

        /// <summary>
        /// Do not set AutoDraw to true when BufferingMode is Manual.
        /// </summary>
        public new bool AutoDraw
        {
            get { return base.AutoDraw; }
            set
            {
                if (base.AutoDraw == value) return;
                base.AutoDraw = value;
                ApplyAutoDraw();
            }
        }

        private void ApplyAutoDraw()
        {
            if (AutoDraw)
            {
                WinFormsControl.Paint += new PaintEventHandler(winFormsControl_Paint);
            }
            else
            {
                WinFormsControl.Paint -= new PaintEventHandler(winFormsControl_Paint);
            }
        }

        void winFormsControl_Paint(object sender, PaintEventArgs e)
        {
            Paint();
        }

        public readonly MethodEvents DrawEvents = new MethodEvents();

        public void Paint()
        {
            DrawEvents.OnBefore();
            base.RequestDraw();
        }

        // Buffer

        public Drawing.Graphics Graphics;
        private GraphicsBuffer Buffer;

        private BufferMode _bufferMode = BufferMode.Automatic;
        /// <summary>
        /// When BufferMode is Manual make sure you set AutoDraw to false.
        /// </summary>
        public BufferMode BufferMode 
        {
            get { return _bufferMode; }
            set
            {
                _bufferMode = value;
                ApplyBufferMode();
            }
        }

        private void ApplyBufferMode()
        {
            switch (BufferMode)
            {
                case WinForms.BufferMode.NoBuffer:
                    StartUnbuffered();
                    break;
            }
        }

        private void StartUnbuffered()
        {
            Graphics = WinFormsControl.CreateGraphics();
            Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }

        public void StartBuffer()
        {
            Buffer.Start();
            Graphics = Buffer.Graphics;
        }

        public void EndBuffer()
        {
            Buffer.End();
        }

        // Draw

        void IDraw.Start()
        {
            switch (BufferMode)
            {
                case BufferMode.NoBuffer:
                    StartUnbuffered();
                    break;

                case BufferMode.Automatic:
                    StartBuffer();
                    break;
            }
        }

        void IDraw.End()
        {
            switch (BufferMode)
            {
                case BufferMode.Automatic:
                    EndBuffer();
                    break;
            }

            DrawEvents.OnAfter();
        }

        public event Action Cleared;

        void IDraw.Clear()
        {
            Graphics.Clear(WinFormsControl.BackColor);

            if (Cleared != null) Cleared();
        }

        void IDraw.Text(Text text)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = text.Style.HorizontalAlignment;
            stringFormat.LineAlignment = text.Style.VerticalAlignment;

            if (text.Style.Wrap == false)
            {
                stringFormat.FormatFlags |= StringFormatFlags.NoWrap;
            }

            if (text.Style.Abbreviate)
            {
                stringFormat.Trimming = StringTrimming.EllipsisCharacter;
            }

            Graphics.DrawString(
                text.FormattedText,
                text.Style.Font,
                new SolidBrush(text.Style.Color),
                text.Rectangle.ToRectangleF(),
                stringFormat);
        }

        void IDraw.Line(Line line)
        {
            Graphics.DrawLine(
                line.Style.ToPen(), 
                line.PointA.ToPointF(), 
                line.PointB.ToPointF());
        }

        void IDraw.Point(PointD point)
        {
            Condition.NotNull(point.Style, "Point.Style");

            // Draw.Ellipse is used for drawing a point.

            BackStyle backStyle = new BackStyle() { Color = point.Style.Color};

            base.Draw.Ellipse(new Ellipse()
            {
                Center = point,
                BackStyle = backStyle,
                RadiusX = point.Style.Width,
                RadiusY = point.Style.Width
            });
        }

        void IDraw.Polygon(Polygon polygon)
        {
            PointF[] pointFArray = polygon.Points.ConvertAll(p => p.ToPointF()).ToArray();

            if (polygon.BackStyle != null)
            {
                Graphics.FillPolygon(new SolidBrush(polygon.BackStyle.Color), pointFArray);
            }

            if (polygon.LineStyle != null)
            {
                Graphics.DrawPolygon(polygon.LineStyle.ToPen(), pointFArray);
            }
        }

        void IDraw.Rectangle(RectangleD rectangle)
        {
            if (rectangle.BackStyle != null)
            {
                Graphics.FillRectangle(new SolidBrush(rectangle.BackStyle.Color), rectangle.ToRectangleF());
            }

            if (rectangle.LineStyle != null)
            {
                Graphics.DrawRectangle(
                    rectangle.LineStyle.ToPen(), 
                    (float)rectangle.X1,
                    (float)rectangle.Y1,
                    (float)rectangle.Width,
                    (float)rectangle.Height);
            }
        }

        void IDraw.Ellipse(Ellipse ellipse)
        {
            RectangleF rectangle = AboveMinimal.EllipseRectangle(ellipse.Rectangle.ToRectangleF());

            if (ellipse.BackStyle != null)
            {
                Graphics.FillEllipse(ellipse.BackStyle.ToBrush(), rectangle);
            }

            if (ellipse.LineStyle != null)
            {
                Graphics.DrawEllipse(ellipse.LineStyle.ToPen(), rectangle);
            }
        }

        /*void IDraw.Picture(Picture picture)
        {
            RectangleF offset =
                picture.Style.Offset != null ?
                picture.Style.Offset.ToRectangleF() :
                default(RectangleF);

            GraphicsHelper.DrawImageWithOffset(
                Graphics,
                picture.Style.Image,
                picture.Rectangle.ToRectangle(),
                picture.Style.Opacity,
                offset);
        }*/

        /*void IDraw.Picture(Picture picture)
        {
            GraphicsHelper.DrawImageWithOffset(
                Graphics,
                picture.Style.OutputImage,
                picture.Rectangle.ToRectangle(),
                offset: picture.Style.Offset.ToRectangleF());
        }*/

        void IDraw.Picture(Picture picture)
        {
            Graphics.DrawImage(picture.OutputImage, picture.Rectangle.PointA.ToPoint());
        }
    }
}
