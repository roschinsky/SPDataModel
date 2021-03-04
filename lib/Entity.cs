using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TRoschinsky.SPDataModel.Lib.FieldTypes;

namespace TRoschinsky.SPDataModel.Lib
{
    public class Entity
    {
        public string DisplayName { get; set; } = "Unknown";
        public string Description { get; set; } = String.Empty;
        public string InternalName { get; set; } = "NoName";
        public Uri Url { get; set; }
        public bool IsHidden { get; set; } = false;
        public bool IsSystem { get; set; } = false;
        public bool IsUil { get; set; } = false;

        public Field[] Fields { get { return fields.ToArray(); } }
        private List<Field> fields { get; set; } = new List<Field>();
        [JsonIgnore]
        public Relation[] Relations { get { return relations.ToArray(); } }
        private List<Relation> relations { get { return GetRelations(false); } }

        public Entity()
        {
            Initialize();
        }

        public Entity(string displayName, string internalName)
        {
            DisplayName = displayName;
            InternalName = internalName;
            Initialize();
        }

        public Entity(string displayName, string internalName, string description)
        {
            DisplayName = displayName;
            InternalName = internalName;
            Description = description;
            Initialize();
        }

        private void Initialize()
        {
            //fields.AddRange(Model.GetDefaulListFields());
        }

        public void AddField(Field field)
        {
            if(fields.FindIndex(f => f.InternalName == field.InternalName) < 0)
            {
                fields.Add(field);
            }
        }

        public void AddFieldRange(Field[] multipleFields)
        {
            foreach(Field field in multipleFields)
            {
                if(fields.FindIndex(f => f.InternalName == field.InternalName) < 0)
                {
                    fields.Add(field);
                }
            }
        }

        public List<Relation> GetRelations(bool includeOnlyResolved)
        {
            List<Relation> relations = new List<Relation>();

            foreach (Field field in fields.FindAll(f => f.RelatedTo != null && !f.IsSystem))
            {
                if(!includeOnlyResolved || field.RelatedTo.IsResolvedRelation)
                {
                    relations.Add(field.RelatedTo);
                }
            }

            return relations;
        }

        public bool ResolveRelations(Model model)
        {
            bool allResolved = true;
            foreach(Relation relation in relations)
            {
                if(!relation.Resolve(model, this) && allResolved)
                {
                    allResolved = false;
                }
            }
            return allResolved;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool showSystemAndHidden)
        {
            string fieldList = "0";
            string relationList = "0";

            try
            {
                List<Field> fieldsToList = showSystemAndHidden ? fields : fields.FindAll(f => !f.IsSystem && !f.IsHidden);
                fieldList = String.Format("[{0}] {{ ", fieldsToList.Count);

                for (int i = 0; i < fieldsToList.Count && i <= 5; i++)
                {
                    fieldList += fieldsToList[i].InternalName + ", ";
                    if (i == 5 && fieldsToList.Count > 6)
                    {
                        fieldList += "...";
                    }
                }
                fieldList = fieldList.TrimEnd(new char[] { ' ', ',' });
                fieldList += " }";

                relationList = String.Format("[{0}] {{ ", relations.Count);
                for (int i = 0; i < relations.Count && i <= 5; i++)
                {
                    relationList += relations[i].LookupToEntityName + ", ";
                    if (i == 5 && relations.Count > 6)
                    {
                        relationList += "...";
                    }
                }
                relationList = relationList.TrimEnd(new char[] { ' ', ',' });
                relationList += " }";
            }
            catch (Exception ex)
            {
                throw new Exception("Whoops: " + ex.Message);
            }

            return String.Format(
                "{0} ('{1}') F:{2} R:{3}{4}{5}",
                InternalName,
                DisplayName,
                fieldList,
                relationList,
                IsSystem ? " +System" : String.Empty,
                IsHidden ? " +Hidden" : String.Empty);
        }
    }
}
