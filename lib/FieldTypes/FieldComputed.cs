using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldComputed : Field
    {

        public FieldComputed(string displayName, string internalName): base(displayName, internalName) 
        { 
            FieldTypeName = String.Empty;
            FieldType = TypeOfField.System;
            IsSystem = true;
        }

    }
}
