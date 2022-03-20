using System;

namespace Napilnik
{
    public class Weapon
    {
        private readonly int _damage;
        private int _bullets;

        public void TryFire(Player player)
        {
            if (player != null && _bullets > 0)
            {
                Fire(player);
            }
            else
                throw new InvalidOperationException();
        }

        private void Fire(Player player)
        {
            _bullets -= 1;
            player.ApplyDamage(_damage);
        }
    }

    public class Player
    {
        private int _health;

        public void ApplyDamage(int damage)
        {
            if (_health <= 0)
                throw new InvalidOperationException();

            if (damage < 0)
                throw new ArgumentOutOfRangeException(nameof(damage));
            else
            if (damage <= _health)
                _health -= damage;
            else
                _health = 0;
        }
    }

    public class Bot
    {
        private readonly Weapon _weapon = new Weapon();

        public void OnSeePlayer(Player player) => _weapon.TryFire(player);
    }
}
