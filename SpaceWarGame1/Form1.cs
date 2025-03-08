using SpaceWarGame1;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;

namespace SpaceWarGame1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Arka plan resmini ayarla
            SetBackgroundImage();
        }

        // Exit butonuna tıklanıldığında uygulamayı kapatır.
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Play butonuna tıklanıldığında isim kontrolü yapılır
        private void btnPlay_Click(object sender, EventArgs e)
        {
            string playerName = txtPlayerName.Text.Trim();

            // Yalnızca harflerden oluşup oluşmadığını kontrol et
            if (IsValidPlayerName(playerName))
            {
                // Geçerli ise Form2'yi aç ve oyuncu adını gönder
                Form2 gameForm = new Form2(playerName);
                gameForm.StartPosition = FormStartPosition.CenterScreen;
                gameForm.Show();
                this.Hide(); // Form1'i gizle
            }
            else
            {
                // Hatalı giriş için mesaj kutusu
                MessageBox.Show("Invalid name! Please enter letters only.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPlayerName.Clear(); // TextBox'ı temizle
                txtPlayerName.Focus(); // TextBox'a odaklan
            }
        }

        // Yalnızca harflerden oluşup oluşmadığını kontrol eden metod
        private bool IsValidPlayerName(string name)
        {
            // Regex kullanarak sadece harf kontrolü (^[a-zA-Z]+$)
            return Regex.IsMatch(name, "^[a-zA-Z]+$");
        }

        private void SetBackgroundImage()
        {
            try
            {
                // Arka plan resminin yolu
                string imagePath = Path.Combine(Application.StartupPath, "Image", "resim.png");

                // Resmin varlığını kontrol et
                if (File.Exists(imagePath))
                {
                    this.BackgroundImage = Image.FromFile(imagePath);
                    this.BackgroundImageLayout = ImageLayout.Stretch; // Resmi form boyutuna göre ölçekle
                }
                else
                {
                    MessageBox.Show($"Arka plan resmi bulunamadı: {imagePath}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Arka plan resmi yüklenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Eğer ekstra bir işlev gerekirse buraya eklenebilir
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Label event'i boş bırakılabilir
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
