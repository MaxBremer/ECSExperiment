using ECSExperiment.Core;
using ECSExperiment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSExperiment.Systems
{
    public sealed class MovementSystem : ISystem
    {
        public void Update(World world, float dt, GameContext ctx)
        {
            foreach (var e in world.Query(typeof(Position), typeof(Velocity)))
            {
                ref var pos = ref world.Get<Position>(e);
                var vel = world.Get<Velocity>(e); // copy
                world.Remove<Velocity>(e); // consume

                int nx = Math.Clamp(pos.X + vel.dX, 0, ctx.Width - 1);
                int ny = Math.Clamp(pos.Y + vel.dY, 0, ctx.Height - 1);

                // collision with solids -> no movement; but still allow damage-on-contact
                bool blocked = false;
                foreach (var other in world.Query(typeof(Position), typeof(Solid)))
                {
                    if (other.Id == e.Id) continue;
                    var op = world.Get<Position>(other);
                    if (op.X == nx && op.Y == ny) { blocked = true; break; }
                }

                // trigger contact damage
                foreach (var other in world.Query(typeof(Position), typeof(DamageOnContact)))
                {
                    if (other.Id == e.Id) continue;
                    var op = world.Get<Position>(other);
                    if (op.X == nx && op.Y == ny)
                    {
                        var dmg = world.Get<DamageOnContact>(other).Amount;
                        world.Emit(new DealDamage(e, dmg));
                    }
                }

                if (!blocked) { pos.X = nx; pos.Y = ny; }
            }
        }
    }
}
