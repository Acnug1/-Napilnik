using System;

namespace CleanCodeTask11
{
    class CleanCode_ExampleTask30
    {
        public void Enable(Effects effects)
        {
            effects.StartAnimation();
        }

        public void Disable(Pool pool)
        {
            pool.Free(this);
        }
    }
}
