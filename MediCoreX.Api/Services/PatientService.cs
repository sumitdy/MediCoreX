using AutoMapper;
using MediCoreX.Api.Data;
using MediCoreX.Api.DTOs;
using MediCoreX.Api.Exceptions;
using MediCoreX.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MediCoreX.Api.Services
{
    public class PatientService : IPatientService
    {
        private readonly MediCoreXDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<PatientService> _logger;

        public PatientService(
            MediCoreXDbContext context,
            IMapper mapper,
            ILogger<PatientService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // 🔹 Get all patients
        public async Task<List<PatientDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all patients");

            var patients = await _context.Patients.ToListAsync();

            return _mapper.Map<List<PatientDto>>(patients);
        }

        // 🔹 Get patient by Id
        public async Task<PatientDto> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching patient with Id: {Id}", id);

            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                _logger.LogWarning("Patient not found with Id: {Id}", id);
                throw new NotFoundException("Patient not found");
            }

            return _mapper.Map<PatientDto>(patient);
        }

        // 🔹 Get patients above given age
        public async Task<List<PatientDto>> GetAboveAgeAsync(int age)
        {
            _logger.LogInformation("Fetching patients above age: {Age}", age);

            var patients = await _context.Patients
                .Where(p => p.Age > age)
                .ToListAsync();

            return _mapper.Map<List<PatientDto>>(patients);
        }

        // 🔹 Get patients by gender
        public async Task<List<PatientDto>> GetByGenderAsync(string gender)
        {
            _logger.LogInformation("Fetching patients with gender: {Gender}", gender);

            var patients = await _context.Patients
                .Where(p => p.Gender == gender)
                .ToListAsync();

            return _mapper.Map<List<PatientDto>>(patients);
        }

        // 🔹 Search patients by name
        public async Task<List<PatientDto>> SearchByNameAsync(string name)
        {
            _logger.LogInformation("Searching patients by name: {Name}", name);

            var patients = await _context.Patients
                .Where(p => p.FullName.Contains(name))
                .ToListAsync();

            return _mapper.Map<List<PatientDto>>(patients);
        }

        // 🔹 Sort patients by age
        public async Task<List<PatientDto>> GetSortedByAgeAsync(bool ascending)
        {
            _logger.LogInformation("Sorting patients by age. Ascending: {Asc}", ascending);

            var query = ascending
                ? _context.Patients.OrderBy(p => p.Age)
                : _context.Patients.OrderByDescending(p => p.Age);

            var patients = await query.ToListAsync();

            return _mapper.Map<List<PatientDto>>(patients);
        }

        // 🔹 PAGINATION (FINAL & CORRECT)
        public async Task<PagedResultDto<PatientDto>> GetPagedAsync(int page, int pageSize)
        {
            if (page <= 0 || pageSize <= 0)
            {
                _logger.LogWarning(
                    "Invalid pagination values. Page: {Page}, PageSize: {PageSize}",
                    page, pageSize);

                throw new BadRequestException(
                    "Page and PageSize must be greater than zero");
            }

            _logger.LogInformation(
                "Fetching paged patients. Page: {Page}, PageSize: {PageSize}",
                page, pageSize);

            var totalRecords = await _context.Patients.CountAsync();

            var patients = await _context.Patients
                .OrderBy(p => p.Id)          // ⚠️ Always order before Skip
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResultDto<PatientDto>
            {
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                Data = _mapper.Map<List<PatientDto>>(patients)
            };
        }

        // 🔹 Add new patient (Admin only)
        public async Task<PatientDto> AddAsync(CreatePatientDto dto)
        {
            _logger.LogInformation("Adding new patient: {Name}", dto.FullName);

            var patient = new Patient
            {
                FullName = dto.FullName.Trim(),
                Age = dto.Age,
                Gender = dto.Gender.Trim()
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Patient added successfully with Id: {Id}", patient.Id);

            return _mapper.Map<PatientDto>(patient);
        }

        // 🔹 Update patient (Admin only)
        public async Task<PatientDto> UpdateAsync(int id, UpdatePatientDto dto)
        {
            _logger.LogInformation("Updating patient with Id: {Id}", id);

            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                _logger.LogWarning("Update failed. Patient not found with Id: {Id}", id);
                throw new NotFoundException("Patient not found");
            }

            patient.FullName = dto.FullName.Trim();
            patient.Age = dto.Age;
            patient.Gender = dto.Gender.Trim();

            await _context.SaveChangesAsync();

            _logger.LogInformation("Patient updated successfully with Id: {Id}", id);

            return _mapper.Map<PatientDto>(patient);
        }

        // 🔹 Delete patient (Admin only)
        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogWarning("Attempting to delete patient with Id: {Id}", id);

            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                _logger.LogWarning(
                    "Delete failed. Patient not found with Id: {Id}", id);

                throw new NotFoundException("Patient not found");
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            _logger.LogInformation(
                "Patient deleted successfully with Id: {Id}", id);

            return true;
        }
    }
}
