using System;
using TRoschinsky.SPDataModel.Lib;
using TRoschinsky.SPDataModel.Lib.FieldTypes;

namespace TRoschinsky.SPDataModel.Lib.Data
{
    public static class SampleModels
    {
        public enum SampleModelType
        {
            MeasureTracking,
            OrderProcess

        }

        public static Model GetSampleModel(SampleModelType typeOfModel)
        {
            switch (typeOfModel)
            {
                case SampleModelType.MeasureTracking:
                    return GenerateCodeBasedModel1();

                default:
                    return new Model("Empty Model");
            }
        }

        private static Model GenerateCodeBasedModel1()
        {
            Model model = new Model("Sample: Measure Tracking");
            model.Description = "A data model that holds results from surveys and allows you to track actions on those results.";

            Entity entityToAdd = new Entity() { DisplayName = "Maßnahme", InternalName = "Measure" };
            entityToAdd.AddField(new FieldMultiChoice("Maßnahmenart", "measureType", new string[] { "Personal", "Führung", "Umgebung", "Sonstiges" }));
            entityToAdd.AddField(new FieldMultiChoice("Status", "statusCode", new string[] { "Geplant", "Verworfen", "In Bearbeitung", "Abgeschlossen" }));
            entityToAdd.AddField(new FieldDateTime("Geplante Umsetzung", "dueDateInitial"));
            entityToAdd.AddField(new FieldUser("Verantwortlich", "responsible", false, model.DefaultUilInternalName));
            entityToAdd.AddField(new FieldLookup("Ergebnis", new Relation(entityToAdd.InternalName, "Result")));
            entityToAdd.AddField(new FieldLookup("OU", new Relation(entityToAdd.InternalName, "OrgUnit")));
            model.AddEntity(entityToAdd);

            entityToAdd = new Entity() { DisplayName = "Maßnahmenstatus", InternalName = "MeasureState" };
            entityToAdd.AddField(new FieldMultiLineText("Fortschritt", "progressDesc"));
            entityToAdd.AddField(new FieldDateTime("Geplante Umsetzung", "dueDateCurrent"));
            entityToAdd.AddField(new FieldDateTime("Status berichtet am", "reportDate"));
            entityToAdd.AddField(new FieldMultiChoice("Fortschritt", "stateCode", new string[] { "GRAU", "GRÜN", "GELB", "ROT" }));
            entityToAdd.AddField(new FieldLookup("Maßnahme", new Relation(entityToAdd.InternalName, "Measure")));
            model.AddEntity(entityToAdd);

            entityToAdd = new Entity() { DisplayName = "Organisation", InternalName = "OrgUnit" };
            entityToAdd.AddField(new FieldText("OU", "title"));
            entityToAdd.AddField(new FieldUser("Gruppe", "orgGroup", true, model.DefaultUilInternalName));
            entityToAdd.AddField(new FieldUser("FK", "orgManager", false, model.DefaultUilInternalName) { IsMultiLookup = true });
            model.AddEntity(entityToAdd);

            entityToAdd = new Entity() { DisplayName = "Umfrage", InternalName = "Survey" };
            entityToAdd.AddField(new FieldDateTime("Durchgeführt am", "conductedOn"));
            entityToAdd.AddField(new FieldNumber("Jahr", "year"));
            entityToAdd.AddField(new FieldUser("Verantwortlich", "responsible", false, model.DefaultUilInternalName));
            model.AddEntity(entityToAdd);

            entityToAdd = new Entity() { DisplayName = "Frage", InternalName = "Question" };
            entityToAdd.AddField(new FieldText("Frage", "title"));
            entityToAdd.AddField(new FieldMultiLineText("Fragentext", "question"));
            entityToAdd.AddField(new FieldLookup("Umfrage", new Relation(entityToAdd.InternalName, "Survey")));
            model.AddEntity(entityToAdd);

            entityToAdd = new Entity() { DisplayName = "Ergebnis", InternalName = "Result" };
            entityToAdd.AddField(new FieldText("Ergebnis", "title"));
            entityToAdd.AddField(new FieldNumber("ErgebnisWert", "resultValue"));
            entityToAdd.AddField(new FieldLookup("Frage", new Relation(entityToAdd.InternalName, "Question")));
            entityToAdd.AddField(new FieldLookup("OU", new Relation(entityToAdd.InternalName, "OrgUnit")));
            model.AddEntity(entityToAdd);

            return model;
        }

    }
}