using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldDateTime : Field
    {

        public FieldDateTime(string displayName, string internalName): base(displayName, internalName) 
        { 
            FieldTypeName = "Date and Time";
            FieldType = TypeOfField.DateTime;
        }

    }
}
