using System.Collections.Generic;
using System.Drawing;
using System;
using System.Windows.Forms;
using System.Media;

public class Spaceship
{
    // Özellikler
    public int Health { get; set; }
    public int Damage { get; set; }
    public int Speed { get; set; }
    public Point Position { get; set; }
    public Image ShipImage { get; set; }

    private float rotationAngle = 0;  // Uzay aracının yönü (derece cinsinden)
    public List<Bullet> Bullets { get; set; } = new List<Bullet>(); // Mermilerin listesi

    // Yapıcı metod
    public Spaceship(int health, int damage, int speed, Point position, Image shipImage)
    {
        Health = health;
        Damage = damage;
        Speed = speed;
        Position = position;
        // Görseli sabit boyutlandır
        ShipImage = ResizeImage(shipImage, 60, 60); // 30x30 boyutunda sabit
    }

    private Image ResizeImage(Image image, int width, int height)
    {
        Bitmap resizedBitmap = new Bitmap(image, new Size(width, height));
        return resizedBitmap;
    }

    // Uzay aracını hareket ettir (yukarı hareket)
    public void MoveUp()
    {
        Position = new Point(
            Position.X + (int)(Speed * Math.Cos(rotationAngle * Math.PI / 180)),
            Position.Y + (int)(Speed * Math.Sin(rotationAngle * Math.PI / 180))
        );

        // Ekran sınırlarını kontrol et
        WrapAround(Form.ActiveForm.ClientSize);
    }


    // Uzay aracını sola döndür
    public void RotateLeft()
    {
        rotationAngle -= 4;  // Yönü sola çevir
        if (rotationAngle < 0) rotationAngle += 360;  // Açı negatif olmasın
    }

    // Uzay aracını sağa döndür
    public void RotateRight()
    {
        rotationAngle += 4;  // Yönü sağa çevir
        if (rotationAngle >= 360) rotationAngle -= 360;  // 360 dereceyi geçmesin
    }

    // Uzay aracını ekranda çizme işlemi
    public void DrawShip(Graphics g)
    {
        g.TranslateTransform(Position.X + ShipImage.Width / 2, Position.Y + ShipImage.Height / 2);
        g.RotateTransform(rotationAngle);
        g.DrawImage(ShipImage, -ShipImage.Width / 2, -ShipImage.Height / 2);
        g.ResetTransform();
    }

    // Mermi atma işlemi
    public void Fire()
    {
        // Merminin başlangıç konumu, uzay aracının yönüne göre olacak
        Bullet newBullet = new Bullet
        {
            X = Position.X + (int)(Math.Cos(rotationAngle * Math.PI / 180) * ShipImage.Width / 2),  // Sağ tarafa yerleştir
            Y = Position.Y + (int)(Math.Sin(rotationAngle * Math.PI / 180) * ShipImage.Width / 2),  // Yükseklik hesaplaması
            Damage = this.Damage,  // Uzay aracının hasarı
            Speed = 10,  // Merminin hızı
            RotationAngle = this.rotationAngle  // Uzay aracının yönüne göre mermiyi ateşle
        };

        // Mermiyi Bullets listesine ekle
        Bullets.Add(newBullet);
    }


    // Mermileri hareket ettir ve ekran dışına çıkmış mermileri temizle
    public void UpdateBullets()
    {
        // Ekranın genişlik ve yükseklik değerlerini al
        int screenWidth = Screen.PrimaryScreen.Bounds.Width;
        int screenHeight = Screen.PrimaryScreen.Bounds.Height;

        for (int i = Bullets.Count - 1; i >= 0; i--)
        {
            Bullets[i].Move(); // Mermiyi hareket ettir

            // Eğer mermi ekran dışına çıkmışsa, listeden kaldır
            if (Bullets[i].IsOutOfBounds(screenWidth, screenHeight))
            {
                Bullets.RemoveAt(i);
            }
        }
    }



    // Hasar alma işlemi
    public void TakeDamage(int amount)
    {
        Health -= amount;  // Sağlık miktarını azalt
        if (Health <= 0)
        {
            Health = 0;
            // Eğer sağlık sıfıra ulaşırsa oyun biter veya oyuncu ölür
        }
    }

    // Ekran sınırlarını kontrol ederek uzay aracını karşı kenara taşı
    public void WrapAround(Size screenSize)
    {
        // Sağdan çıkarsa soldan girsin
        if (Position.X >= screenSize.Width)
            Position = new Point(0, Position.Y);

        // Soldan çıkarsa sağdan girsin
        if (Position.X + ShipImage.Width < 0)
            Position = new Point(screenSize.Width - 1, Position.Y);

        // Aşağıdan çıkarsa yukarıdan girsin
        if (Position.Y >= screenSize.Height)
            Position = new Point(Position.X, 0);

        // Yukarıdan çıkarsa aşağıdan girsin
        if (Position.Y + ShipImage.Height < 0)
            Position = new Point(Position.X, screenSize.Height - 1);
    }


    public void DrawHealth(Graphics g, int screenWidth)
    {
        try
        {
            Image heartImage = Image.FromFile("Image/pinkheart.png"); // Kalp görseli
            int heartWidth = 60; // Kalp genişliği
            int heartHeight = 60; // Kalp yüksekliği
            int spacing = 5; // Kalpler arasındaki boşluk

            // Kalplerin başlangıç konumunu hesapla (orta üstte hizalama)
            int startX = (screenWidth - (Health / 10 * (heartWidth + spacing))) / 2;
            int startY = 10; // Ekranın üstünden biraz boşluk bırak

            // Her 10 can için bir kalp çiz
            for (int i = 0; i < Health / 10; i++)
            {
                g.DrawImage(heartImage, startX + (i * (heartWidth + spacing)), startY, heartWidth, heartHeight);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Kalpler çizilirken bir hata oluştu: " + ex.Message);
        }
    }



}
