using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib
{
    public abstract class ModelGenerator
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public ModelParser RelatedModelParser { get; }
        public Model Input { get; private set; }
        public object Output { get; protected set; }
        public Type OutputType { get; protected set; }
        public Uri RelatedApplication { get; protected set; }
        public string RelatedApplicationUsage { get; protected set; } = String.Empty;
        public ModelGeneratorSetting Settings { get; protected set; }

        public ModelGenerator(Model input)
        {
            if(input == null)
            {
                throw new ArgumentNullException("input", "A ModelGenerator must have been given a model as input when being created!");
            }

            Input = input;
            Settings = new ModelGeneratorSetting();
        }

        public virtual void Generate()
        {
            throw new NotImplementedException("Use the implemented generator for a specific processing.");
        }

        public override string ToString()
        {
            Type thisIsMadeOf = this.GetType();
            if(thisIsMadeOf.IsAbstract)
            {
                return "Nuffn here - its just the generator blueprints...";
            }
            else
            {
                var outputString = Output != null ? String.Concat("'", String.Concat(Output).Substring(0, 12),"'") : "nothing";
                return String.Format("{0} generated a {1} '{2}'.", thisIsMadeOf.Name, OutputType.Name, outputString);
            }
        }
    }
}
