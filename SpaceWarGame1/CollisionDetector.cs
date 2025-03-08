using System.Drawing;

public class CollisionDetector
{
    // İki obje arasında çarpışmayı kontrol eder
    public bool AreColliding(Rectangle object1, Rectangle object2)
    {
        return object1.IntersectsWith(object2);
    }

    // Çarpışma durumunda zarar verir
    public void HandleCollision(Spaceship player, Enemy enemy)
    {
        if (player != null && enemy != null)
        {
            player.TakeDamage(5); // Spaceship 5 can kaybeder
            enemy.TakeDamage(5);  // Enemy 5 can kaybeder
        }
    }
}
