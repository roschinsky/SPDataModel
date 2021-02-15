using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldMultiLineText : Field
    {
        public bool IsRichTextEnabled { get; set; } = false;
        public bool IsRichTextFullHtml { get; set; } = false;
        public string RichTextMode { get { return IsRichTextFullHtml ? "FullHtml" : "Compatible"; } }
        public bool IsRichTextIsolatedStyles { get; set; } = true;
        public int NumLines {get; set;} = 6;
        public bool AppendOnly {get; set;} = false;

        public FieldMultiLineText(string displayName, string internalName) : base(displayName, internalName)
        {
            Init();
        }

        public FieldMultiLineText(string displayName, string internalName, int numLines) : base(displayName, internalName)
        {
            Init();
            NumLines = numLines;
        }

        public FieldMultiLineText(string displayName, string internalName, int numLines, bool supportsRichTextFullHtml) : base(displayName, internalName)
        {
            Init();
            NumLines = numLines;
            IsRichTextEnabled = true;
            IsRichTextFullHtml = supportsRichTextFullHtml;
        }

        public FieldMultiLineText(string displayName, string internalName, int numLines, bool isRichText, bool supportsRichTextFullHtml, bool canAppend, bool allowUnsafeHtml) : base(displayName, internalName)
        {
            Init();
            NumLines = numLines;
            IsRichTextEnabled = isRichText;
            IsRichTextFullHtml = supportsRichTextFullHtml;
            AppendOnly = canAppend;
            IsRichTextIsolatedStyles = !allowUnsafeHtml;
        }

        private void Init()
        {
            FieldTypeName = "Multiple lines of text";
            FieldType = TypeOfField.Note;
        }

    }
}
