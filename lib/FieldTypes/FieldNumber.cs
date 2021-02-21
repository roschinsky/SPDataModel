using System;
using System.Collections.Generic;

namespace TRoschinsky.SPDataModel.Lib.FieldTypes
{
    public class FieldNumber : Field
    {
        public int Decimals { get; set; } = 0;
        public bool AsPercentage { get; set; } = false;
        public int ValueMinimum { get; set; } = int.MinValue;
        public int ValueMaximum { get; set; } = int.MaxValue;

        public FieldNumber(string displayName, string internalName) : base(displayName, internalName)
        {
            Init();
        }

        public FieldNumber(string displayName, string internalName, int decimals) : base(displayName, internalName)
        {
            Init();
            Decimals = decimals;
        }

        public FieldNumber(string displayName, string internalName, int decimals, int min, int max) : base(displayName, internalName)
        {
            Init();
            Decimals = decimals;
            ValueMinimum = min;
            ValueMaximum = max;
        }

        public FieldNumber(string displayName, string internalName, int decimals, int min, int max, bool asPercentage) : base(displayName, internalName)
        {
            Init();
            Decimals = decimals;
            ValueMinimum = min;
            ValueMaximum = max;
            AsPercentage = asPercentage;
        }

        private void Init()
        {
            FieldTypeName = "Number";
            FieldType = TypeOfField.Number;
        }

    }
}