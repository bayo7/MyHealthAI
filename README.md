# MyHealthAI - Yapay Zeka Destekli Tıbbi Ön Değerlendirme Asistanı

Bu proje, .NET 8.0 ve Google Gemini 2.5 Flash modeli kullanılarak geliştirilmiş bir yapay zeka sağlık asistanıdır. Kullanıcıların semptomlarını ve laboratuvar sonuçlarını (görsel veya metin) analiz ederek olası durumları listeler ve ilgili tıbbi bölüme yönlendirme yapar.

**⚠️ Uyarı:** Bu proje sadece bilgilendirme ve eğitim amaçlıdır. Kesinlikle tıbbi tanı yerine geçmez.

## 🚀 Özellikler
* **Semptom Analizi:** Kullanıcı şikayetlerini ve hikayesini analiz eder.
* **Lab Sonuçları Okuma (OCR):** Yüklenen tahlil fotoğraflarını (Vision API) okuyup yorumlar.
* **Akıllı Triage:** Hastayı Kardiyoloji, Dahiliye gibi doğru bölümlere yönlendirir.
* **Tıbbi Filtreleme:** Sadece referans dışı değerleri yorumlar ve semptom-bulgu ilişkisi kurar.

## 🛠 Teknolojiler
* **Backend:** ASP.NET Core 8.0 Web API
* **AI Model:** Google Gemini 2.5 Flash (via Google AI Studio)
* **Frontend:** HTML5, Bootstrap 5, JavaScript (Fetch API)
* **Architecture:** Service-Repository Pattern (Simplified), Dependency Injection

## 🔮 Gelecek Planları (Roadmap)
Bu proje aktif geliştirme aşamasındadır. Gelecek sürümler için planlanan özellikler:
- [ ] **Gelişmiş Prompt Mühendisliği:** Nadir hastalıklar için daha derinlemesine analiz senaryoları.
- [ ] **Kullanıcı Arayüzü (UI):** Bootstrap tasarımının modernleştirilmesi ve mobil uyumluluğun artırılması.
- [ ] **Kullanıcı Sistemi:** Geçmiş tahlillerin kaydedilmesi için üyelik sistemi (Identity).
- [ ] **Veri Güvenliği:** KVKK/GDPR uyumlu veri saklama politikaları.
- [ ] **Chat Modu:** Kullanıcının yapay zeka ile karşılıklı soru-cevap yapabileceği sohbet arayüzü.

## ⚙️ Kurulum
1. Repoyu klonlayın.
2. `appsettings.json` dosyasına kendi Google Gemini API anahtarınızı ekleyin.
3. Projeyi Visual Studio ile açıp `F5` ile başlatın.

---
*Geliştirici: [Senin Adın]*