using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib
{
    public abstract class ModelParser
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public ModelParser RelatedModelParser { get; }
        public object Input { get; private set; }
        public Model Output { get; private set; }
        public Uri RelatedApplication { get; private set; }
        public string RelatedApplicationUsage { get; private set; } = String.Empty;

        public ModelParser(object input)
        {
            Input = input;
        }

        public void Parse()
        {
            throw new NotImplementedException("Use the implemented parser for a specific input.");
        }
    }
}
