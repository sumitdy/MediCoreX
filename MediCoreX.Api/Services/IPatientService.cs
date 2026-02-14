using MediCoreX.Api.DTOs;

namespace MediCoreX.Api.Services
{
    public interface IPatientService
    {
        Task<List<PatientDto>> GetAllAsync();
        Task<PatientDto> GetByIdAsync(int id);

        Task<List<PatientDto>> GetAboveAgeAsync(int age);
        Task<List<PatientDto>> GetByGenderAsync(string gender);
        Task<List<PatientDto>> SearchByNameAsync(string name);
        Task<List<PatientDto>> GetSortedByAgeAsync(bool ascending);

        // ✅ ONLY THIS pagination
        Task<PagedResultDto<PatientDto>> GetPagedAsync(int page, int pageSize);

        Task<PatientDto> AddAsync(PatientDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
