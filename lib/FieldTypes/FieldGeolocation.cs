using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldGeolocation : Field
    {

        public FieldGeolocation(string displayName, string internalName): base(displayName, internalName) 
        { 
            FieldTypeName = "Location";
            FieldType = TypeOfField.Complex;
        }

    }
}
