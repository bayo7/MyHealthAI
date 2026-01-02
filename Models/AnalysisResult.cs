namespace MyHealthAI.Models
{
    public class AnalysisResult
    {
        public List<PotentialCondition>? Olasiliklar { get; set; }
        public List<LabWarning>? LabUyarilari { get; set; }
        public string? OzetDegerlendirme { get; set; }
    }
    public class PotentialCondition
    {
        public string? HastalikAdi { get; set; }
        public string? OlasilikSeviyesi { get; set; } // Yüksek, Orta, Düşük, Çok Nadir
        public string? NedenBuTahmin { get; set; } // Yapay zekanın gerekçesi
        public string? OnerilenBolum { get; set; } // **İstediğin özellik: Kardiyoloji, KBB vb.**
        public bool AciliyetVarMi { get; set; } // True ise kullanıcıyı hemen uyaracağız
    }

    public class LabWarning
    {
        public string? Parametre { get; set; } // Örn: D Vitamini
        public string? Durum { get; set; } // Düşük/Yüksek
        public string? Aciklama { get; set; }
    }
}
