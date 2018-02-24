﻿using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Thingy.Views.Notes
{
    /// <summary>
    /// Interaction logic for NoteEditor.xaml
    /// </summary>
    public partial class NoteEditor : UserControl, INoteEditor
    {
        public NoteEditor()
        {
            InitializeComponent();
        }

        public void ClearText()
        {
            TextEditor.Clear();
        }

        public void LoadFile(string file)
        {
            TextEditor.Load(file);
        }

        public void Print(string filename)
        {
            PrintDialog pDialog = new PrintDialog();
            var print = pDialog.ShowDialog();
            if (print == true)
            {
                var document = CreateFlowDocumentForEditor(TextEditor);
                IDocumentPaginatorSource idpSource = document;
                pDialog.PrintDocument(idpSource.DocumentPaginator, filename);
            }
        }

        public void SaveFile(string file)
        {
            TextEditor.Save(file);
        }

        private void ConfigureEditorOptions(object sender, RoutedEventArgs e)
        {
            TextEditor.Options.ShowEndOfLine = ViewLineEndings.IsChecked;
            TextEditor.Options.ShowTabs = ViewWhitespace.IsChecked;
            TextEditor.Options.ShowSpaces = ViewWhitespace.IsChecked;
        }

        private static FlowDocument CreateFlowDocumentForEditor(TextEditor editor)
        {
            IHighlighter highlighter = editor.TextArea.GetService(typeof(IHighlighter)) as IHighlighter;
            FlowDocument doc = new FlowDocument(ConvertTextDocumentToBlock(editor.Document, highlighter));
            doc.FontFamily = editor.FontFamily;
            doc.FontSize = editor.FontSize;
            return doc;
        }

        private static Block ConvertTextDocumentToBlock(TextDocument document, IHighlighter highlighter)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            Paragraph p = new Paragraph();
            foreach (DocumentLine line in document.Lines)
            {
                int lineNumber = line.LineNumber;
                HighlightedInlineBuilder inlineBuilder = new HighlightedInlineBuilder(document.GetText(line));
                if (highlighter != null)
                {
                    HighlightedLine highlightedLine = highlighter.HighlightLine(lineNumber);
                    int lineStartOffset = line.Offset;
                    foreach (HighlightedSection section in highlightedLine.Sections)
                        inlineBuilder.SetHighlighting(section.Offset - lineStartOffset, section.Length, section.Color);
                }
                p.Inlines.AddRange(inlineBuilder.CreateRuns());
                p.Inlines.Add(new LineBreak());
            }
            return p;
        }
    }
}