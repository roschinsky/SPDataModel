using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldRecurrence : Field
    {

        public FieldRecurrence(string displayName, string internalName): base(displayName, internalName) 
        { 
            FieldTypeName = String.Empty;
            FieldType = TypeOfField.Complex;
            IsSystem = true;
        }

    }
}
