using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SpaceWarGame1
{
    public partial class Form2 : Form
    {
        private Game game; // Game sınıfını kontrol etmek için
        private bool isUpPressed = false;
        private bool isLeftPressed = false;
        private bool isRightPressed = false;
        private bool isSpacePressed = false;

        private string playerName; // Oyuncu adı burada saklanacak
        public string PlayerName { get; private set; } // Oyuncu adı için bir özellik

        public Form2(string playerName)
        {
            InitializeComponent();
            this.playerName = playerName; // Oyuncu adını al
            PlayerName = playerName; // Özelliği doldur

            // DoubleBuffered özelliğini açıyoruz
            this.DoubleBuffered = true;

            // Game sınıfını başlat
            game = new Game(this);

            // Oyunu başlat
            game.StartGame();

            // Arka plan resmini ayarlayın
            byte[] imageBytes = Properties.Resources.background1;
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                this.BackgroundImage = Image.FromStream(ms);
            }

            this.BackgroundImageLayout = ImageLayout.Stretch;

            // Form üzerindeki tuş olaylarını bağla
            this.KeyDown += Form2_KeyDown;
            this.KeyUp += Form2_KeyUp;
        }

        // Klavye tuşlarına basıldığında
        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    isUpPressed = true;
                    break;

                case Keys.A:
                    isLeftPressed = true;
                    break;

                case Keys.D:
                    isRightPressed = true;
                    break;

                case Keys.Space:
                    if (!isSpacePressed)
                    {
                        isSpacePressed = true;
                        game.Player.Fire();
                    }
                    break;
            }
        }

        // Klavye tuşları bırakıldığında
        private void Form2_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    isUpPressed = false;
                    break;

                case Keys.A:
                    isLeftPressed = false;
                    break;

                case Keys.D:
                    isRightPressed = false;
                    break;

                case Keys.Space:
                    isSpacePressed = false;
                    break;
            }
        }

        // Çizim ve güncelleme işlemleri
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Oyuncunun hareketini kontrol et
            if (isUpPressed)
                game.Player.MoveUp();
            if (isLeftPressed)
                game.Player.RotateLeft();
            if (isRightPressed)
                game.Player.RotateRight();

            // Oyunu güncelle
            game.UpdateGame(); // Düşmanların hareketi ve mermi atışı burada kontrol edilir

            // Oyuncuyu çiz
            game.Player.DrawShip(e.Graphics);

            // Oyuncu mermilerini çiz
            foreach (var bullet in game.Player.Bullets)
            {
                bullet.Draw(e.Graphics);
            }

            // Düşmanları ve mermilerini çiz
            foreach (var enemy in game.Enemies)
            {
                enemy.Draw(e.Graphics);
            }

            // Spaceship'in canını çizen kod (eksikse ekleyin)
            game.Player.DrawHealth(e.Graphics, this.ClientSize.Width);

            // Sürekli güncelleme için yeniden çizim tetiklenir
            this.Invalidate();
        }

    }
}
