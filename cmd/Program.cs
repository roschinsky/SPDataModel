using System;
using System.Collections.Generic;
using TRoschinsky.SPDataModel.Lib;
using TRoschinsky.SPDataModel.Lib.FieldTypes;
using TRoschinsky.SPDataModel.Lib.ModelGenerators;

namespace cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            Model model = RunSomeEntityMagic();
            OutputModel(model, true);

            Console.WriteLine("{0}Press any key to continue...", Environment.NewLine);
            Console.Read();
        }

        private static Model RunSomeEntityMagic()
        {
            Model model = new Model("Sample");

            Entity entityToAdd = new Entity() { DisplayName = "Organisation", InternalName = "OrgUnit" };
            entityToAdd.AddField(new FieldText("OU", "title"));
            entityToAdd.AddField(new FieldUser("Gruppe", "orgGroup", true));
            entityToAdd.AddField(new FieldUser("FK", "orgManager", false) { IsMultiLookup = true });
            model.AddEntity(entityToAdd);

            entityToAdd = new Entity() { DisplayName = "Umfrage", InternalName = "Survey" };
            entityToAdd.AddField(new FieldDateTime("Durchgeführt am", "conductedOn"));
            entityToAdd.AddField(new FieldNumber("Jahr", "year"));
            entityToAdd.AddField(new FieldUser("Verantwortlich", "responsible", false));
            model.AddEntity(entityToAdd);

            entityToAdd = new Entity() { DisplayName = "Frage", InternalName = "Question" };
            entityToAdd.AddField(new FieldText("Frage", "title"));
            entityToAdd.AddField(new FieldMultiLineText("Fragentext", "question"));
            entityToAdd.AddField(new FieldLookup("Umfrage", new Relation(entityToAdd, model.GetEntityByName("Umfrage"))));
            model.AddEntity(entityToAdd);

            entityToAdd = new Entity() { DisplayName = "Ergebnis", InternalName = "Result" };
            entityToAdd.AddField(new FieldText("Ergebnis", "title"));
            entityToAdd.AddField(new FieldNumber("ErgebnisWert", "resultValue"));
            entityToAdd.AddField(new FieldLookup("Frage", new Relation(entityToAdd, model.GetEntityByName("Frage"))));
            entityToAdd.AddField(new FieldLookup("OU", new Relation(entityToAdd, model.GetEntityByName("Organisation"))));
            model.AddEntity(entityToAdd);

            entityToAdd = new Entity() { DisplayName = "Maßnahme", InternalName = "Measure" };
            entityToAdd.AddField(new FieldMultiChoice("Maßnahmenart", "measureType", new string[] { "Personal", "Führung", "Umgebung", "Sonstiges" }));
            entityToAdd.AddField(new FieldMultiChoice("Status", "statusCode", new string[] { "Geplant", "Verworfen", "In Bearbeitung", "Abgeschlossen" }));
            entityToAdd.AddField(new FieldDateTime("Geplante Umsetzung", "dueDateInitial"));
            entityToAdd.AddField(new FieldUser("Verantwortlich", "responsible", false));
            entityToAdd.AddField(new FieldLookup("Frage", new Relation(entityToAdd, model.GetEntityByName("Ergebnis"))));
            entityToAdd.AddField(new FieldLookup("OU", new Relation(entityToAdd, model.GetEntityByName("Organisation"))));
            model.AddEntity(entityToAdd);

            entityToAdd = new Entity() { DisplayName = "Maßnahmenstatus", InternalName = "MeasureState" };
            entityToAdd.AddField(new FieldMultiLineText("Fortschritt", "progressDesc"));
            entityToAdd.AddField(new FieldDateTime("Geplante Umsetzung", "dueDateCurrent"));
            entityToAdd.AddField(new FieldDateTime("Status berichtet am", "reportDate"));
            entityToAdd.AddField(new FieldMultiChoice("Fortschritt", "stateCode", new string[] { "GRAU", "GRÜN", "GELB", "ROT" }));
            entityToAdd.AddField(new FieldLookup("Maßnahme", new Relation(entityToAdd, model.GetEntityByName("Maßnahme"))));
            model.AddEntity(entityToAdd);


            return model;
        }

        private static void OutputModel(Model model, bool verbose)
        {
            try
            {
                
                Console.WriteLine("*** Summary for data model '{0}' (created {1:d}) ***", model.Name, model.CreatedOn);
                Console.WriteLine("--------------------------------------------------------------------");
                foreach (Entity entity in model.Entities)
                {
                    if (!verbose)
                    {
                        Console.WriteLine("- " + entity);
                    }
                    else
                    {
                        Console.WriteLine("--| {0} ({1}) |--", entity.DisplayName, entity.InternalName);
                        foreach (Field field in entity.Fields)
                        {
                            Console.WriteLine("\t- {0}", field);
                        }
                        Console.WriteLine();
                    }
                }

                Console.WriteLine(Environment.NewLine);
                

                ModelGenerator generator1 = new DrawioTextList(model, String.Format("Convert {0}", model.Name));
                Console.WriteLine("----------| {0}", generator1);
                Console.WriteLine(generator1.Output);
                Console.WriteLine("-----------------------------------------");

                ModelGenerator generator2 = new DrawioTextDiagram(model, String.Format("Convert {0} ", model.Name));
                Console.WriteLine("----------| {0}", generator2);
                Console.WriteLine(generator2.Output);
                Console.WriteLine("-----------------------------------------");

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Output failed due to: {0}", ex.Message);
            }
        }
    }
}
