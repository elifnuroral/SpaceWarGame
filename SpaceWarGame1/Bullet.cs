using System;
using System.Drawing;

public class Bullet
{
    // Merminin özellikleri
    public int X { get; set; }          // Merminin X konumu
    public int Y { get; set; }          // Merminin Y konumu
    public int Width { get; set; } = 30; // Merminin genişliği
    public int Height { get; set; } = 20; // Merminin yüksekliği
    public int Speed { get; set; }  // Merminin hızı
    public int Damage { get; set; }      // Merminin verdiği hasar
    public float RotationAngle { get; set; } // Merminin hareket yönü

    private static Image BulletImage = Image.FromFile("Image\\bulletalev.png");

    // Yapıcı metot
    public Bullet()
    {
        BulletImage = BulletImage ?? Image.FromFile("Image\\bulletalev.png");
    }

    // Merminin hareket etmesini sağlayacak metot
    public void Move()
    {
        // Mermiyi RotationAngle yönünde hareket ettir
        X += (int)(Math.Cos(RotationAngle * Math.PI / 180) * Speed);
        Y += (int)(Math.Sin(RotationAngle * Math.PI / 180) * Speed);
    }

    public void OnHit(object target)
    {
        if (target is Enemy enemy)
        {
            enemy.TakeDamage(Damage); // Düşmana hasar uygula
        }
        else if (target is Spaceship player)
        {
            player.TakeDamage(Damage); // Oyuncuya hasar uygula
        }
        Dispose(); // Mermiyi yok et
    }

    public void Dispose()
    {
        // Mermiyi bellekten kaldırma işlemleri
        Console.WriteLine("Mermi yok edildi.");
    }

    // Merminin ekran dışına çıkıp çıkmadığını kontrol etme
    public bool IsOutOfBounds(int boundaryWidth, int boundaryHeight)
    {
        return X < 0 || X > boundaryWidth || Y < 0 || Y > boundaryHeight;
    }

    // Mermiyi çizme işlemi
    public void Draw(Graphics g)
    {
        if (BulletImage != null)
        {
            g.TranslateTransform(X + Width / 2, Y + Height / 2); // Merminin merkezini referans al
            g.RotateTransform(RotationAngle); // Mermiyi döndür
            g.DrawImage(BulletImage, -Width / 2, -Height / 2, Width, Height);
            g.ResetTransform();
        }
        else
        {
            g.FillRectangle(Brushes.Red, X, Y, Width, Height); // Varsayılan dikdörtgen
        }
    }


}
