using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSExperiment
{
    public struct Position { public int X, Y; }
    public struct Velocity { public int dX, dY; }            // set by input/AI, consumed by movement
    public struct Renderable { public char Glyph; public ConsoleColor Color; }
    public struct PlayerTag { }                               // marker
    public struct AIWalker { public int StepEvery; public int Timer; public int Seed; }
    public struct Health { public int Current, Max; }
    public struct Solid { }                                   // blocks movement
    public struct DamageOnContact { public int Amount; }      // applied when two solid things overlap
    public struct DeadTag { }
}
