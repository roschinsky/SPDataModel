using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldMultiChoice : FieldChoice
    {

        public FieldMultiChoice(string displayName, string internalName): base(displayName, internalName) 
        { 
            FieldTypeName = "Choice";
            FieldType = TypeOfField.MultiChoice;
        }

        public FieldMultiChoice(string displayName, string internalName, string[] choices): base(displayName, internalName, choices) 
        { 
            FieldTypeName = "Choice";
            FieldType = TypeOfField.MultiChoice;
        }

    }
}
