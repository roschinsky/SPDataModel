using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldFile : Field
    {

        public FieldFile(string displayName, string internalName): base(displayName, internalName) 
        { 
            FieldTypeName = String.Empty;
            FieldType = TypeOfField.File;
        }

    }
}
