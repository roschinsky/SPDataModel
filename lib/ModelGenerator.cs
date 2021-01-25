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
        public object Output { get; private set; }
        public Uri RelatedApplication { get; private set; }
        public string RelatedApplicationUsage { get; private set; } = String.Empty;

        public ModelGenerator(Model input)
        {
            Input = input;
        }

        public void Generate()
        {
            throw new NotImplementedException("Use the implemented generator for a specific output.");
        }
    }
}
