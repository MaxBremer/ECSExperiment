using ECSExperiment.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECSExperiment
{
    public readonly record struct DealDamage(Entity Target, int Amount);
    public readonly record struct Killed(Entity Victim);
}
