using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using TRoschinsky.SPDataModel.Lib.FieldTypes;

namespace TRoschinsky.SPDataModel.Lib
{
    public class Model
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOn { get; private set; }
        public Guid ModelId { get; private set; }
        public Uri Url { get; private set; }
        public List<Entity> Entities { get => entities; }
        private List<Entity> entities = new List<Entity>();
        public CultureInfo CultureToUse { get; private set; } = CultureInfo.CurrentCulture;
        private Dictionary<string, string> customRessources = new Dictionary<string, string>();
        public bool RelationsAreResolved { get; private set; } = false;

        public string DefaultUilInternalName { get; private set; }

        public Model(string name)
        {
            Name = name;
            Initialize();
        }
        public Model(string name, Uri uri)
        {
            Name = name;
            Url = uri;
            Initialize();
        }
        public Model(string name, Uri uri, CultureInfo cultureToUse)
        {
            Name = name;
            Url = uri;
            CultureToUse = cultureToUse;
            Initialize();
        }

        public Model(ModelExport exportedModel)
        {
            Name = exportedModel.Name;
            Description = exportedModel.Description;
            CreatedOn = exportedModel.CreatedOn != 0 ? new DateTime(exportedModel.CreatedOn) : DateTime.Now;
            ModelId = !String.IsNullOrEmpty(exportedModel.ModelId) ? new Guid(exportedModel.ModelId) : Guid.NewGuid();
            Url = !String.IsNullOrEmpty(exportedModel.Url) ? new Uri(exportedModel.Url) : null;
            CultureToUse = exportedModel.CultureToUse != 0 ? new CultureInfo(exportedModel.CultureToUse) : CultureInfo.CurrentCulture;
            
            // get resources for globalization
            customRessources = Commons.LoadCustomResources("Defaults", CultureToUse);

            if (customRessources.ContainsKey("_ERROR"))
            {
                throw new InvalidDataException("Initialization failed while loading resoures: " + customRessources["_ERROR"]);
            }
            
            AddEntityRange(exportedModel.Entities.ToArray());
            DefaultUilInternalName = exportedModel.DefaultUilInternalName;
        }

        private void Initialize()
        {
            // set some basic references
            ModelId = Guid.NewGuid();
            CreatedOn = DateTime.Now;

            // get resources for globalization
            customRessources = Commons.LoadCustomResources("Defaults", CultureToUse);

            if (customRessources.ContainsKey("_ERROR"))
            {
                throw new InvalidDataException("Initialization failed while loading resoures: " + customRessources["_ERROR"]);
            }

            // add default list
            if (entities.Find(e => e.InternalName == customRessources["lin_uil"]) == null)
            {
                DefaultUilInternalName = customRessources["lin_uil"];
                Entity uil = new Entity(customRessources["ldn_uil"], customRessources["lin_uil"]);
                uil.IsSystem = true;
                uil.IsHidden = true;
                uil.IsUil = true;
                entities.Add(uil);
            }
        }

        public void AddEntity(Entity entity)
        {
            entity.AddFieldRange(GetDefaulListFields().ToArray());
            entities.Add(entity);
            RelationsAreResolved = ResolveRelations();
        }

        public void AddEntityRange(Entity[] multipleEntities)
        {
            foreach (Entity entity in multipleEntities)
            {
                entity.AddFieldRange(GetDefaulListFields().ToArray());
                entities.Add(entity);
            }
            RelationsAreResolved = ResolveRelations();
        }

        public Entity GetEntityByName(string entityName)
        {
            return entities.Find(e => e.DisplayName == entityName);
        }

        public Entity GetEntityByInternalName(string entityInternalName)
        {
            return entities.Find(e => e.InternalName == entityInternalName);
        }

        public override string ToString()
        {
            return String.Format("'{0}' with {1} entit{2} (@ {3:d})", Name, entities.Count, entities.Count != 1 ? "ies" : "y", CreatedOn);
        }

        #region Helper

        public List<Field> GetDefaulListFields()
        {
            List<Field> fields = new List<Field>();
            fields.Add(new FieldComputed("ID", "id") { IsSystem = true });
            fields.Add(new FieldText(customRessources["fdn_title"], customRessources["fin_title"]));
            fields.Add(new FieldDateTime(customRessources["fdn_created"], customRessources["fin_created"]) { IsSystem = true });
            fields.Add(new FieldUser(customRessources["fdn_author"], customRessources["fin_author"], false, DefaultUilInternalName) { IsSystem = true });
            fields.Add(new FieldDateTime(customRessources["fdn_modified"], customRessources["fin_modified"]) { IsSystem = true });
            fields.Add(new FieldUser(customRessources["fdn_editor"], customRessources["fin_editor"], false, DefaultUilInternalName) { IsSystem = true });
            return fields;
        }

        public bool ResolveRelations()
        {
            bool allResolved = true;
            foreach (Entity entity in entities)
            {
                if (!entity.ResolveRelations(this) && allResolved)
                {
                    allResolved = false;
                }
            }
            return allResolved;
        }

        #endregion
    }
}
