//
//  Circle.Controls.Classes.DrawTrigger_Older
//
//      Author: Jan-Joost van Zon
//      Date: 23-06-2011 - 23-06-2011
//
//  -----

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Conditions;
using Circle.Controls.Style;
using Circle.Code.Events;
using Circle.Graphics.Objects;
using Circle.Code.Objects;

namespace Circle.Controls.Classes
{
    internal class DrawTrigger_Older
    {
        public DrawTrigger_Older(Screen screen)
        {
            Condition.NotNull(screen, "Screen");

            Screen = screen;
        }

        private Screen Screen;

        public void Set(Label label)
        {
            label.Text.ValueEvents.Changed += (x, y) => Screen.RequestDraw();

            label.StyleEvents.Created += BoxStyle_Created;
            label.StyleEvents.Annulled += BoxStyle_Annulled;

            if (label.StyleEvents.IsCreated) BoxStyle_Created(label.Style);
        }

        public void Annull(Label label)
        {
            label.Text.ValueEvents.Changed -= (x, y) => Screen.RequestDraw();

            label.StyleEvents.Created -= BoxStyle_Created;
            label.StyleEvents.Annulled -= BoxStyle_Annulled;
        }

            void BoxStyle_Created(BoxStyle boxStyle)
            {
                Set(boxStyle.PositioningEvents);
                Set(boxStyle.BackStyleEvents);
                Set(boxStyle.LineStyles.TopEvents);
                Set(boxStyle.LineStyles.RightEvents);
                Set(boxStyle.LineStyles.BottomEvents);
                Set(boxStyle.LineStyles.LeftEvents);
                Set(boxStyle.TextStyleEvents);
            }

            void BoxStyle_Annulled(BoxStyle boxStyle)
            {
                Annull(boxStyle.PositioningEvents);
                Annull(boxStyle.BackStyleEvents);
                Annull(boxStyle.LineStyles.TopEvents);
                Annull(boxStyle.LineStyles.RightEvents);
                Annull(boxStyle.LineStyles.BottomEvents);
                Annull(boxStyle.LineStyles.LeftEvents);
                Annull(boxStyle.TextStyleEvents);
            }

                public void Set(Events<BoxPositioning> boxPositioning)
                {
                    boxPositioning.Changed += (x, y) => Screen.RequestDraw();
                    boxPositioning.Created += BoxPositioning_Created;
                    boxPositioning.Annulled += BoxPositioning_Annulled;

                    if (boxPositioning.IsCreated) BoxPositioning_Created(boxPositioning.Value);
                }

                public void Annull(Events<BoxPositioning> boxPositioning)
                {
                    boxPositioning.Changed -= (x, y) => Screen.RequestDraw();
                    boxPositioning.Created -= BoxPositioning_Created;
                    boxPositioning.Annulled -= BoxPositioning_Annulled;
                }

                    void BoxPositioning_Created(BoxPositioning boxPositioning)
                    {
                        boxPositioning.BackPositioningEvents.Changed += (x, y) => Screen.RequestDraw();
                        boxPositioning.LineEndPositioningEvents.Changed += (x, y) => Screen.RequestDraw();

                        Set(boxPositioning.PaddingEvents);
                    }

                    void BoxPositioning_Annulled(BoxPositioning boxPositioning)
                    {
                        boxPositioning.BackPositioningEvents.Changed -= (x, y) => Screen.RequestDraw();
                        boxPositioning.LineEndPositioningEvents.Changed -= (x, y) => Screen.RequestDraw();

                        Annull(boxPositioning.PaddingEvents);
                    }

                        public void Set(Events<BoxAspect> boxAspect)
                        {
                            boxAspect.Changed += (x, y) => Screen.RequestDraw();
                            boxAspect.Created += BoxAspect_Created;
                            boxAspect.Annulled += BoxAspect_Annulled;

                            if (boxAspect.IsCreated) BoxAspect_Created(boxAspect.Value);
                        }

                        void Annull(Events<BoxAspect> boxAspect)
                        {
                            boxAspect.Changed -= (x, y) => Screen.RequestDraw();
                            boxAspect.Created -= BoxAspect_Created;
                            boxAspect.Annulled -= BoxAspect_Annulled;
                        }

                            void BoxAspect_Created(BoxAspect boxAspect)
                            {
                                boxAspect.TopEvents.Changed += (x, y) => Screen.RequestDraw();
                                boxAspect.RightEvents.Changed += (x, y) => Screen.RequestDraw();
                                boxAspect.BottomEvents.Changed += (x, y) => Screen.RequestDraw();
                                boxAspect.LeftEvents.Changed += (x, y) => Screen.RequestDraw();
                            }

