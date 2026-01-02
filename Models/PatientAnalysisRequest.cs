namespace MyHealthAI.Models
{
    public class PatientAnalysisRequest
    {
        // Temel Bilgiler
        public int Yas { get; set; }
        public string? Cinsiyet { get; set; } // Erkek/Kadın
        public double Boy { get; set; } // cm
        public double Kilo { get; set; } // kg
        public string? Meslek { get; set; } // Örn: Madenci, Yazılımcı

        // Kritik Anamnez Bilgileri (Nadir hastalıkları yakalamak için)
        public List<string> KronikHastaliklar { get; set; } = new();
        public List<string> AileOykusu { get; set; } = new(); // Örn: Babada erken kalp krizi
        public string? SeyahatGecmisi { get; set; } // Son 6 ay
        public List<string> KullandigiIlaclar { get; set; } = new();

        // Şikayet Detayları
        public string? AnaSikayet { get; set; } // "Göğüs ağrısı"
        public string? SikayetHikayesi { get; set; } // "3 gündür var, merdiven çıkınca artıyor"
        public List<string>? EkBelirtiler { get; set; } // "Mide bulantısı", "Terleme"

        // Lab Sonuçları (OCR'dan gelen ham metin buraya eklenecek)
        public string? LabSonuclariText { get; set; }
        public string? LabResmiBase64 { get; set; } // Resmin kendisi (uzun bir kod)
        public string? LabResmiMimeType { get; set; } // Resmin türü (image/jpeg veya image/png)
    }
}
