using System;
using System.Diagnostics;
using System.Xml;

namespace Poplar.Parameters
{
    public class Parameter
    {
        public Parameter(XmlNode node)
        {
            Name = ReadXmlNode(node, "name");
            Value = ReadXmlNode(node, "value");
            // Type should be read AFTER value because it may override it
            Type = ReadXmlNode(node, "type");
            Description = ReadXmlNode(node, "description");
        }

        public string Name { get; private set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Description { get; private set; }

        public virtual bool HasValue
        {
            [DebuggerStepThrough]
            get { return !String.IsNullOrEmpty(Value); }
        }

        public virtual string Stub
        {
            [DebuggerStepThrough]
            get { return "{" + Name + "}"; }
        }

        private string ReadXmlNode(XmlNode node, string xpath)
        {
            var child = node.SelectSingleNode(xpath);
            if (child == null)
            {
                return String.Empty;
            }
            if (xpath.Equals("type") && child.InnerText.Equals("guid"))
            {
                Value = Guid.NewGuid().ToString();
            }
            return child.InnerText;
        }

        [DebuggerStepThrough]
        public override string ToString()
        {
            if (HasValue)
            {
                return String.Format("{0} ({1})", Name, Value);
            }
            return Name;
        }
    }
}