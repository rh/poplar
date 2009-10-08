using System.Collections.ObjectModel;

namespace Poplar.Parameters
{
    public class ParameterCollection : Collection<Parameter>
    {
        public Parameter this[string name]
        {
            get
            {
                foreach (var parameter in Items)
                {
                    if (parameter.Name.Equals(name))
                    {
                        return parameter;
                    }
                }
                return null;
            }
        }
    }
}