using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldDateTime : Field
    {
        public bool ShowDateAndTime { get; set; } = false;
        public FieldDateTime(string displayName, string internalName) : base(displayName, internalName)
        {
            FieldTypeName = "Date and Time";
            FieldType = TypeOfField.DateTime;
        }

        public FieldDateTime(string displayName, string internalName, bool showDateAndTime) : base(displayName, internalName)
        {
            FieldTypeName = "Date and Time";
            FieldType = TypeOfField.DateTime;
            ShowDateAndTime = showDateAndTime;
        }

    }
}
