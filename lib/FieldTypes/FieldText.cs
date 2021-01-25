using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldText : Field
    {

        public FieldText(string displayName, string internalName): base(displayName, internalName) 
        { 
            FieldTypeName = "Single line of text";
            FieldType = TypeOfField.Text;
        }

    }
}
