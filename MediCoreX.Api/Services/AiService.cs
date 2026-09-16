using Google.GenAI;
using Google.GenAI.Types;

namespace MediCoreX.Api.Services
{
    public class AiService : IAiService
    {
        private readonly Client _client;

        public AiService(IConfiguration configuration)
        {
            var apiKey = configuration["Gemini:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    "Gemini API key is not configured."
                );
            }

            _client = new Client(apiKey: apiKey);
        }

        public async Task<string> GeneratePatientSummaryAsync(
    string fullName,
    int age,
    string gender)
{
    var prompt = $"""
        Generate a short, professional patient record summary.

        Patient Name: {fullName}
        Age: {age}
        Gender: {gender}

        Important:
        - Summarize only the information provided.
        - Do not diagnose any medical condition.
        - Do not recommend treatment or medication.
        - Do not use Markdown.
        - Do not use asterisks, bullet points, headings, or special formatting.
        - Return only one concise plain-text paragraph.
        """;

    var response = await _client.Models.GenerateContentAsync(
        model: "gemini-3.6-flash",
        contents: prompt
    );

    var summary = response.Candidates?[0]
                       .Content?
                       .Parts?[0]
                       .Text
                   ?? "Unable to generate patient summary.";

    summary = summary
        .Replace("**", "")
        .Replace("*", "")
        .Replace("#", "")
        .Trim();

    return summary;
}
    }
}