                            void BoxAspect_Annulled(BoxAspect boxAspect)
                            {
                                boxAspect.TopEvents.Changed -= (x, y) => Screen.RequestDraw();
                                boxAspect.RightEvents.Changed -= (x, y) => Screen.RequestDraw();
                                boxAspect.BottomEvents.Changed -= (x, y) => Screen.RequestDraw();
                                boxAspect.LeftEvents.Changed -= (x, y) => Screen.RequestDraw();
                            }

                public void Set(Events<BackStyle> backStyle)
                {
                    backStyle.Changed += (x, y) => Screen.RequestDraw();

                    backStyle.Created += BackStyle_Created;
                    backStyle.Annulled += BackStyle_Annulled;

                    if (backStyle.IsCreated) BackStyle_Created(backStyle.Value);
                }

                public void Annull(Events<BackStyle> backStyle)
                {
                    backStyle.Changed -= (x, y) => Screen.RequestDraw();

                    backStyle.Created -= BackStyle_Created;
                    backStyle.Annulled -= BackStyle_Annulled;
                }

                    void BackStyle_Created(BackStyle backStyle)
                    {
                        backStyle.ColorEvents.Changed += (x, y) => Screen.RequestDraw();
                    }

                    void BackStyle_Annulled(BackStyle backStyle)
                    {
                        backStyle.ColorEvents.Changed -= (x, y) => Screen.RequestDraw();
                    }

                public void Set(Events<LineStyle> lineStyle)
                {
                    lineStyle.Changed += (x, y) => Screen.RequestDraw();

                    lineStyle.Created += LineStyle_Created;
                    lineStyle.Annulled += LineStyle_Annulled;

                    if (lineStyle.IsCreated) LineStyle_Created(lineStyle.Value);
                }

                public void Annull(Events<LineStyle> lineStyle)
                {
                    lineStyle.Changed -= (x, y) => Screen.RequestDraw();

                    lineStyle.Created -= LineStyle_Created;
                    lineStyle.Annulled -= LineStyle_Annulled;
                }

                    void LineStyle_Created(LineStyle lineStyle)
                    {
                        lineStyle.ColorEvents.Changed += (x, y) => Screen.RequestDraw();
                        lineStyle.DashEvents.Changed += (x, y) => Screen.RequestDraw();
                        lineStyle.WidthEvents.Changed += (x, y) => Screen.RequestDraw();
                        lineStyle.ZIndexEvents.Changed += (x, y) => Screen.RequestDraw();
                    }

                    void LineStyle_Annulled(LineStyle lineStyle)
                    {
                        lineStyle.ColorEvents.Changed -= (x, y) => Screen.RequestDraw();
                        lineStyle.DashEvents.Changed -= (x, y) => Screen.RequestDraw();
                        lineStyle.WidthEvents.Changed -= (x, y) => Screen.RequestDraw();
                        lineStyle.ZIndexEvents.Changed -= (x, y) => Screen.RequestDraw();
                    }

                public void Set(Events<TextStyle> textStyle)
                {
                    textStyle.Changed += (x, y) => Screen.RequestDraw();

                    textStyle.Created += TextStyle_Created;
                    textStyle.Annulled += TextStyle_Annulled;

                    if (textStyle.IsCreated) TextStyle_Created(textStyle.Value);
                }

                public void Annull(Events<TextStyle> textStyle)
                {
                    textStyle.Created -= TextStyle_Created;
                    textStyle.Annulled -= TextStyle_Annulled;
                }

                    void TextStyle_Created(TextStyle textStyle)
                    {
                        textStyle.AbbreviateEvents.Changed += (x, y) => Screen.RequestDraw();
                        textStyle.ColorEvents.Changed += (x, y) => Screen.RequestDraw();
                        textStyle.FontEvents.Changed += (x, y) => Screen.RequestDraw();
                        textStyle.HorizontalAlignmentEvents.Changed += (x, y) => Screen.RequestDraw();
                        textStyle.VerticalAlignmentEvents.Changed += (x, y) => Screen.RequestDraw();
                        textStyle.WrapEvents.Changed += (x, y) => Screen.RequestDraw();
                    }

                    void TextStyle_Annulled(TextStyle textStyle)
                    {
                        textStyle.AbbreviateEvents.Changed -= (x, y) => Screen.RequestDraw();
                        textStyle.ColorEvents.Changed -= (x, y) => Screen.RequestDraw();
                        textStyle.FontEvents.Changed -= (x, y) => Screen.RequestDraw();
                        textStyle.HorizontalAlignmentEvents.Changed -= (x, y) => Screen.RequestDraw();
                        textStyle.VerticalAlignmentEvents.Changed -= (x, y) => Screen.RequestDraw();
                        textStyle.WrapEvents.Changed -= (x, y) => Screen.RequestDraw();
                    }
    }
}
