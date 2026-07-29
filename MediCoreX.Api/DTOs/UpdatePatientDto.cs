namespace MediCoreX.Api.DTOs;

public class UpdatePatientDto
{
    public string FullName { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Gender { get; set; } = string.Empty;
}
