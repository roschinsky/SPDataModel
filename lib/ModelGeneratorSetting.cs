using System;
using System.Collections.Generic;

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

        public ModelGeneratorSetting()
        {
            SetDefaultOutputMode(2);
        }

        public ModelGeneratorSetting(int displayOutputMode)
        {
            SetDefaultOutputMode(displayOutputMode);
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
    }
}
