using System.Drawing;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Timers;
using System.IO;

public abstract class Enemy
{
    // Temel özellikler
    public int Health { get; set; }  // Düşmanın can seviyesi
    public int Speed { get; set; }   // Düşmanın hareket hızı
    public int Damage { get; set; }  // Düşmanın verebileceği hasar
    public int X { get; set; }       // Düşmanın X konumu
    public int Y { get; set; }       // Düşmanın Y konumu
    public int Width { get; set; } = 50; // Düşmanın genişliği
    public int Height { get; set; } = 50; // Düşmanın yüksekliği
    public Image EnemyImage { get; set; } // Düşmanın görseli
    public float RotationAngle { get; set; } // Düşmanın hareket yönü (derece cinsinden)
    public System.Timers.Timer ShootTimer { get; private set; }


    public List<Bullet> Bullets { get; set; } = new List<Bullet>(); // Mermilerin listesi

    // Yapıcı metod
    public Enemy(int health, int speed, int damage, int x, int y, Image enemyImage)
    {
        Health = health;
        Speed = speed;
        Damage = damage;
        X = x;
        Y = y;
        EnemyImage = enemyImage;

        ShootTimer = new System.Timers.Timer();
        ShootTimer.Interval = 4000; // 4 saniyede bir ateşler (değiştirilebilir)
        ShootTimer.Elapsed += (sender, e) => Shoot(); // Timer tetiklendiğinde Shoot çağrılır
        ShootTimer.Start();
    }

    // Soyut metodlar (Alt sınıflarda tanımlanacak)
    public abstract void Move(); // Düşmanın hareket davranışı
    public abstract void Attack(); // Düşmanın saldırı davranışı

    // Hasar alma işlemi
    public virtual void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Destroy();  // Düşman yok edildiğinde çağrılır
        }
    }

    // Düşman yok edilme işlemi
    public virtual void Destroy()
    {
        // Düşman yok edildiğinde yapılacak işlemler (örneğin, skor artırma)
        ShootTimer?.Stop();
        ShootTimer?.Dispose(); // Zamanlayıcıyı serbest bırak
    }

    public void DisposeResources()
    {

        EnemyImage?.Dispose();
        ShootTimer?.Dispose();
    }


    // Can seviyesini çizme işlemi
    public void DrawHealthSymbols(Graphics g)
    {
        try
        {
            string heartImagePath = Path.Combine(Application.StartupPath, "Image", "kalp.png");

            // Resmin varlığını kontrol et
            if (!File.Exists(heartImagePath))
            {
                Console.WriteLine($"Kalp resmi bulunamadı: {heartImagePath}");
                return;
            }

            Image heartImage = Image.FromFile(heartImagePath);

            int heartWidth = 20; // Kalp genişliği
            int heartHeight = 20; // Kalp yüksekliği
            int startX = X;      // Düşmanın sol üst köşesi
            int startY = Y - 15; // Düşmanın üstünden 15 piksel yukarıda

            int numHearts = Math.Max(0, Health / 10); // 10 can = 1 kalp

            if (numHearts == 0)
            {
                Console.WriteLine("Çizilecek kalp yok (Can seviyesi düşük).");
                return;
            }

            for (int i = 0; i < numHearts; i++)
            {
                g.DrawImage(heartImage, startX + (i * heartWidth), startY, heartWidth, heartHeight);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Can sembolü çizilirken bir hata oluştu: {ex.Message}");
        }
    }



    // Düşmanı çizme işlemi
    public virtual void Draw(Graphics g)
    {
        if (EnemyImage != null)
        {
            // Düşman görselini döndürerek çiz
            g.TranslateTransform(X + Width / 2, Y + Height / 2);
            g.RotateTransform(RotationAngle - 90); // Sol tarafı ön yapmak için -90 derece
            g.DrawImage(EnemyImage, -Width / 2, -Height / 2, Width, Height);
            g.ResetTransform();
        }

        // Kalp sembollerini düşmanın üzerine çiz
        DrawHealthSymbols(g);

        // Düşman mermilerini çiz
        foreach (var bullet in Bullets)
        {
            bullet.Draw(g);
        }
    }




    // Hedefe doğru hareket etmek için metot
    public void MoveTowards(int targetX, int targetY)
    {
        int deltaX = targetX - X; // X eksenindeki fark
        int deltaY = targetY - Y; // Y eksenindeki fark

        double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY); // Mesafe

        if (distance > 0)
        {
            // Hareket
            X += (int)(Speed * (deltaX / distance)); // X ekseni hareketi
            Y += (int)(Speed * (deltaY / distance)); // Y ekseni hareketi

            // Rotasyonu hesapla
            RotationAngle = (float)(Math.Atan2(deltaY, deltaX) * 180 / Math.PI);

            // Pozitif bir açıya dönüştür (0-360 derece arası)
            if (RotationAngle < 0)
            {
                RotationAngle += 360;
            }
        }
    }



    public void StartShooting()
    {
        ShootTimer.Start();
    }

    public void StopShooting()
    {
        ShootTimer.Stop();
    }


    // Mermi atma işlemi
    public void Shoot()
    {
        // Merminin çıkış noktasını düşmanın ön kısmına göre hesapla
        int bulletStartX = X + (int)(Math.Cos(RotationAngle * Math.PI / 180) * Width / 2);
        int bulletStartY = Y + (int)(Math.Sin(RotationAngle * Math.PI / 180) * Height / 2);

        // Yeni mermi oluştur
        Bullet newBullet = new Bullet
        {
            X = bulletStartX,
            Y = bulletStartY,
            Width = 30,
            Height = 20,
            Speed = 4,
            Damage = this.Damage,
            RotationAngle = this.RotationAngle // Merminin hareket yönü
        };

        // Mermiyi listeye ekle
        Bullets.Add(newBullet);

        Console.WriteLine($"Mermi ateşlendi! Başlangıç: ({bulletStartX}, {bulletStartY}), Yön: {RotationAngle}°");
    }









    public void UpdateBullets()
    {
        // Ekranın genişlik ve yükseklik değerlerini al
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;

        for (int i = Bullets.Count - 1; i >= 0; i--)
        {
            Bullets[i].Move(); // Mermiyi hareket ettir

            // Mermi ekran dışına çıktıysa kaldır
            if (Bullets[i].IsOutOfBounds(screenWidth, screenHeight))
            {
                Console.WriteLine("Mermi ekran dışına çıktı ve kaldırıldı.");
                Bullets.RemoveAt(i);
            }
        }
    }






    // Düşman yok olma kontrolü
    public virtual bool IsDestroyed()
    {
        return Health <= 0;
    }
}
