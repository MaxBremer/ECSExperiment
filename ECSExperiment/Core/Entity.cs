using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSExperiment.Core
{
    public readonly record struct Entity(int Id)
    {
        public static readonly Entity Null = new(-1);
        public bool IsNull => Id < 0;
    }
}
