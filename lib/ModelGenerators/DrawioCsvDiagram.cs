using System;
using System.Collections.Generic;
using System.Text;

namespace TRoschinsky.SPDataModel.Lib.ModelGenerators
{
    public class DrawioCsvDiagram : ModelGenerator
    {
        private const string baseTemplate = ""; 

        /*
            ## SPDataModel ...styles and layout...
            # label: <b>%name%</b><br><i style="color:dimgray;">%desc%</i>
            # style: rounded=1;whiteSpace=wrap;html=1;strokeColor=#BABABA;gradientColor=%fill%;fontStyle=1;
            # namespace: csvimport-
            # connect: {"from":"lkup1", "to":"eid", "invert":true, "label":"lkup", "style":"endArrow=block;endFill=1;edgeStyle=entityRelationEdgeStyle;"}
            # connect: {"from":"lkupN", "to":"eid", "invert":true, "label":"lkup", "style":"endArrow=block;endFill=1;edgeStyle=entityRelationEdgeStyle;startArrow=block;startFill=0;"}
            # width: auto
            # height: auto
            # padding: 15
            # ignore: eid,lkup1,lkupN
            # nodespacing: 60
            # levelspacing: 80
            # edgespacing: 60
            # layout: auto
            ## SPDataModel ...CSV based model...
            eid,name,desc,lkup,fill,lkup1,lkupN
            1,Test 1,I like it!,lkupToNext,#FEFEFE,,
            2,Am I alive?,lkupToNext,#fff2cc,#FEFEFE,1,
            3,Yes,#d5e8d4,lkupToNext,#FEFEFE,,2
            4,No,#f8cecc,lkupToNext,#b85450,,2 
            5,,#fff2cc,lkupToNext,#FEFEFE,3,
        */

        public DrawioCsvDiagram(Model inputModel, string generatorName) : base(inputModel)
        {
            Name = generatorName;
            RelatedApplication = new Uri("https://app.diagrams.net/");
            RelatedApplicationUsage = "Click the plus button in symbol bar and choose 'Advanced'>>'CSV...' and insert the generated output in the textbox.";
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
