using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib
{
    public abstract class Field
    {
        public string DisplayName { get; set; } = "Unknown";
        public string InternalName { get; set; } = "NoName";
        public string FieldTypeName { get; protected set; }
        public TypeOfField FieldType = TypeOfField.Undefined;
        public bool IsHidden { get; set; } = false;
        public bool IsSystem { get; set; } = false;

        public Relation RelatedTo { get; set; }
        public string Description { get; set; } = String.Empty;
        public bool IsRequiredField { get; set; } = false;
        public bool InitialAddToView { get; set; } = false;
        public virtual string Format { get; protected set; } = String.Empty;
        public string Default { get; set; } = String.Empty;

        public Field() { }

        public Field(string displayName, string internalName)
        {
            DisplayName = displayName;
            InternalName = internalName;
        }

        public Field(string displayName, string internalName, bool isSystem)
        {
            DisplayName = displayName;
            InternalName = internalName;
            IsSystem = isSystem;
        }

        public override string ToString()
        {
            return String.Format(
                "{0} ('{1}') [{2}]{3}{4}{5}",
                InternalName,
                DisplayName,
                FieldTypeName,
                IsSystem ? " +System" : String.Empty,
                IsHidden ? " +Hidden" : String.Empty,
                RelatedTo != null ? " +Lookup " + (RelatedTo.IsMultiLookup ? "<=> " : "=> ") + RelatedTo.LookupToEntity.InternalName : String.Empty);
        }

        public enum TypeOfField
        {
            Number,
            DateTime,
            Text,
            Note,
            File,
            Url,
            Boolean,
            Choice,
            MultiChoice,
            Lookup,
            User,
            Complex,
            System,
            Undefined
        }
    }
}
