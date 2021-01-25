using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldBoolean : Field
    {

        public FieldBoolean(string displayName, string internalName): base(displayName, internalName) 
        { 
            FieldTypeName = "Yes/No";
            FieldType = TypeOfField.Boolean;
        }

    }
}
