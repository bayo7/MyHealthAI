using MyHealthAI.Services;

var builder = WebApplication.CreateBuilder(args);

// --- SERVÝSLER ---

// Hem MVC (View) hem de API Controller özelliklerini ekliyoruz
builder.Services.AddControllersWithViews();

// Swagger Servislerini Ekle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Senin AI Servisin (Gemini)
builder.Services.AddHttpClient<IAIService, GeminiHealthService>();

var app = builder.Build();

// --- MIDDLEWARE (Uygulama Hattý) ---

// Swagger'ý "Development" kontrolü dýþýna çýkaralým ki her türlü açýlsýn (Test için)
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles(); // wwwroot klasörü için (CSS/JS)

app.UseRouting();

app.UseAuthorization();

// --- YÖNLENDÝRMELER (Routing) ---

// 1. API Controller'larý için (Bizim AnalysisController buna ihtiyaç duyar)
app.MapControllers();

// 2. Senin görseldeki MVC rotasý (Home/Index için)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();