using System;
using System.Collections.Generic;
using System.Text;

namespace TRoschinsky.SPDataModel.Lib.ModelGenerators
{
    public class DrawioCsvDiagram : ModelGenerator
    {
        private const string baseTemplate =
            "# label: <a href=\"%url%\"<b>%name%</b><br><i style=\"color:dimgray;font-size:smaller;\">%desc%</i></a>\n" +
            "# style: rounded=1;whiteSpace=wrap;html=1;strokeColor=#BABABA;gradientColor=%fill%;fontStyle=1;\n" +
            "# namespace: csvimport-\n" +
            "# connect: {\"from\":\"lkup1\", \"to\":\"eid\", \"invert\":false, \"label\":\"1:n\", \"style\":\"endArrow=block;endFill=1;edgeStyle=orthogonalEdgeStyle;\"}\n" +
            "# connect: {\"from\":\"lkupN\", \"to\":\"eid\", \"invert\":false, \"label\":\"n:m\", \"style\":\"endArrow=block;endFill=1;edgeStyle=orthogonalEdgeStyle;startArrow=block;startFill=0;\"}\n" +
            "# width: auto\n" +
            "# height: auto\n" +
            "# padding: 10\n" +
            "# ignore: eid,lkup1,lkupN\n" +
            "# nodespacing: 30\n" +
            "# levelspacing: 20\n" +
            "# edgespacing: 40\n" +
            "# layout: auto\n" +
            "## SPDataModel ...CSV based model...\n" +
            "eid,name,desc,url,fill,lkup1,lkupN";

        public DrawioCsvDiagram(Model inputModel) : base(inputModel)
        {
            RelatedApplication = new Uri("https://app.diagrams.net/");
            RelatedApplicationUsage = "Click the plus button in symbol bar and choose 'Advanced'>>'CSV...' and insert the generated output in the textbox.";
            OutputType = typeof(string);
            Generate();
        }

        public override void Generate()
        {
            string lb = Environment.NewLine;
            StringBuilder outputSb = new StringBuilder();
            outputSb.Append(String.Format("## SPDataModel '{0}', created on {1:d} ...styles and layout...{2}", Input.Name, Input.CreatedOn, lb));
            outputSb.Append(baseTemplate);
            outputSb.Append(lb);

            Dictionary<string, int> indexedEntities = new Dictionary<string, int>();
            for (int i = 0; i < Input.Entities.Count; i++)
            {
                indexedEntities.Add(Input.Entities[i].InternalName, i + 1);
            }

            foreach (Entity entity in Input.Entities)
            {
                try
                {
                    if ((entity.IsHidden && Settings.ShowHiddenLists) || (entity.IsSystem && Settings.ShowSystemLists) || (!entity.IsHidden && !entity.IsSystem))
                    {
                        string lkup1s = String.Empty;
                        string lkupNs = String.Empty;

                        foreach (Relation relation in entity.Relations)
                        {
                            if (relation.IsMultiLookup)
                            {
                                lkupNs += indexedEntities[relation.LookupToEntityName] + ",";
                            }
                            else
                            {
                                lkup1s += indexedEntities[relation.LookupToEntityName] + ",";
                            }
                        }

                        lkup1s = lkup1s.TrimEnd(',');
                        lkupNs = lkupNs.TrimEnd(',');

                        // eid,name,desc,url,fill,lkup1,lkupN
                        outputSb.Append(String.Format("{0},{1},{2},{3},{4},{5},{6}{7}",
                            indexedEntities[entity.InternalName],
                            Settings.UseDisplayNames ? entity.DisplayName : entity.InternalName,
                            entity.Description,
                            entity.Url != null ? entity.Url : "#",
                            entity.IsUil ? "#FFE599" : (entity.IsSystem || entity.IsHidden ? "#FFCCE6" : "#CCCCCC" ),
                            lkup1s.Contains(',') ? String.Concat("\"", lkup1s, "\"") : lkup1s,
                            lkupNs.Contains(',') ? String.Concat("\"", lkupNs, "\"") : lkupNs,
                            lb));
                    }
                }
                catch (Exception ex)
                {
                    outputSb.Append(String.Format("##Error rendering entity '{0}': {1}{2}", entity.InternalName, ex.Message, lb));
                }
            }
            Output = outputSb.ToString();
        }
    }
}
