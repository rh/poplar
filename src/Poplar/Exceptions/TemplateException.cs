using System;
using System.Collections.Generic;

namespace Poplar.Exceptions
{
    public class TemplateException : Exception
    {
        private readonly string template;
        private readonly List<string> errors = new List<string>();

        public TemplateException(string template)
        {
            this.template = template;
        }

        public string Template
        {
            get { return template; }
        }

        public void AddError(string error)
        {
            errors.Add(error);
        }

        public override string ToString()
        {
            if (errors.Count == 0)
            {
                return string.Format("Failed to process template '{0}'.", Template);
            }

            if (errors.Count == 1)
            {
                return string.Format("Failed to process template '{0}':{1}{2}", Template, Environment.NewLine, errors[0]);
            }

            var s = string.Format("Failed to process template '{0}'.", Template);
            foreach (var error in errors)
            {
                s += Environment.NewLine + "* " + error;
            }
            return s;
        }
    }
}