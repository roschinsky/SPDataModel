using System;
using System.Collections.Generic;
using System.IO;
using TRoschinsky.Common;

namespace TRoschinsky.SPDataModel.Lib
{
    public abstract class ModelParser
    {
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public Type RelatedModelGenerator { get; protected set; }
        public object Input { get; protected set; }
        public Type InputType { get; protected set; }
        public Model Output { get; protected set; }
        public Uri RelatedApplication { get; protected set; }
        public string RelatedApplicationUsage { get; protected set; } = String.Empty;
        public bool CanReadFromFile { get; protected set; } = false;
        protected List<JournalEntry> log = new List<JournalEntry>();
        public JournalEntry[] Log { get => log.ToArray(); }

        public ModelParser(object input)
        {
            Input = input;
        }
        public ModelParser(FileInfo file)
        {
            CanReadFromFile = true;
            Read(file);
        }

        public virtual void Parse()
        {
            throw new NotImplementedException("Use the implemented parser for a specific input.");
        }

        public virtual bool Read(FileInfo fileToRead)
        {
            if(CanReadFromFile && fileToRead != null) 
            {
                log.Add(new JournalEntry("Unable to read input", GetType().Name, new NotImplementedException("The generator didn't implement a read method")));
            }
            else
            {
                log.Add(new JournalEntry(
                    String.Format("Unable to read input (Parser {0}able to save / input {1})", 
                        CanReadFromFile ? "is " : "un", 
                        Input != null ? "exists" : "is empty"), 
                    GetType().Name, true));
            }
            return false;
        }
    }
}
