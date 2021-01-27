using System;
using System.Collections.Generic;
using System.Text;

namespace TRoschinsky.SPDataModel.Lib.ModelGenerators
{
    public class DrawioTextDiagram : ModelGenerator
    {

        public DrawioTextDiagram(Model inputModel, string generatorName) : base(inputModel)
        {
            Name = generatorName;
            RelatedApplication = new Uri("https://app.diagrams.net/");
            RelatedApplicationUsage = "Click the plus button in symbol bar and choose 'Advanced'>>'From Text...' and choose 'Diagram' in dropdown at the bottom of the dialog. Then insert the generated output in the textbox.";
            OutputType = typeof(string);
            Generate();
        }

        public override void Generate()
        {
            string lb = Environment.NewLine;
            StringBuilder outputSb = new StringBuilder();
            outputSb.Append(String.Format(";Data model '{0}', created on {1:d}:{2}", Input.Name, Input.CreatedOn, lb));

            foreach (Entity entity in Input.Entities)
            {
                try
                {
                    if ((entity.IsHidden && Settings.ShowHiddenLists) || (entity.IsSystem && Settings.ShowSystemLists) || (!entity.IsHidden && !entity.IsSystem))
                    {
                        foreach(Relation relation in entity.Relations)
                        {
                        outputSb.Append(String.Format("{0}->{1}{2}",
                            Settings.UseDisplayNames ? entity.DisplayName : entity.InternalName,
                            Settings.UseDisplayNames ? relation.LookupToEntity.DisplayName : relation.LookupToEntity.InternalName,
                            lb));
                        }
                    }
                }
                catch (Exception ex)
                {
                    outputSb.Append(String.Format(";Error rendering entity '{0}': {1}{2}", entity.InternalName, ex.Message, lb));
                }
            }
            Output = outputSb.ToString();
        }
    }
}
