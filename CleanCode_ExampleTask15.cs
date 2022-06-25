using System;
using System.Collections.Generic;

namespace CleanCodeTask6
{
    class Player { }
    class Gun { }
    class TargetFollower { }
    class Units
    {
        public IReadOnlyCollection<Unit> UnitsToGet { get; private set; }
    }
    class Unit { }
}
