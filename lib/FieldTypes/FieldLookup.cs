using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldLookup : Field
    {
        public bool IsMultiLookup { get; set; } = false;

        public FieldLookup(string displayName, Relation relatedTo) : base(displayName, String.Format("lkup{0}", relatedTo.LookupToEntity.InternalName))
        {
            FieldTypeName = "Lookup";
            FieldType = TypeOfField.Lookup;
            RelatedTo = relatedTo;
        }

        public FieldLookup(string displayName, string internalName, Relation relatedTo) : base(displayName, internalName)
        {
            FieldTypeName = "Lookup";            
            FieldType = TypeOfField.Lookup;
            RelatedTo = relatedTo;
        }

    }
}
