using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldLookup : Field
    {
        public enum RelationshipBehaviorTypes
        {
            Restrict,
            Cascade,
            None
        };

        public bool IsMultiLookup { get; set; } = false;
        public string List { get; set; } = String.Empty;
        public string ShowField { get; set; } = "Title";
        public RelationshipBehaviorTypes RelationshipDeleteBehavior { get; set; } = RelationshipBehaviorTypes.None;

        public FieldLookup(string displayName, Relation relatedTo) : base(displayName, String.Format("lkup{0}", relatedTo.LookupToEntityName))
        {
            FieldTypeName = "Lookup";
            FieldType = TypeOfField.Lookup;
            RelatedTo = relatedTo;
            if (RelatedTo != null && RelatedTo.LookupToEntity != null)
            {
                List = RelatedTo.LookupToEntityName;
            }
        }

        public FieldLookup(string displayName, string internalName, Relation relatedTo) : base(displayName, internalName)
        {
            FieldTypeName = "Lookup";
            FieldType = TypeOfField.Lookup;
            RelatedTo = relatedTo;
            if (RelatedTo != null)
            {
                List = RelatedTo.LookupToEntityName;
            }
        }

        public FieldLookup(string displayName, string internalName, string relatedToListName) : base(displayName, internalName)
        {
            FieldTypeName = "Lookup";
            FieldType = TypeOfField.Lookup;
            RelatedTo = new Relation(relatedToListName);
            List = relatedToListName;
        }

    }
}
