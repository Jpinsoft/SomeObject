using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpinsoft.SomeObject.Types
{
    public class GeneratorErrorInfo
    {
        public GeneratorErrorInfo(string generatorMessage, int recursionLevel, Exception ex)
        {
            this.GeneratorMessage = generatorMessage;
            this.RecursionLevel = recursionLevel;
            this.InnerException = ex;
        }

        public string GeneratorMessage { get; private set; }

        public int RecursionLevel { get; private set; }

        public Exception InnerException { get; private set; }
    }
}
