using ECSExperiment.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSExperiment.Interfaces
{
    public interface ISystem
    {
        void Update(World world, float dt, GameContext ctx);
    }
}
