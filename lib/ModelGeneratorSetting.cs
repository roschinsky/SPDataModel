using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TRoschinsky.SPDataModel.Lib
{
    public class ModelGeneratorSetting
    {
        public bool UseDisplayNames { get; set; }
        public bool UseInternalNames { get; set; }
        public bool ShowHiddenFields { get; set; }
        public bool ShowSystemFields { get; set; }
        public bool ShowHiddenLists { get; set; }
        public bool ShowSystemLists { get; set; }
        public bool ShowListsWhenLookupTarget { get; set; }
        public DirectoryInfo DefaultFilePath { get; set; }
        public string DefaultFolderName { get; set; } = "SPDataModel";

        public ModelGeneratorSetting()
        {
            SetDefaultOutputMode(2);
            SetDefaultDirectory();
        }

        public ModelGeneratorSetting(int displayOutputMode)
        {
            SetDefaultOutputMode(displayOutputMode);
            SetDefaultDirectory();
        }

        public void SetDefaultOutputMode(int displayOutputMode)
        {
            switch (displayOutputMode)
            {
                // nice looking for users
                case 1:
                    UseInternalNames = ShowSystemLists = ShowHiddenFields = ShowHiddenLists = false;
                    UseDisplayNames = ShowSystemFields = true;
                    break;

                // nice looking for developers
                case 2:
                    UseDisplayNames = ShowHiddenFields = ShowHiddenLists = false;
                    UseInternalNames = ShowSystemFields = ShowSystemLists = true;
                    break;

                // nice looking for DB guys
                case 3:
                    UseDisplayNames = false;
                    UseInternalNames = ShowSystemFields = ShowSystemLists = ShowHiddenFields = ShowHiddenLists = true;
                    break;

                default:
                    UseDisplayNames = false;
                    UseInternalNames = true;
                    ShowHiddenFields = false;
                    ShowSystemFields = true;
                    ShowHiddenLists = false;
                    ShowSystemLists = true;
                    break;

            }
        }

        private void SetDefaultDirectory()
        {
            DefaultFilePath = new DirectoryInfo(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), DefaultFolderName));
        }

        public static string GetSafeFileName(string name, string extension, char substitute = '_')
        {
            char[] invalids = Path.GetInvalidFileNameChars();
            string result = new string(name.Select(c => invalids.Contains(c) ? substitute : c).ToArray());
            result = result.Trim().Replace(" ", "");
            result += extension;
            return result;
        }
    }
}
