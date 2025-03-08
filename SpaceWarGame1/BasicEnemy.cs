using System.Drawing;
using System;
using System.Collections.Generic;
public class BasicEnemy : Enemy
{
    public BasicEnemy(int x, int y, Image enemyImage)
        : base(health: 30, speed: 2, damage: 5, x: x, y: y, enemyImage: enemyImage)
    {
    }

    public override void Move()
    {
        // BasicEnemy sadece düz aşağı hareket eder
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
