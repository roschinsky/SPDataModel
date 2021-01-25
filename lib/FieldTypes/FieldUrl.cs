using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldUrl : Field
    {
        public bool IsHyperlink { get; set; } = true;

        public FieldUrl(string displayName, string internalName): base(displayName, internalName) 
        { 
            FieldTypeName = "Hyperlink or Picture";
            FieldType = TypeOfField.Url;
        }
        public FieldUrl(string displayName, string internalName, bool isHyperlink): base(displayName, internalName) 
        { 
            FieldTypeName = "Hyperlink or Picture";
            IsHyperlink = isHyperlink;
            FieldType = TypeOfField.Url;
        }

    }
}
