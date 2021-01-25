using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldMultiChoice : Field
    {
        public List<string> Choices = new List<string>();

        public FieldMultiChoice(string displayName, string internalName): base(displayName, internalName) 
        { 
            FieldTypeName = "Choice";
            FieldType = TypeOfField.Choice;
        }

        public FieldMultiChoice(string displayName, string internalName, string[] choices): base(displayName, internalName) 
        { 
            FieldTypeName = "Choice";
            FieldType = TypeOfField.Choice;
            Choices.AddRange(choices);
        }

    }
}
