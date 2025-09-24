using ECSExperiment.Core;
using ECSExperiment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSExperiment.Systems
{
    public sealed class CombatSystem : ISystem
    {
        public void Update(World world, float dt, GameContext ctx)
        {
            foreach (var ev in world.Drain<DealDamage>())
            {
                if (!world.Alive(ev.Target)) continue;
                if (!world.TryGet(ev.Target, out Health hp)) continue;
                hp.Current -= ev.Amount;
                if (hp.Current <= 0)
                {
                    hp.Current = 0;
                    // mark dead
                    world.Add(ev.Target, new DeadTag());
                    world.Emit(new Killed(ev.Target));
                }
                world.Add(ev.Target, hp); // write back
            }
        }
    }
}
