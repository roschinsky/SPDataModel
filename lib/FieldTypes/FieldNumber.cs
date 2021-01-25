using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldNumber : Field
    {

        public FieldNumber(string displayName, string internalName): base(displayName, internalName) 
        { 
            FieldTypeName = "Number";
            FieldType = TypeOfField.Decimal;
        }

    }
}
