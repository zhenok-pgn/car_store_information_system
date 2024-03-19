using System;

namespace CarStore.Domain
{
    class LabelAttribute : Attribute
    {
        public string LabelText { get; set; }
        public LabelAttribute(string labelText)
        {
            LabelText = labelText;
        }
    }
}
