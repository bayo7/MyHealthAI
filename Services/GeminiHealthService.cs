using MyHealthAI.Models;
using System.Text;
using System.Text.Json;

namespace MyHealthAI.Services
{
    public class GeminiHealthService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GeminiHealthService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<AnalysisResult> AnalyzePatientAsync(PatientAnalysisRequest request)
        {
            var apiKey = _configuration["GoogleGemini:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new Exception("API Key bulunamadı! appsettings.json dosyasını kontrol et.");
            }

            // 1. Kullanıcı verisini JSON string'e çevir (Prompt içine gömmek için)
            var jsonOptions = new JsonSerializerOptions { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            string patientJson = JsonSerializer.Serialize(request, jsonOptions);

            // 2. Prompt'u Güncelle (Gelişmiş Tıbbi Mantık ve Korelasyon Ayarı)
            string fullPrompt = $@"
            SİSTEM TALİMATI:
            Sen uzman bir Tıbbi Ön Değerlendirme (Triage) Asistanısın.
            
            TEMEL KURALLAR:
            1. Asla kesin tanı koyma.
            2. Görseldeki laboratuvar değerlerini oku (OCR) ve sadece referans dışı olanları yorumla.
            3. SADECE JSON formatında cevap ver.

            KRİTİK ANALİZ MANTIĞI (BUNLARA KESİNLİKLE UY):
            
            KURAL 1: YANLIŞ POZİTİFLERİ ELe (Tahlil Yorumlama)
            - İdrar Analizi: Şerit testinde (Strip) 'Hemoglobin' veya 'Kan' pozitif/eser olsa bile, mikroskopi (Eritrosit/RBC) sonucu normalse (0-3 arası), bunu 'İdrar Yolu Enfeksiyonu' veya 'Taş' belirtisi olarak SAYMA. Bunu 'Önemsiz bulgu' olarak not et.

            KURAL 2: SEMPTOM - BULGU İLİŞKİSİ (Diagnosis Logic)
            - İlişkiyi Doğru Kur: Laboratuvar sonuçlarını şikayetle zorla eşleştirme.
            - ÖRNEK SENARYO: Hasta 'Nefes Darlığı' şikayetiyle gelirse ve Kalp/Akciğer temizse; kan testindeki 'D Vitamini Eksikliği'ni nefes darlığının ANA SEBEBİ yapma. D Vitamini 'Yorgunluk' yapar, nefes darlığı yapmaz.
            - Bu durumda (Kalp/Ciğer temizse) 'Reflü (GERD)', 'Kondisyon Eksikliği' veya 'Kilo Fazlalığı' gibi mekanik sebepleri 'Yüksek Olasılık' olarak en başa koy. D vitamini eksikliğini sadece 'Katkıda bulunan yan faktör' olarak belirt.

            KURAL 3: HİYERARŞİ
            - Önce hayati organları (Kalp, Akciğer) değerlendir (Kullanıcı temiz dediyse ele).
            - Sonra mekanik/sistemik sebepleri (Reflü, Kilo, Anemi) değerlendir.
            - En son vitamin/mineral eksikliklerini değerlendir.

            ANALİZ EDİLECEK HASTA VERİSİ (JSON):
            {patientJson}

            İSTENEN JSON FORMATI:
            {{
               ""Olasiliklar"": [
                 {{ ""HastalikAdi"": ""..."", ""OlasilikSeviyesi"": ""..."", ""NedenBuTahmin"": ""..."", ""OnerilenBolum"": ""..."", ""AciliyetVarMi"": true/false }}
               ],
               ""LabUyarilari"": [ 
                 {{ ""Parametre"": ""..."", ""Durum"": ""..."", ""Aciklama"": ""..."" }}
               ],
               ""OzetDegerlendirme"": ""...""
            }}
            ";

            // --- YENİ EKLENEN KISIM BAŞLANGIÇ: RESİM VE METİN BİRLEŞTİRME ---

            // Google'a göndereceğimiz parçaları tutacak dinamik bir liste oluşturuyoruz
            var partsList = new List<object>();

            // 1. Parça: Metin (Bizim Prompt)
            partsList.Add(new { text = fullPrompt });

            // 2. Parça: Resim (Eğer kullanıcı yüklemişse)
            if (!string.IsNullOrEmpty(request.LabResmiBase64) && !string.IsNullOrEmpty(request.LabResmiMimeType))
            {
                partsList.Add(new
                {
                    inline_data = new
                    {
                        mime_type = request.LabResmiMimeType, // Örn: "image/jpeg"
                        data = request.LabResmiBase64         // Resmin Base64 kodu
                    }
                });
            }

            // --- YENİ EKLENEN KISIM BİTİŞ ---

            // 3. Google Gemini Request Gövdesi
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = partsList // Hazırladığımız listeyi buraya veriyoruz
                    }
                },
                generationConfig = new
                {
                    response_mime_type = "application/json" // Gemini'yi JSON dönmeye zorlar
                }
            };

            // 4. İsteği Gönder (Gemini 2.5 Flash Modelini kullanıyoruz - Senin hesabında çalışan versiyon)
            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            // URL Değişikliği: gemini-2.5-flash kullanıldı
            var response = await _httpClient.PostAsync($"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={apiKey}", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API Hatası: {response.StatusCode} - {errorContent}");
            }

            // 5. Cevabı İşle
            var responseString = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseString);

            try
            {
                // Google'ın iç içe geçmiş JSON yapısından cevabı çıkar
                string aiResponseText = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                // Gelen metni bizim C# AnalysisResult sınıfına dönüştür
                var resultOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<AnalysisResult>(aiResponseText, resultOptions) ?? new AnalysisResult();
            }
            catch
            {
                throw new Exception("Yapay zeka yanıtı işlenirken hata oluştu veya JSON formatı bozuk.");
            }
        }
    }
}