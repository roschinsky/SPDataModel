using System;
using System.Collections.Generic;
using System.IO;
using TRoschinsky.Common;

namespace TRoschinsky.SPDataModel.Lib
{
    public abstract class ModelGenerator
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public Type RelatedModelParser { get; protected set; }
        public Model Input { get; private set; }
        public object Output { get; protected set; }
        public Type OutputType { get; protected set; }
        public Uri RelatedApplication { get; protected set; }
        public string RelatedApplicationUsage { get; protected set; } = String.Empty;
        public ModelGeneratorSetting Settings { get; protected set; }
        public bool CanSaveToFile { get; protected set; } = false;
        protected List<JournalEntry> log = new List<JournalEntry>();
        public JournalEntry[] Log { get => log.ToArray(); }

        public ModelGenerator(Model input)
        {
            if(input == null)
            {
                throw new ArgumentNullException("input", "A ModelGenerator must have been given a model as input when being created!");
            }

            Input = input;
            Settings = new ModelGeneratorSetting();
        }

        public virtual void Generate()
        {
            throw new NotImplementedException("Use the implemented generator for a specific processing.");
        }

        public virtual bool Save()
        {
            return Save(null);
        }

        public virtual bool Save(DirectoryInfo pathToSave)
        {
            if(CanSaveToFile && Output != null) 
            {
                log.Add(new JournalEntry("Unable to save output", GetType().Name, new NotImplementedException("The generator didn't implement a save method")));
            }
            else
            {
                log.Add(new JournalEntry(
                    String.Format("Unable to save output (Generator {0}able to save / output {1})", 
                        CanSaveToFile ? "is " : "un", 
                        Output != null ? "exists" : "is empty"), 
                    GetType().Name, true));
            }
            return false;
        }

        public override string ToString()
        {
            Type thisIsMadeOf = this.GetType();
            if(thisIsMadeOf.IsAbstract)
            {
                return "Nuffn here - its just the generator blueprints...";
            }
            else
            {
                var outputString = Output != null ? String.Concat("'", String.Concat(Output).Substring(0, 12),"'") : "nothing";
                return String.Format("{0} generated a {1} '{2}'.", thisIsMadeOf.Name, OutputType.Name, outputString);
            }
        }
    }
}
