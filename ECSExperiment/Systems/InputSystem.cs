using ECSExperiment.Core;
using ECSExperiment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSExperiment.Systems
{
    public sealed class InputSystem : ISystem
    {
        public void Update(World world, float dt, GameContext ctx)
        {
            if (!Console.KeyAvailable) return;
            var key = Console.ReadKey(intercept: true).Key;

            int dx = 0, dy = 0;
            if (key == ConsoleKey.W || key == ConsoleKey.UpArrow) dy = -1;
            else if (key == ConsoleKey.S || key == ConsoleKey.DownArrow) dy = 1;
            else if (key == ConsoleKey.A || key == ConsoleKey.LeftArrow) dx = -1;
            else if (key == ConsoleKey.D || key == ConsoleKey.RightArrow) dx = 1;
            else if (key == ConsoleKey.Escape) ctx.Running = false;

            if (dx == 0 && dy == 0) return;

            foreach (var e in world.Query(typeof(PlayerTag), typeof(Position)))
            {
                world.Add(e, new Velocity { dX = dx, dY = dy });
            }
        }
    }
}
