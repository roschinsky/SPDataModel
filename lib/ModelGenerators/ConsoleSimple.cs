using System;
using System.Text;

namespace TRoschinsky.SPDataModel.Lib.ModelGenerators
{
    public class ConsoleSimple : ModelGenerator
    {

        public ConsoleSimple(Model inputModel) : base(inputModel)
        {
            Name = "System: Simple Console Output";
            OutputType = typeof(string);
            Generate();
        }

        public override void Generate()
        {
            StringBuilder outputSb = new StringBuilder();
            foreach (Entity entity in Input.Entities)
            {
                try
                {
                    if ((entity.IsHidden && Settings.ShowHiddenLists) || (entity.IsSystem && Settings.ShowSystemLists) || (!entity.IsHidden && !entity.IsSystem))
                    {
                        outputSb.AppendLine(String.Format("--| {0} ({1}) |--", entity.DisplayName, entity.InternalName));
                        foreach (Field field in entity.Fields)
                        {
                            outputSb.AppendLine(String.Format("\t- {0}", field));
                        }
                    }
                }
                catch (Exception ex)
                {
                    outputSb.Append(String.Format("\t..error: {0}", ex.Message));
                }
                outputSb.AppendLine();
            }
            Output = outputSb.ToString();
        }
    }
}
