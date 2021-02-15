using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldUser : FieldLookup
    {
        public int UserSelectionMode { get; set; } = 0;
        public int UserSelectionScope { get; set; } = 0;

        public FieldUser(string displayName, string internalName, bool isGroup) : base(displayName, internalName, null)
        {
            FieldTypeName = "Person or Group";
            UserSelectionMode = isGroup ? 1 : 0;
            FieldType = TypeOfField.User;
        }

    }
}
