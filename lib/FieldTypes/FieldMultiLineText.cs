using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldMultiLineText : Field
    {

        public FieldMultiLineText(string displayName, string internalName): base(displayName, internalName) 
        { 
            FieldTypeName = "Multiple lines of text";
            FieldType = TypeOfField.Note;
        }

    }
}
