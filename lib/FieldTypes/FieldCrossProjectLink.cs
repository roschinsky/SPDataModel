using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldCrossProjectLink : Field
    {

        public FieldCrossProjectLink(string displayName, string internalName): base(displayName, internalName) 
        { 
            FieldTypeName = String.Empty;
            FieldType = TypeOfField.Lookup;
            IsSystem = true;
        }

    }
}
