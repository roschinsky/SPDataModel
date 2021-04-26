using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TRoschinsky.Common;

namespace TRoschinsky.SPDataModel.Lib.ModelGenerators
{
    public class FormatJson : ModelGenerator
    {
        private string fileExtension = ".json";

        public FormatJson(Model inputModel) : base(inputModel)
        {
            Name = "System: Serializes model to JSON";
            OutputType = typeof(string);
            RelatedModelParser = typeof(ModelParsers.FormatJson);
            CanSaveToFile = true;
            Generate();
        }

        public override void Generate()
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                // TODO: https://docs.microsoft.com/de-de/dotnet/standard/serialization/system-text-json-converters-how-to?pivots=dotnet-5-0#support-polymorphic-deserialization
                options.Converters.Add(new FieldSerializationConverter());
                options.WriteIndented = true;
                options.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
                options.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                Output = JsonSerializer.Serialize(new ModelExport(Input), typeof(ModelExport), options);
            }
            catch (Exception ex)
            {
                log.Add(new JournalEntry("Failed to generate output", GetType().Name, ex));
            }
        }

        public override bool Save(DirectoryInfo pathToSave)
        {
            bool result = false;
            if (!String.IsNullOrEmpty(Output as string) && CanSaveToFile)
            {
                try
                {
                    if (pathToSave == null)
                    {
                        pathToSave = Settings.DefaultFilePath;
                        log.Add(new JournalEntry("No path given - will use default...", GetType().Name));
                    }

                    if (!pathToSave.Exists)
                    {
                        log.Add(new JournalEntry(String.Format("Path not existing. Try to create '{0}'", pathToSave.FullName), GetType().Name));
                        Directory.CreateDirectory(pathToSave.FullName);
                    }

                    string fileName = String.IsNullOrWhiteSpace(Input.Name) ? String.Format("Model_{0:yyMMdd-HHmmss}", Input.CreatedOn) : Input.Name;
                    fileName = fileName.Trim();
                    FileInfo fileInfo = new FileInfo(Path.Combine(
                        pathToSave.FullName,
                        ModelGeneratorSetting.GetSafeFileName(fileName, fileExtension)));
                    File.WriteAllText(fileInfo.FullName, Output as string);

                    result = true;
                    log.Add(new JournalEntry(String.Format("Saved output to {0}", fileInfo.FullName), GetType().Name));
                }
                catch (Exception ex)
                {
                    log.Add(new JournalEntry("Failed to save output", GetType().Name, ex));
                }
            }
            else
            {
                log.Add(new JournalEntry(
                    String.Format("Unable to save output (Generator {0}able to save / output {1})", 
                        CanSaveToFile ? "is " : "un", 
                        Output != null ? "exists" : "is empty"), 
                    GetType().Name, true));
            }
            return result;
        }
    }
}
