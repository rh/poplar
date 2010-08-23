using System;

// ReSharper disable InconsistentNaming

namespace Poplar.Dsl
{
    public abstract class PoplarTemplateFactory
    {
        public abstract void Initialize();

        public Template Template { get; protected set; }

        private IHasDescription current;

        public void template(string name, Action action)
        {
            current = Template = new Template {Name = name};
            action();
        }

        public void description(string value)
        {
            Console.WriteLine("-- description: {0}", value);
            if (current != null)
            {
                current.Description = value;
            }
        }

        public void parameter(string name, Action action)
        {
            Console.WriteLine("-- parameter: {0}", name);
            var parameter = new Parameter {Name = name};
            Template.Add(parameter);
            current = parameter;
            action();
        }

        public void value(string value)
        {
            if (current != null && current is Parameter)
            {
                (current as Parameter).Value = value;
            }
        }

        public void value(Func<string> func)
        {
            if (current != null && current is Parameter)
            {
                (current as Parameter).GetValue = func;
            }
        }

        public void value(Func<Guid> func)
        {
            if (current != null && current is Parameter)
            {
                (current as Parameter).GetValue = () => { return func().ToString(); };
            }
        }

        public Func<string> guid = () => Guid.NewGuid().ToString();
    }
}

// ReSharper restore InconsistentNaming