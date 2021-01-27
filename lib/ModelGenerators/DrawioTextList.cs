using System;
using System.Collections.Generic;
using System.Text;

namespace TRoschinsky.SPDataModel.Lib.ModelGenerators
{
    public class DrawioTextList : ModelGenerator
    {

        public DrawioTextList(Model inputModel, string generatorName) : base(inputModel)
        {
            Name = generatorName;
            RelatedApplication = new Uri("https://app.diagrams.net/");
            RelatedApplicationUsage = "Click the plus button in symbol bar and choose 'Advanced'>>'From Text...' and choose 'List' in dropdown at the bottom of the dialog. Then insert the generated output in the textbox.";
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
                        outputSb.Append(String.Format("{0}{1}",
                            Settings.UseDisplayNames && Settings.UseInternalNames ?
                                (entity.DisplayName + " [" + entity.InternalName + "]") :
                                (Settings.UseDisplayNames ? entity.DisplayName : entity.InternalName),
                            lb));

                        bool addedPrivateFields = false;

                        List<Field> fieldList = new List<Field>();
                        fieldList.AddRange(entity.Fields);

                        foreach (Field field in fieldList.FindAll(f => f.IsHidden || f.IsSystem))
                        {
                            if ((field.IsHidden && Settings.ShowHiddenFields) || (field.IsSystem && Settings.ShowSystemFields))
                            {
                                outputSb.Append(String.Format("-{0}: {1}{2}",
                                    Settings.UseDisplayNames && Settings.UseInternalNames ?
                                        (field.DisplayName + " " + field.InternalName) :
                                        (Settings.UseDisplayNames ? field.DisplayName : field.InternalName),
                                    Settings.UseDisplayNames ? field.FieldTypeName : field.FieldType,
                                    lb));
                                addedPrivateFields = true;
                            }
                        }

                        if (addedPrivateFields && (fieldList.FindAll(f => !(f.IsHidden || f.IsSystem))).Count > 0)
                        {
                            outputSb.Append(String.Format("---{0}", lb));
                        }

                        foreach (Field field in fieldList.FindAll(f => !(f.IsHidden || f.IsSystem)))
                        {
                            outputSb.Append(String.Format("+{0}: {1}{2}",
                                Settings.UseDisplayNames && Settings.UseInternalNames ?
                                    (field.DisplayName + " " + field.InternalName) :
                                    (Settings.UseDisplayNames ? field.DisplayName : field.InternalName),
                                Settings.UseDisplayNames ? field.FieldTypeName : field.FieldType,
                                lb));
                        }
                    }
                }
                catch (Exception ex)
                {
                    outputSb.Append(String.Format("{0}{1}", ex.Message, lb));
                }
                outputSb.Append(lb);
            }
            Output = outputSb.ToString();
        }
    }
}
