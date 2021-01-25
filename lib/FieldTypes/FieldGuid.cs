using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldGuid : Field
    {

        public FieldGuid(string displayName, string internalName): base(displayName, internalName) 
        { 
            FieldTypeName = String.Empty;
            FieldType = TypeOfField.Complex;
            IsSystem = true;
        }

    }
}
