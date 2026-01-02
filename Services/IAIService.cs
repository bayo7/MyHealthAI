using MyHealthAI.Models;

namespace MyHealthAI.Services
{
    public interface IAIService
    {
        Task<AnalysisResult> AnalyzePatientAsync(PatientAnalysisRequest request);
    }
}
