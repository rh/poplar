using System;

namespace Poplar
{
    public class Parameter : IHasDescription
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Func<string> GetValue { get; set; }

        private string _value;

        public string Value
        {
            get
            {
                if (_value == null)
                {
                    _value = GetValue();
                }
                return _value;
            }
            set { _value = value; }
        }

        public Parameter()
        {
            GetValue = () => { return string.Empty; };
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Name, Description);
        }
    }
}