using ECSExperiment.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSExperiment.Interfaces
{
    public interface IComponentStore
    {
        bool Has(Entity e);
        void Remove(Entity e);
        IEnumerable<Entity> Entities();
        int Count { get; }
    }
}
