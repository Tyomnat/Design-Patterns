using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Enemies
{
    abstract class Prototype<T>
    {
        public T ShallowCopy()
        {
            return (T) this.MemberwiseClone();
        }

        abstract public T DeepCopy();
    }
}
