using SpaceWarGame1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public class Game
{
    // Özellikler
    public Spaceship Player { get; private set; } // Oyuncu uzay aracı
    public List<Enemy> Enemies { get; private set; } = new List<Enemy>(); // Düşmanlar listesi

    private CollisionDetector collisionDetector; // CollisionDetector örneği
    public bool IsGameOver { get; private set; } = false; // Oyun bitiş durumu
    private int currentLevel = 1; // Mevcut seviye
    private int skor = 0;
    private Form2 gameForm; // Oyun alanı (Form2)

    public Game(Form2 form)
    {
        this.gameForm = form;
        this.collisionDetector = new CollisionDetector(); // CollisionDetector örneğini başlat
    }

    // Oyunu başlat
    public void StartGame()
    {
        IsGameOver = false;
        currentLevel = 1;
        skor = 0;
        Enemies.Clear();

        // Oyuncuyu oluştur
        string imagePath = Path.Combine(Application.StartupPath, "Image", "purplespaceship.png");

        if (!File.Exists(imagePath))
        {
            MessageBox.Show($"Dosya bulunamadı: {imagePath}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        Image spaceshipImage = Image.FromFile(imagePath);

        Player = new Spaceship(100, 10, 6, new Point(gameForm.ClientSize.Width / 2, gameForm.ClientSize.Height - 300), spaceshipImage);

        // İlk level'i başlat
        StartLevel();
    }

    // Level başlat
    private void StartLevel()
    {
        Enemies.Clear();
        AddEnemiesForCurrentLevel();

        foreach (var enemy in Enemies)
        {
            enemy.StartShooting(); // Her düşmanın zamanlayıcısını başlat
        }
        gameForm.Focus();

        // Sadece 4. seviyeye kadar mesaj göster
        if (currentLevel <= 4)
        {
            MessageBox.Show($"Level {currentLevel} başladı! Hazır olun!", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }



    // Mevcut level için düşmanları ekle
    // Mevcut level için düşmanları ekle
    private void AddEnemiesForCurrentLevel()
    {
        Random random = new Random();
        int screenWidth = gameForm.ClientSize.Width;
        int screenHeight = gameForm.ClientSize.Height;

        switch (currentLevel)
        {
            case 1:
                skor = 0;
                Player.Health = 60; // Level 1: Can değeri 60 olarak başlar
                for (int i = 0; i < 4; i++)
                {
                    string basicEnemyImagePath = Path.Combine("Image", "BasicEnemy.png");
                    Image basicEnemyImage = Image.FromFile(basicEnemyImagePath);

                    int randomX = random.Next(50, screenWidth - 50);
                    int randomY = random.Next(50, screenHeight / 2);

                    BasicEnemy basicEnemy = new BasicEnemy(randomX, randomY, basicEnemyImage);
                    Enemies.Add(basicEnemy);
                }
                break;

            case 2:
                Player.Health += 20; // Level 2: Can değerini 20 artır
                for (int i = 0; i < 2; i++)
                {
                    string basicEnemyImagePath = Path.Combine("Image", "BasicEnemy.png");
                    Image basicEnemyImage = Image.FromFile(basicEnemyImagePath);

                    int randomX = random.Next(50, screenWidth - 50);
                    int randomY = random.Next(50, screenHeight / 2);

                    BasicEnemy basicEnemy = new BasicEnemy(randomX, randomY, basicEnemyImage);
                    Enemies.Add(basicEnemy);
                }

                for (int i = 0; i < 3; i++)
                {
                    string fastEnemyImagePath = Path.Combine("Image", "FastEnemy.png");
                    Image fastEnemyImage = Image.FromFile(fastEnemyImagePath);

                    int randomX = random.Next(50, screenWidth - 50);
                    int randomY = random.Next(50, screenHeight / 2);

                    FastEnemy fastEnemy = new FastEnemy(randomX, randomY, fastEnemyImage);
                    Enemies.Add(fastEnemy);
                }
                skor = 200;
                break;

            case 3:
                Player.Health += 40; // Level 3: Can değerini 40 artır
                for (int i = 0; i < 1; i++)
                {
                    string basicEnemyImagePath = Path.Combine("Image", "BasicEnemy.png");
                    Image basicEnemyImage = Image.FromFile(basicEnemyImagePath);

                    int randomX = random.Next(50, screenWidth - 50);
                    int randomY = random.Next(50, screenHeight / 2);

                    BasicEnemy basicEnemy = new BasicEnemy(randomX, randomY, basicEnemyImage);
                    Enemies.Add(basicEnemy);
                }

                for (int i = 0; i < 2; i++)
                {
                    string fastEnemyImagePath = Path.Combine("Image", "FastEnemy.png");
                    Image fastEnemyImage = Image.FromFile(fastEnemyImagePath);

                    int randomX = random.Next(50, screenWidth - 50);
                    int randomY = random.Next(50, screenHeight / 2);

                    FastEnemy fastEnemy = new FastEnemy(randomX, randomY, fastEnemyImage);
                    Enemies.Add(fastEnemy);
                }

                for (int i = 0; i < 2; i++)
                {
                    string strongEnemyImagePath = Path.Combine("Image", "StrongEnemy.png");
                    Image strongEnemyImage = Image.FromFile(strongEnemyImagePath);

                    int randomX = random.Next(50, screenWidth - 50);
                    int randomY = random.Next(50, screenHeight / 2);

                    StrongEnemy strongEnemy = new StrongEnemy(randomX, randomY, strongEnemyImage);
                    Enemies.Add(strongEnemy);
                }
                skor = 300;
                break;

            case 4:
                Player.Health += 60; // Level 4: Can değerini 60 artır
                for (int i = 0; i < 2; i++)
                {
                    string bossEnemyImagePath = Path.Combine("Image", "BossEnemy.png");
                    Image bossEnemyImage = Image.FromFile(bossEnemyImagePath);

                    int randomX = random.Next(50, screenWidth - 50);
                    int randomY = random.Next(50, screenHeight / 2);

                    BossEnemy bossEnemy = new BossEnemy(randomX, randomY, bossEnemyImage);
                    Enemies.Add(bossEnemy);
                    skor = 400;
                }
                break;

            default:
                EndGame(); // Eğer level sayısı 4'ten büyükse oyun biter
                return;
        }
    }
    // Oyunu her turda güncelle
    public void UpdateGame()
    {
        if (IsGameOver) return;

        Player.UpdateBullets();

        foreach (var enemy in Enemies)
        {
            enemy.MoveTowards(Player.Position.X, Player.Position.Y); // Oyuncuya yönel
            enemy.UpdateBullets(); // Düşman mermilerini güncelle

            // Her 2 saniyede bir mermi ateşle
            if (DateTime.Now.Ticks % (2 * TimeSpan.TicksPerSecond) == 0)
            {
                enemy.Shoot();
            }

            // Gerektiğinde belleği temizle
            if (DateTime.Now.Second % 2 == 0) // Her 2 saniyede bir
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        CheckCollisions();

        if (Enemies.Count == 0)
        {
            currentLevel++;
            StartLevel();
        }
    }







    // Çarpışmaları kontrol et
    private void CheckCollisions()
    {
        // Spaceship ve Enemy çarpışmasını kontrol et
        foreach (var enemy in Enemies)
        {
            Rectangle playerRect = new Rectangle(Player.Position.X, Player.Position.Y, Player.ShipImage.Width, Player.ShipImage.Height);
            Rectangle enemyRect = new Rectangle(enemy.X, enemy.Y, enemy.Width, enemy.Height);

            if (collisionDetector.AreColliding(playerRect, enemyRect))
            {
                collisionDetector.HandleCollision(Player, enemy);
                if (Player.Health <= 0)
                {
                    EndGame();
                }
                if (enemy.IsDestroyed())
                {
                    Enemies.Remove(enemy);
                    break; // Düşman listesi değiştiği için döngüyü kır
                }
            }
        }

        // Mermilerin çarpışmalarını kontrol et (mevcut kod)
        for (int i = Player.Bullets.Count - 1; i >= 0; i--)
        {
            Bullet playerBullet = Player.Bullets[i];
            for (int j = Enemies.Count - 1; j >= 0; j--)
            {
                Enemy enemy = Enemies[j];
                if (IsColliding(playerBullet, enemy))
                {
                    enemy.TakeDamage(playerBullet.Damage);
                    Player.Bullets.RemoveAt(i);
                    if (enemy.IsDestroyed())
                    {
                        Enemies.RemoveAt(j);
                    }
                    break;
                }
            }
        }

        foreach (var enemy in Enemies)
        {
            for (int i = enemy.Bullets.Count - 1; i >= 0; i--)
            {
                Bullet enemyBullet = enemy.Bullets[i];
                if (IsColliding(enemyBullet, Player))
                {
                    Player.TakeDamage(enemyBullet.Damage);
                    enemy.Bullets.RemoveAt(i);
                    if (Player.Health <= 0)
                    {
                        EndGame();
                    }
                }
            }
        }
    }

    private bool IsColliding(Bullet bullet, object target)
    {
        Rectangle bulletRect = new Rectangle(bullet.X, bullet.Y, bullet.Width, bullet.Height);
        if (target is Enemy enemy)
        {
            Rectangle enemyRect = new Rectangle(enemy.X, enemy.Y, enemy.Width, enemy.Height);
            return bulletRect.IntersectsWith(enemyRect);
        }
        else if (target is Spaceship player)
        {
            Rectangle playerRect = new Rectangle(player.Position.X, player.Position.Y, player.ShipImage.Width, player.ShipImage.Height);
            return bulletRect.IntersectsWith(playerRect);
        }
        return false;
    }

    // Oyunu bitir
    private void EndGame()
    {
        IsGameOver = true;

        // Tüm düşmanların saldırılarını durdur
        foreach (var enemy in Enemies)
        {
            enemy.StopShooting();
        }

        // Oyun bitiş mesajını belirle
        string message;
        if (currentLevel == 4 && Enemies.Count == 0 && Player.Health > 0)
        {
            message = $"Tebrikler, kazandınız! Skorunuz: {skor}";
        }
        else
        {
            message = $"Oyun Bitti! Skorunuz: {skor}";
        }

        // MessageBox ile sadece oyun sonucu ve skor gösterilir
        MessageBox.Show(message, "Oyun Sonu", MessageBoxButtons.OK, MessageBoxIcon.Information);

        // Skoru kaydet
        Utility.SaveScore(gameForm.PlayerName, skor);

        // Skor tablosu formunu aç (isteğe bağlı)
        ScoreBoardForm scoreBoardForm = new ScoreBoardForm();
        scoreBoardForm.StartPosition = FormStartPosition.CenterScreen;
        scoreBoardForm.ShowDialog();
    }
}
