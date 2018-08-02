// Copyright (c) 2010 Joe Moorhouse

using ICSharpCode.AvalonEdit.Document;

namespace PythonConsoleControl
{
    internal struct VerySimpleSegment : ISegment
    {
        public readonly int _Offset, _Length;

        public VerySimpleSegment(int offset, int length)
        {
            _Offset = offset;
            _Length = length;
        }

        public int EndOffset
        {
            get
            {
                return _Offset + _Length;
            }
        }

        int ISegment.Length
        {
            get { return _Length; }
        }

        int ISegment.Offset
        {
            get { return _Offset; }
        }
    }
}
