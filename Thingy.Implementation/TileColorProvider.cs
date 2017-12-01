using System;
using System.Windows.Media;

namespace Thingy.Implementation
{
    public static class TileColorProvider
    {
        private static int[] _colors;

        static TileColorProvider()
        {
            _colors = new int[]
            {
                0x99b433,
                0x00a300,
                0x1e7145,
                0xff0097,
                0x9f00a7,
                0x7e3878,
                0x603cba,
                0x1d1d1d,
                0x00aba9,
                0x2d89ef,
                0x2b5797,
                0xffc40d,
                0xe3a21a,
                0xda532c,
                0xee1111,
                0xb91d47
            };
        }

        public static Color GetColor(int index)
        {
            var color = _colors[Math.Abs(index % _colors.Length)];
            var r = (byte)((color & 0x00FF0000) >> 16);
            var g = (byte)((color & 0x0000FF00) >> 8);
            var b = (byte)((color & 0x000000FF));
            return Color.FromRgb(r, g, b);
        }
    }
}
