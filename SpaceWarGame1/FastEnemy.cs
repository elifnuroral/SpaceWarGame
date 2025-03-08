using System;
using System.Drawing;
public class FastEnemy : Enemy
{
    public FastEnemy(int x, int y, Image enemyImage)
        : base(health: 30, speed: 3, damage: 10, x: x, y: y, enemyImage: enemyImage)
    {
    }

    public override void Move()
    {
        // FastEnemy daha hızlı ve zikzak hareket eder
        X += (int)(Speed * Math.Sin(Y * 0.5));
        Y += Speed;
    }

    public override void Attack()
    {
        Shoot(); // Mermiyi ateşle
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount); // Temel hasar alma işlemi
    }
}
