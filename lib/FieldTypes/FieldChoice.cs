using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldChoice : Field
    {
        public List<string> Choices = new List<string>();
        public bool ShowAsDropdown { get; set; } = true;
        public bool FillInChoice { get; set; } = false;
        public override string Format { get { return ShowAsDropdown ? "Dropdown" : "RadioButtons"; } }

        public FieldChoice(string displayName, string internalName) : base(displayName, internalName)
        {
            FieldTypeName = "Choice";
            FieldType = TypeOfField.Choice;
        }

        public FieldChoice(string displayName, string internalName, string[] choices) : base(displayName, internalName)
        {
            FieldTypeName = "Choice";
            FieldType = TypeOfField.Choice;
            Choices.AddRange(choices);
        }

    }
}
