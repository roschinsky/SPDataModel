using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldUser : FieldLookup
    {
        

        public FieldUser(string displayName, string internalName, bool isGroup): base(displayName, internalName, null) 
        { 
            FieldTypeName = "Person or Group";
            FieldType = isGroup ? TypeOfField.LookupGroup : TypeOfField.LookupUser;
        }

        public FieldUser(string displayName, string internalName, Relation relatedTo, bool isGroup) : base(displayName, internalName, relatedTo)
        {
            FieldTypeName = "Person or Group";
            FieldType = isGroup ? TypeOfField.LookupGroup : TypeOfField.LookupUser;
            RelatedTo = relatedTo;            
        }

    }
}
