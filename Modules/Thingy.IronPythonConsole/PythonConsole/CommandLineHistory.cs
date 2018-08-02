// Copyright (c) 2010 Joe Moorhouse

using System.Collections.Generic;

namespace PythonConsoleControl
{
    /// <summary>
    /// Stores the command line history for the PythonConsole.
    /// </summary>
    public class CommandLineHistory
    {
        private List<string> _lines = new List<string>();
        private int _position;

        public CommandLineHistory()
        {
        }

        /// <summary>
        /// Gets the current command line. By default this will be the last command line entered.
        /// </summary>
        public string Current
        {
            get
            {
                if ((_position >= 0) && (_position < _lines.Count))
                {
                    return _lines[_position];
                }
                return null;
            }
        }

        /// <summary>
        /// Adds the command line to the history.
        /// </summary>
        public void Add(string line)
        {
            if (!string.IsNullOrEmpty(line))
            {
                int index = _lines.Count - 1;
                if (index >= 0)
                {
                    if (_lines[index] != line)
                    {
                        _lines.Add(line);
                    }
                }
                else
                {
                    _lines.Add(line);
                }
            }
            _position = _lines.Count;
        }
        /// <summary>
        /// Moves to the next command line.
        /// </summary>
        /// <returns>False if the current position is at the end of the command line history.</returns>
        public bool MoveNext()
        {
            int nextPosition = _position + 1;
            if (nextPosition < _lines.Count)
            {
                ++_position;
            }
            return nextPosition < _lines.Count;
        }

        /// <summary>
        /// Moves to the previous command line.
        /// </summary>
        /// <returns>False if the current position is at the start of the command line history.</returns>
        public bool MovePrevious()
        {
            if (_position >= 0)
            {
                if (_position == 0)
                {
                    return false;
                }
                --_position;
            }
            return _position >= 0;
        }
    }
}
