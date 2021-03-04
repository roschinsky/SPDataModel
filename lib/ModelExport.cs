using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TRoschinsky.SPDataModel.Lib
{
    public class ModelExport
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [JsonInclude]
        public long CreatedOn;
        [JsonInclude]
        public string ModelId;
        public string Url { get; set; }
        public List<Entity> Entities { get; set; } = new List<Entity>();
        public int CultureToUse { get; set; }
        public bool RelationsAreResolved { get; set; }
        public string DefaultUilInternalName { get; set; }

        public ModelExport()
        {

        }

        public ModelExport(Model modelToExort)
        {
            Name = modelToExort.Name;
            Description = modelToExort.Description;
            CreatedOn = modelToExort.CreatedOn.Ticks;
            ModelId = modelToExort.ModelId.ToString();
            Url = modelToExort.Url != null ? modelToExort.Url.ToString() : String.Empty;
            Entities.AddRange(modelToExort.Entities);
            CultureToUse = modelToExort.CultureToUse != null ? modelToExort.CultureToUse.LCID : 0;
            RelationsAreResolved = modelToExort.RelationsAreResolved;
            DefaultUilInternalName = modelToExort.DefaultUilInternalName;
        }

        public Model GetModel()
        {
            Model result = new Model(this);

            return result;
        }

        public override string ToString()
        {
            return String.Format("Exported model with {1} entit{2}", Name, Entities.Count, Entities.Count != 1 ? "ies" : "y");
        }

        #region Helper

        #endregion
    }
}
