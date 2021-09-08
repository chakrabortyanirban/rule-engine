using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngine.Domain
{
    public abstract class RuleContext<T>
    {
        public abstract Task<T> ExecuteAction();
    }
}
