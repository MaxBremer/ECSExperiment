using ECSExperiment.Core;
using ECSExperiment.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSExperiment.Systems
{
    public sealed class RenderSystem : ISystem
    {
        private readonly char[,] _buffer;
        public RenderSystem(int w, int h) { _buffer = new char[h, w]; }

        public void Update(World world, float dt, GameContext ctx)
        {
            // Clear
            for (int y = 0; y < ctx.Height; y++)
                for (int x = 0; x < ctx.Width; x++)
                    _buffer[y, x] = '.';

            // Draw
            foreach (var e in world.Query(typeof(Position), typeof(Renderable)))
            {
                var p = world.Get<Position>(e);
                var r = world.Get<Renderable>(e);
                if (p.X >= 0 && p.X < ctx.Width && p.Y >= 0 && p.Y < ctx.Height)
                    _buffer[p.Y, p.X] = r.Glyph;
            }

            // HUD: draw health of player at top line
            string hud = "";
            foreach (var e in world.Query(typeof(PlayerTag), typeof(Health)))
            {
                var hp = world.Get<Health>(e);
                hud = $"HP {hp.Current}/{hp.Max}";
                break;
            }

            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(hud.PadRight(ctx.Width));
            for (int y = 0; y < ctx.Height; y++)
            {
                for (int x = 0; x < ctx.Width; x++)
                {
                    Console.Write(_buffer[y, x]);
                }
                Console.WriteLine();
            }
        }
    }
}
