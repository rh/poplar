using System.Collections.Generic;

namespace Poplar
{
    public class Template : IHasDescription
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public Parameter[] Parameters
        {
            get { return parameters.ToArray(); }
        }

        private readonly List<Parameter> parameters = new List<Parameter>();

        public void Add(Parameter parameter)
        {
            parameters.Add(parameter);
        }
    }
}