using System;

namespace UniTrait
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UniTraitDebugActionAttribute : Attribute
    {
        public UniTraitDebugActionAttribute(string label)
        {
            Label = label;
        }

        public string Label { get; }
    }
}
