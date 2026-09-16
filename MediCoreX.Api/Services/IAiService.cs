namespace MediCoreX.Api.Services
{
    public interface IAiService
    {
        Task<string> GeneratePatientSummaryAsync(
            string fullName,
            int age,
            string gender);
    }
}