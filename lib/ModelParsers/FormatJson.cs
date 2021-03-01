using System;
using System.IO;
using System.Text;
using System.Text.Json;
using TRoschinsky.Common;

namespace TRoschinsky.SPDataModel.Lib.ModelParsers
{
    public class FormatJson : ModelParser
    {
        private string fileExtension = ".json";

        public FormatJson(string jsonString) : base(jsonString)
        {
            Name = "System: Deserializes model from JSON string";
            InputType = typeof(string);
            CanReadFromFile = true;
            Parse();
        }
        public FormatJson(FileInfo jsonFile) : base(jsonFile)
        {
            Name = "System: Deserializes model from JSON file";
            InputType = typeof(string);
            Parse();
        }

        public override void Parse()
        {
            if(Input == null || String.IsNullOrWhiteSpace(Input as string))
            {
                log.Add(new JournalEntry("Input is empty - nothing to parse...", GetType().Name, true));
                return;
            }

            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions();
                options.WriteIndented = true;
                options.IgnoreReadOnlyFields = true;
                options.IgnoreReadOnlyProperties = true;
                options.NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals;
                options.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                Output = JsonSerializer.Deserialize(Input as string, typeof(Model)) as Model;
            }
            catch (Exception ex)
            {
                log.Add(new JournalEntry("Failed to parse input", GetType().Name, ex));
            }
        }

        public override bool Read(FileInfo fileToRead)
        {
            bool result = false;
            if (fileToRead != null && CanReadFromFile)
            {
                try
                {
                    if (!fileToRead.Exists)
                    {
                        log.Add(new JournalEntry(String.Format("File '{0}' is not existing", fileToRead.FullName), GetType().Name, true));
                        return result;
                    }

                    Input = File.ReadAllText(fileToRead.FullName);

                    result = true;
                    log.Add(new JournalEntry(String.Format("Read input from {0}", fileToRead.FullName), GetType().Name));
                }
                catch (Exception ex)
                {
                    log.Add(new JournalEntry("Failed to read input", GetType().Name, ex));
                }
            }
            else
            {
                log.Add(new JournalEntry(
                    String.Format("Unable to read input (Parser {0}able to save / input {1})", 
                        CanReadFromFile ? "is " : "un", 
                        Input != null ? "exists" : "is empty"), 
                    GetType().Name, true));
            }
            return result;
        }
    }
}
