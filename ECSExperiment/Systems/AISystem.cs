using ECSExperiment.Core;
using ECSExperiment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSExperiment.Systems
{
    public sealed class AISystem : ISystem
    {
        public void Update(World world, float dt, GameContext ctx)
        {
            foreach (var e in world.Query(typeof(AIWalker), typeof(Position)))
            {
                ref var ai = ref world.Get<AIWalker>(e);
                ai.Timer++;
                if (ai.Timer < ai.StepEvery) continue;
                ai.Timer = 0;

                // Simple pseudo-random walk (deterministic per-entity)
                unchecked { ai.Seed = (ai.Seed * 1103515245 + 12345); }
                int r = (ai.Seed >> 16) & 3;
                var (dx, dy) = r switch { 0 => (1, 0), 1 => (-1, 0), 2 => (0, 1), _ => (0, -1) };
                world.Add(e, new Velocity { dX = dx, dY = dy });
            }
        }
    }
}
