using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TRoschinsky.SPDataModel.Lib.FieldTypes;

namespace TRoschinsky.SPDataModel.Lib
{
    public class Model
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public readonly DateTime CreatedOn = DateTime.Now;
        public readonly Guid ModelId = Guid.NewGuid();
        public Uri Url { get; private set; }
        public List<Entity> Entities { get => entities; }
        private List<Entity> entities = new List<Entity>();
        public CultureInfo CultureToUse {get; private set;} = CultureInfo.CurrentCulture;
        private Dictionary<string, string> customRessources = new Dictionary<string, string>();

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

        private void Initialize()
        {
            // get resources for globalization
            customRessources = Commons.LoadCustomResources("Defaults", CultureToUse);

            if(customRessources.ContainsKey("_ERROR"))
            {
                throw new InvalidDataException("Initialization failed while loading resoures: " + customRessources["_ERROR"]);
            }

            // add default list
            if(entities.Find(e => e.InternalName == customRessources["lin_ual"]) == null)
            {
                Entity ual = new Entity(customRessources["ldn_ual"] , customRessources["lin_ual"]);
                ual.IsSystem = true;
                ual.IsHidden = true;
                Entities.Add(ual);
            }
        }

        public void AddEntity(Entity entity)
        {
            entity.AddFieldRange(GetDefaulListFields().ToArray());
            entities.Add(entity);
        }

        public void AddEntityRange(Entity[] multipleEntities)
        {
            foreach(Entity entity in multipleEntities)
            {
                entity.AddFieldRange(GetDefaulListFields().ToArray());
                entities.Add(entity);
            }
        }

        public Entity GetEntityByName(string entityName)
        {
            Entity result = entities.Find(e => e.DisplayName == entityName);
            return result;
        }

#region Helper

        public List<Field> GetDefaulListFields()
        {
            List<Field> fields = new List<Field>();
            fields.Add(new FieldComputed("ID", "id") { IsSystem = true });
            fields.Add(new FieldText(customRessources["fdn_title"], customRessources["fin_title"]));
            fields.Add(new FieldDateTime(customRessources["fdn_created"], customRessources["fin_created"]) { IsSystem = true });
            fields.Add(new FieldUser(customRessources["fdn_author"], customRessources["fin_author"], false) { IsSystem = true });
            fields.Add(new FieldDateTime(customRessources["fdn_modified"], customRessources["fin_modified"]) { IsSystem = true });
            fields.Add(new FieldUser(customRessources["fdn_editor"], customRessources["fin_editor"], false) { IsSystem = true });
            return fields;
        }

#endregion

    }
}
