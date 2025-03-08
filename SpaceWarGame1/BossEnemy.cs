using System;
using System.Drawing;

public class BossEnemy : Enemy
{
    public BossEnemy(int x, int y, Image enemyImage)
        : base(health: 150, speed: 2, damage: 50, x: x, y: y, enemyImage: enemyImage)
    {
    }

    public override void Move()
    {
        // BossEnemy sağa sola hareket ederken yavaşça aşağı iner
        X += (int)(Speed * Math.Sin(Y * 0.02));
        Y += Speed / 2;
    }

    public override void Attack()
    {
        // Birden fazla mermi ateşler
        for (int i = -1; i <= 1; i++)
        {
            Bullet bossBullet = new Bullet
            {
                X = this.X + (this.Width / 2) - 5 + (i * 10),
                Y = this.Y + this.Height,
                Width = 30,
                Height = 20,
                Speed = 4,
                Damage = this.Damage
            };
            Bullets.Add(bossBullet);
        }
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount); // Temel hasar alma işlemi
    }
}
