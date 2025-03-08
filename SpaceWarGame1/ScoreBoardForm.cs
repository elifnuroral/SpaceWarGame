using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace SpaceWarGame1
{
    public partial class ScoreBoardForm : Form
    {
        public ScoreBoardForm()
        {
            InitializeComponent();

            // Form başlığı
            this.Text = "Skor Tablosu";
            this.Width = 600;
            this.Height = 500;
            this.StartPosition = FormStartPosition.CenterScreen;

            // Arka plan rengini açık pembe yap
            this.BackColor = System.Drawing.Color.Indigo;

            // Başlık arka planı için panel
            Panel titleBackgroundPanel = new Panel
            {
                BackColor = Color.Plum,
                Location = new Point(0, 10),
                Size = new Size(this.Width, 60)
            };
            this.Controls.Add(titleBackgroundPanel);

            // Başlık
            Label lblTitle = new Label
            {
                Text = "Skor Tablosu",
                Dock = DockStyle.Fill,
                Font = new System.Drawing.Font("Impact", 28, System.Drawing.FontStyle.Bold),
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter
            };
            titleBackgroundPanel.Controls.Add(lblTitle);

            // DataGridView
            DataGridView dgvScores = new DataGridView
            {
                Width = 500,
                Height = 250,
                Location = new System.Drawing.Point(50, 100),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Regular),
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold)
                },
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Sütunları ekle
            dgvScores.Columns.Add("PlayerName", "Oyuncu Adı");
            dgvScores.Columns.Add("Score", "Skor");

            // Sütun genişliklerini ayarla
            dgvScores.Columns[0].FillWeight = 70;
            dgvScores.Columns[1].FillWeight = 30;

            this.Controls.Add(dgvScores);

            // Skorları yükle
            LoadScores(dgvScores);

            // Butonları oluştur ve hizala
            CreateButtons(dgvScores);
        }

        private void CreateButtons(DataGridView dgvScores)
        {
            var buttonFont = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
            var buttonSize = new System.Drawing.Size(150, 50);
            var buttonBackColor = Color.Purple;
            var buttonForeColor = Color.Black;

            int formWidth = this.ClientSize.Width;
            int buttonSpacing = 20;
            int totalButtonsWidth = (buttonSize.Width * 3) + (buttonSpacing * 2);
            int startX = (formWidth - totalButtonsWidth) / 2;

            // Kapat butonu
            Button btnClose = new Button
            {
                Text = "Kapat",
                Size = buttonSize,
                Font = buttonFont,
                BackColor = buttonBackColor,
                ForeColor = buttonForeColor,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 2, BorderColor = Color.Black },
                Location = new System.Drawing.Point(startX, this.ClientSize.Height - buttonSize.Height - 20)
            };
            btnClose.Click += (s, e) => this.Close();
            this.Controls.Add(btnClose);

            // Skoru temizle butonu
            Button btnClear = new Button
            {
                Text = "Skoru Temizle",
                Size = buttonSize,
                Font = buttonFont,
                BackColor = buttonBackColor,
                ForeColor = buttonForeColor,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 2, BorderColor = Color.Black },
                Location = new System.Drawing.Point(startX + buttonSize.Width + buttonSpacing, this.ClientSize.Height - buttonSize.Height - 20)
            };
            btnClear.Click += (s, e) =>
            {
                ClearScores(dgvScores);
            };
            this.Controls.Add(btnClear);

            // Yeniden Oyna butonu
            Button btnRestart = new Button
            {
                Text = "Yeniden Oyna",
                Size = buttonSize,
                Font = buttonFont,
                BackColor = buttonBackColor,
                ForeColor = buttonForeColor,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 2, BorderColor = Color.Black },
                Location = new System.Drawing.Point(startX + 2 * (buttonSize.Width + buttonSpacing), this.ClientSize.Height - buttonSize.Height - 20)
            };
            btnRestart.Click += (s, e) =>
            {
                RestartGame();
            };
            this.Controls.Add(btnRestart);
        }

        private void LoadScores(DataGridView dgv)
        {
            if (File.Exists(Utility.ScoreFilePath))
            {
                dgv.Rows.Clear();
                var lines = File.ReadAllLines(Utility.ScoreFilePath);
                foreach (var line in lines)
                {
                    var parts = line.Split('-');
                    if (parts.Length == 2)
                    {
                        dgv.Rows.Add(parts[0].Trim(), parts[1].Trim());
                    }
                }
            }
        }

        private void ClearScores(DataGridView dgv)
        {
            try
            {
                File.Delete(Utility.ScoreFilePath);
                dgv.Rows.Clear(); // Tabloyu hemen temizle
                MessageBox.Show("Tüm skorlar temizlendi!", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Skorlar temizlenirken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RestartGame()
        {
            // OpenForms koleksiyonunun kopyasını al
            var openForms = Application.OpenForms.Cast<Form>().ToList();

            // Formları kapat
            foreach (Form form in openForms)
            {
                if (form is Form2 || form is ScoreBoardForm)
                {
                    form.Close();
                }
            }

            // Form1'i yeniden başlat
            new Form1().Show();
        }

    }
}
