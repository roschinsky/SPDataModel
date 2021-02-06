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
            string lb = Environment.NewLine;
            StringBuilder outputSb = new StringBuilder();
            foreach (Entity entity in Input.Entities)
            {
                try
                {
                    if ((entity.IsHidden && Settings.ShowHiddenLists) || (entity.IsSystem && Settings.ShowSystemLists) || (!entity.IsHidden && !entity.IsSystem))
                    {
                        outputSb.Append(String.Format("- {0}{1}", entity, lb));
                        outputSb.Append(String.Format("--| {0} ({1}) |--{2}", entity.DisplayName, entity.InternalName, lb));
                        foreach (Field field in entity.Fields)
                        {
                            outputSb.Append(String.Format("\t- {0}", field));
                        }
                        outputSb.Append(lb);
                    }
                }
                catch (Exception ex)
                {
                    outputSb.Append(String.Format("\t..error: {0}{1}", ex.Message, lb));
                }
                outputSb.Append(lb);
            }
            Output = outputSb.ToString();
        }
    }
}
