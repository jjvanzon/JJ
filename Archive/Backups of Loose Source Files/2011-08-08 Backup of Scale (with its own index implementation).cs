//2011-08-08

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Circle.Code.Events;
using System.Drawing;
using Circle.Client.WinForms.Helpers;

namespace Circle.Graphics.Objects
{
    public class Scale : Operator
    {
        // Opacity

        public float Opacity
        {
            get { return OpacityEvents.Value; }
            set { OpacityEvents.Value = value; }
        }

        public readonly Events<float> OpacityEvents = new Events<float>();

        // Width

        public int Width
        {
            get { return WidthEvents.Value; }
            set { WidthEvents.Value = value; }
        }

        public readonly Events<int> WidthEvents = new Events<int>();

        // Height

        public int Height
        {
            get { return HeightEvents.Value; }
            set { HeightEvents.Value = value; }
        }

        public readonly Events<int> HeightEvents = new Events<int>();

        // Output

        public override Image Output(bool cache = true)
        {
            Image output = null;

            if (cache)
            {
                output = Cache[Input, Opacity, Width, Height];
            }

            if (output == null)
            {
                output = GraphicsHelper.ScaleImage(Input, new Rectangle(0, 0, Width, Height), Opacity);

                if (cache)
                {
                    Cache[Input, Opacity, Width, Height] = output;
                }
            }

            return output;
        }

        // Cache

        private static readonly CacheClass Cache = new CacheClass();

        // TODO: program an Index class.

        public class CacheClass
        {
            private Dictionary<Image, Dictionary<double, Dictionary<double, Dictionary<float, Image>>>> _storage = 
                new Dictionary<Image, Dictionary<double, Dictionary<double, Dictionary<float, Image>>>>();

            public Image this[Image input, double width, double height, float opacity]
            {
                get 
                {
                    if (_storage.ContainsKey(input))
                    {
                        var inputEntry = _storage[input];

                        if (inputEntry.ContainsKey(width))
                        {
                            var widthEntry = inputEntry[width];

                            if (widthEntry.ContainsKey(height))
                            {
                                var heightEntry = widthEntry[height];

                                if (heightEntry.ContainsKey(opacity))
                                {
                                    return heightEntry[opacity];
                                }
                            }
                        }
                    }

                    return null;
                }
                set 
                {
                    Dictionary<double, Dictionary<double, Dictionary<float, Image>>> inputEntry = null;

                    if (_storage.ContainsKey(input))
                    {
                        inputEntry = _storage[input];
                    }
                    else
                    {
                        inputEntry = new Dictionary<double, Dictionary<double, Dictionary<float, Image>>>();
                        _storage[input] = inputEntry;
                    }

                    Dictionary<double, Dictionary<float, Image>> widthEntry = null;

                    if (inputEntry.ContainsKey(width))
                    {
                        widthEntry = inputEntry[width];
                    }
                    else
                    {
                        widthEntry = new Dictionary<double, Dictionary<float, Image>>();
                        inputEntry[width] = widthEntry;
                    }
                    
                    Dictionary<float, Image> heightEntry = null;

                    if (widthEntry.ContainsKey(height))
                    {
                        heightEntry = widthEntry[height];
                    }
                    else
                    {
                        heightEntry = new Dictionary<float, Image>();
                        widthEntry[height] = heightEntry;
                    }

                    heightEntry[opacity] = value;
                }
            }

            public bool Contains(Image input, double width, double height, float opacity)
            {
                if (_storage.ContainsKey(input))
                {
                    var inputEntry = _storage[input];

                    if (inputEntry.ContainsKey(width))
                    {
                        var widthEntry = inputEntry[width];

                        if (widthEntry.ContainsKey(height))
                        {
                            var heightEntry = widthEntry[height];

                            if (heightEntry.ContainsKey(opacity))
                            {
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
        }
    }
}
