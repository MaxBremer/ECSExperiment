using ECSExperiment.Core;
using ECSExperiment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSExperiment.Systems
{

    public sealed class CleanupSystem : ISystem
    {
        public void Update(World world, float dt, GameContext ctx)
        {
            foreach (var e in world.Query(typeof(DeadTag)))
            {
                // You could spawn a corpse, particles, etc. Here we just destroy.
                world.Destroy(e);
            }
        }
    }
}
