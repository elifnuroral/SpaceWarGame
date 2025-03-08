using System;
using System.Drawing;

public class StrongEnemy : Enemy
{
    public StrongEnemy(int x, int y, Image enemyImage)
        : base(health: 100, speed: 2, damage: 20, x: x, y: y, enemyImage: enemyImage)
    {
    }

    public override void Move()
    {
        // StrongEnemy yavaş ama düz bir şekilde aşağı hareket eder
        Y += Speed;
    }

    public override void Attack()
    {
        // Daha güçlü bir mermiyle saldırır
        Bullet strongBullet = new Bullet
        {
            X = this.X + (this.Width / 2) - 5,
            Y = this.Y + this.Height,
            Width = 30,
            Height = 20,
            Speed = 4,
            Damage = this.Damage
        };
        Bullets.Add(strongBullet);
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount); // Temel hasar alma işlemi
    }
}
