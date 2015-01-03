//
//  Circle.Controls.Classes.DrawTrigger_Old
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
    internal class DrawTrigger_Old
    {
        public DrawTrigger_Old(Screen screen)
        {
            Condition.NotNull(screen, "Screen");

            Screen = screen;
        }

        private Screen Screen;

        public void Set(Label label)
        {
            label.Text.ValueEvents.Changed += (x, y) => Screen.RequestDraw();

            label.StyleEvents.Changed += StyleEvents_Changed;

            if (label.StyleEvents.IsCreated) StyleEvents_Changed(null, label.Style);
        }

        public void Annull(Label label)
        {
            label.Text.ValueEvents.Changed -= (x, y) => Screen.RequestDraw();

            label.StyleEvents.Changed -= StyleEvents_Changed;
        }

            void StyleEvents_Changed(BoxStyle old, BoxStyle value)
            {
                if (old != null)
                { 
                    Annull(old.PositioningEvents);
                    Annull(old.BackStyleEvents);
                    Annull(old.LineStyles.TopEvents);
                    Annull(old.LineStyles.RightEvents);
                    Annull(old.LineStyles.BottomEvents);
                    Annull(old.LineStyles.LeftEvents);
                    Annull(old.TextStyleEvents);
                }
                if (value != null)
                {
                    Set(value.PositioningEvents);
                    Set(value.BackStyleEvents);
                    Set(value.LineStyles.TopEvents);
                    Set(value.LineStyles.RightEvents);
                    Set(value.LineStyles.BottomEvents);
                    Set(value.LineStyles.LeftEvents);
                    Set(value.TextStyleEvents);
                }
            }

                public void Set(Events<BoxPositioning> boxPositioning)
                {
                    boxPositioning.Changed += (x, y) => Screen.RequestDraw();

                    boxPositioning.Changed += boxPositioning_Changed;

                    if (boxPositioning.IsCreated) boxPositioning_Changed(null, boxPositioning.Value);
                }

                public void Annull(Events<BoxPositioning> boxPositioning)
                {
                    boxPositioning.Changed -= (x, y) => Screen.RequestDraw();

                    boxPositioning.Changed -= boxPositioning_Changed;
                }

                    void boxPositioning_Changed(BoxPositioning old, BoxPositioning value)
                    {
                        if (old != null)
                        {
                            old.BackPositioningEvents.Changed -= (x, y) => Screen.RequestDraw();
                            old.LineEndPositioningEvents.Changed -= (x, y) => Screen.RequestDraw();

                            Annull(old.PaddingEvents);
                        }
                        if (value != null)
                        {
                            value.BackPositioningEvents.Changed += (x, y) => Screen.RequestDraw();
                            value.LineEndPositioningEvents.Changed += (x, y) => Screen.RequestDraw();

                            Set(value.PaddingEvents);
                        }
                    }

                        public void Set(Events<BoxAspect> boxAspect)
                        {
                            boxAspect.Changed += (x, y) => Screen.RequestDraw();

                            boxAspect.Changed += BoxAspect_Changed;

                            if (boxAspect.IsCreated) BoxAspect_Changed(null, boxAspect.Value);
                        }

                        void Annull(Events<BoxAspect> boxAspect)
                        {
                            boxAspect.Changed -= (x, y) => Screen.RequestDraw();

                            boxAspect.Changed -= BoxAspect_Changed;
                        }

                            void BoxAspect_Changed(BoxAspect old, BoxAspect value)
                            {
                                if (old != null)
                                {
                                    old.TopEvents.Changed -= (x, y) => Screen.RequestDraw();
                                    old.RightEvents.Changed -= (x, y) => Screen.RequestDraw();
                                    old.BottomEvents.Changed -= (x, y) => Screen.RequestDraw();
                                    old.LeftEvents.Changed -= (x, y) => Screen.RequestDraw();
                                }
                                if (value != null)
                                {
                                    value.TopEvents.Changed += (x, y) => Screen.RequestDraw();
                                    value.RightEvents.Changed += (x, y) => Screen.RequestDraw();
                                    value.BottomEvents.Changed += (x, y) => Screen.RequestDraw();
                                    value.LeftEvents.Changed += (x, y) => Screen.RequestDraw();
                                }
                            }

                public void Set(Events<BackStyle> backStyle)
                {
                    backStyle.Changed += (x, y) => Screen.RequestDraw();

                    backStyle.Changed += BackStyle_Changed;

                    if (backStyle.IsCreated) BackStyle_Changed(null, backStyle.Value);
                }

                public void Annull(Events<BackStyle> backStyle)
                {
                    backStyle.Changed -= (x, y) => Screen.RequestDraw();

                    backStyle.Changed -= BackStyle_Changed;
                }

                    void BackStyle_Changed(BackStyle old, BackStyle value)
                    {
                        if (old != null)
                        {
                            old.ColorEvents.Changed -= (x, y) => Screen.RequestDraw();
                        }
                        if (value != null)
                        {
                            value.ColorEvents.Changed += (x, y) => Screen.RequestDraw();
                        }
                    }

                public void Set(Events<LineStyle> lineStyle)
                {
                    lineStyle.Changed += (x, y) => Screen.RequestDraw();

                    lineStyle.Changed += LineStyle_Changed;

                    if (lineStyle.IsCreated) LineStyle_Changed(null, lineStyle.Value);
                }

                public void Annull(Events<LineStyle> lineStyle)
                {
                    lineStyle.Changed -= (x, y) => Screen.RequestDraw();

                    lineStyle.Changed -= LineStyle_Changed;
                }

                    void LineStyle_Changed(LineStyle old, LineStyle value)
                    {
                        if (old != null)
                        {
                            old.ColorEvents.Changed -= (x, y) => Screen.RequestDraw();
                            old.DashEvents.Changed -= (x, y) => Screen.RequestDraw();
                            old.WidthEvents.Changed -= (x, y) => Screen.RequestDraw();
                            old.ZIndexEvents.Changed -= (x, y) => Screen.RequestDraw();
                        }
                        if (value != null)
                        {
                            value.ColorEvents.Changed += (x, y) => Screen.RequestDraw();
                            value.DashEvents.Changed += (x, y) => Screen.RequestDraw();
                            value.WidthEvents.Changed += (x, y) => Screen.RequestDraw();
                            value.ZIndexEvents.Changed += (x, y) => Screen.RequestDraw();
                        }
                    }

                public void Set(Events<TextStyle> textStyle)
                {
                    textStyle.Changed += (x, y) => Screen.RequestDraw();

                    textStyle.Changed += TextStyle_Changed;

                    if (textStyle.IsCreated) TextStyle_Changed(null, textStyle.Value);
                }

                public void Annull(Events<TextStyle> textStyle)
                {
                    textStyle.Changed -= (x, y) => Screen.RequestDraw();

                    textStyle.Changed -= TextStyle_Changed;
                }

                    void TextStyle_Changed(TextStyle old, TextStyle value)
                    {
                        if (old != null)
                        {
                            old.AbbreviateEvents.Changed -= (x, y) => Screen.RequestDraw();
                            old.ColorEvents.Changed -= (x, y) => Screen.RequestDraw();
                            old.FontEvents.Changed -= (x, y) => Screen.RequestDraw();
                            old.HorizontalAlignmentEvents.Changed -= (x, y) => Screen.RequestDraw();
                            old.VerticalAlignmentEvents.Changed -= (x, y) => Screen.RequestDraw();
                            old.WrapEvents.Changed -= (x, y) => Screen.RequestDraw();
                        }
                        if (value != null)
                        {
                            value.AbbreviateEvents.Changed += (x, y) => Screen.RequestDraw();
                            value.ColorEvents.Changed += (x, y) => Screen.RequestDraw();
                            value.FontEvents.Changed += (x, y) => Screen.RequestDraw();
                            value.HorizontalAlignmentEvents.Changed += (x, y) => Screen.RequestDraw();
                            value.VerticalAlignmentEvents.Changed += (x, y) => Screen.RequestDraw();
                            value.WrapEvents.Changed += (x, y) => Screen.RequestDraw();
                        }
                    }
    }
}
