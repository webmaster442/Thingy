using ManagedBass;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Thingy.MusicPlayerCore.Controls
{
    public class AudioWaveForm: FrameworkElement
    {
        private List<Point> _points;

        public void Update(int ChannelHandle)
        {
            int length = (int)Bass.ChannelSeconds2Bytes(ChannelHandle, 0.01);
            short[] data = new short[length / 2];
            length = Bass.ChannelGetData(ChannelHandle, data, length);
            double xscale = ActualWidth / data.Length;
            double yscale = ActualHeight / short.MaxValue;

            _points = new List<Point>(data.Length);

            for (int i = 1; i < data.Length; i += 2)
            {
                _points.Add(new Point(i * xscale, (data[i] - short.MaxValue) * yscale));
            }
            InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            for (int i=0; i< _points.Count -1; i+=2)
            {
                Point start = _points[i];
                Point end = _points[i + 1];
                drawingContext.DrawLine(new Pen(new SolidColorBrush(Colors.Black), 1), start, end);
            }
        }
    }
}
