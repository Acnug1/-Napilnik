using System;

namespace CleanCodeTask3
{
    class Weapon
    {
        private const int EmptyMagazine = 0;
        private const int BulletsPerShot = 1;
        private int _bullets;

        public bool CanShoot() => _bullets > EmptyMagazine;

        public void Shoot() => _bullets -= BulletsPerShot;
    }
}
