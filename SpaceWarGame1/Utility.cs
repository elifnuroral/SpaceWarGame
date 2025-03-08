using System;
using System.IO;
using System.Windows.Forms;

public static class Utility
{
    public static readonly string ScoreFilePath = Path.Combine(Application.StartupPath, "scores.txt");

    // Text dosyası yoksa oluştur
    public static void EnsureScoreFileExists()
    {
        if (!File.Exists(ScoreFilePath))
        {
            File.Create(ScoreFilePath).Close(); // Dosya oluştur ve kapat
        }
    }


    public static void SaveScore(string playerName, int score)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(ScoreFilePath, true))
            {
                writer.WriteLine($"{playerName} - {score}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Skor kaydedilirken bir hata oluştu: {ex.Message}");
        }
    }


    public static string LoadScores()
    {
        try
        {
            if (File.Exists(ScoreFilePath))
            {
                return File.ReadAllText(ScoreFilePath);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Skorlar yüklenirken bir hata oluştu: {ex.Message}");
        }
        return "Henüz skor yok.";
    }
}
