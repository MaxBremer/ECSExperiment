using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSExperiment.Core
{
    public sealed class GameContext
    {
        public int Width { get; }
        public int Height { get; }
        public bool Running { get; set; } = true;
        public GameContext(int w, int h) { Width = w; Height = h; }
    }
}
