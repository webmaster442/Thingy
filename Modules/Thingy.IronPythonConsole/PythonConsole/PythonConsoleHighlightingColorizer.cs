// Copyright (c) 2010 Joe Moorhouse

using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using System;

namespace PythonConsoleControl
{
    /// <summary>
    /// Only colourize when text is input
    /// </summary>
    public class PythonConsoleHighlightingColorizer : HighlightingColorizer
    {
        private TextDocument _document;

        /// <inheritdoc/>
        protected override void ColorizeLine(DocumentLine line)
        {
            string lineString = _document.GetText(line);
            if (CurrentContext.TextView.Services.GetService(typeof(IHighlighter)) is IHighlighter highlighter)
            {
                if (lineString.Length < 3 || lineString.Substring(0, 3) == ">>>" || lineString.Substring(0, 3) == "...")
                {
                    HighlightedLine hl = highlighter.HighlightLine(line.LineNumber);
                    foreach (HighlightedSection section in hl.Sections)
                    {
                        ChangeLinePart(section.Offset, section.Offset + section.Length,
                                       visualLineElement => ApplyColorToElement(visualLineElement, section.Color));
                    }
                }
                else
                { // Could add foreground colour functionality here.
                }
            }
        }

        /// <summary>
        /// Creates a new HighlightingColorizer instance.
        /// </summary>
        /// <param name="ruleSet">The root highlighting rule set.</param>
        public PythonConsoleHighlightingColorizer(IHighlightingDefinition highlightingDefinition, TextDocument document)
            : base(new DocumentHighlighter(document, highlightingDefinition))
        {
            _document = document ?? throw new ArgumentNullException("document");
        }
    }
}
