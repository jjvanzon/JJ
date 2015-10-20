﻿using JJ.Framework.Presentation.VectorGraphics.Gestures;
using JJ.Framework.Presentation.VectorGraphics.Models.Elements;
using VectorGraphicsElements = JJ.Framework.Presentation.VectorGraphics.Models.Elements;

namespace JJ.Framework.Presentation.WinForms.TestForms.Helpers
{
    internal static class VectorGraphicsFactory
    {
        public static Diagram CreateTestVectorGraphicsModel()
        {
            var diagram = new Diagram();

            Rectangle background = diagram.Background;

            Rectangle rectangle1 = CreateRectangle(diagram, 200, 10, "Block 1");

            Rectangle rectangle2 = CreateRectangle(diagram, 10, 200, "Block 2");

            var point1 = new Point
            {
                Diagram = diagram,
                Parent = rectangle1,
                X = 150,
                Y = 30,
                PointStyle = VectorGraphicsHelper.InvisiblePointStyle
            };

            var point2 = new Point  
            {
                Diagram = diagram,
                Parent = rectangle2,
                X = 150,
                Y = 30,
                PointStyle = VectorGraphicsHelper.InvisiblePointStyle
            };

            var line = new Line
            {
                Diagram = diagram,
                Parent = diagram.Background,
                PointA = point1,
                PointB = point2,
                LineStyle = VectorGraphicsHelper.DefaultLineStyle
            };

            line.ZIndex = -1;

            return diagram;
        }

        private static Rectangle CreateRectangle(Diagram diagram, float x, float y, string text)
        {
            var rectangle = new Rectangle()
            {
                Diagram = diagram,
                Parent = diagram.Background,
                X = x,
                Y = y,
                Width = 300,
                Height = 60,
                LineStyle = VectorGraphicsHelper.DefaultLineStyle
            };
            rectangle.Gestures.Add(new MoveGesture());

            var label = new Label 
            {
                Diagram = diagram,
                Parent = rectangle,
                Width = 300,
                Height = 60,
                Text = text,
                TextStyle = VectorGraphicsHelper.DefaultTextStyle
            };

            return rectangle;
        }

        public static Rectangle CreateRectangle(Diagram diagram, string text)
        {
            var rectangle = new Rectangle()
            {
                Diagram = diagram,
                Parent = diagram.Background,
                X = VectorGraphicsHelper.SPACING,
                Y = VectorGraphicsHelper.SPACING,
                Width = VectorGraphicsHelper.BLOCK_WIDTH,
                Height = VectorGraphicsHelper.BLOCK_HEIGHT,
                BackStyle = VectorGraphicsHelper.BlueBackStyle,
                LineStyle = VectorGraphicsHelper.DefaultLineStyle
            };

            var label = new Label
            {
                Diagram = diagram,
                Parent = rectangle,
                Text = text,
                X = 0,
                Y = 0,
                Width = VectorGraphicsHelper.BLOCK_WIDTH,
                Height = VectorGraphicsHelper.BLOCK_HEIGHT,
                TextStyle = VectorGraphicsHelper.DefaultTextStyle
            };

            return rectangle;
        }

    }
}
