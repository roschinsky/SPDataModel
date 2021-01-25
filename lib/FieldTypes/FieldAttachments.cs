using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldAttachments : Field
    {

        public FieldAttachments(string displayName, string internalName): base(displayName, internalName) 
        { 
            FieldTypeName = "Attachments";
            FieldType = TypeOfField.File;
        }

    }
}
