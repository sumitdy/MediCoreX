using AutoMapper;
using MediCoreX.Api.Data;
using MediCoreX.Api.DTOs;
using MediCoreX.Api.Models;
using MediCoreX.Api.Services;
using MediCoreX.Api.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace MediCoreX.Tests;

public class PatientServiceTests
{
    private readonly MediCoreXDbContext _context;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<PatientService>> _mockLogger;

    public PatientServiceTests()
    {
        var options = new DbContextOptionsBuilder<MediCoreXDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new MediCoreXDbContext(options);

        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<PatientService>>();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingPatient_ReturnsPatient()
    {
        // Arrange

        var patient = new Patient
        {
            Id = 1,
            FullName = "Rahul Sharma",
            Age = 30,
            Gender = "Male"
        };

        _context.Patients.Add(patient);

        await _context.SaveChangesAsync();

        var patientDto = new PatientDto
        {
            Id = 1,
            FullName = "Rahul Sharma",
            Age = 30,
            Gender = "Male"
        };

        _mockMapper
            .Setup(x => x.Map<PatientDto>(It.IsAny<Patient>()))
            .Returns(patientDto);

        var service = new PatientService(
            _context,
            _mockMapper.Object,
            _mockLogger.Object
        );

        // Act

        var result = await service.GetByIdAsync(1);

        // Assert

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Rahul Sharma", result.FullName);
        Assert.Equal(30, result.Age);
        Assert.Equal("Male", result.Gender);
    }
     
     [Fact]
public async Task GetByIdAsync_PatientNotFound_ThrowsNotFoundException()
{
    // Arrange

    var service = new PatientService(
        _context,
        _mockMapper.Object,
        _mockLogger.Object
    );

    // Act

    var exception = await Assert.ThrowsAsync<NotFoundException>(
        () => service.GetByIdAsync(999)
    );

    // Assert

    Assert.Equal(
        "Patient not found",
        exception.Message
    );
}
    [Fact]
public async Task GetAllAsync_ExistingPatients_ReturnsAllPatients()
{
    // Arrange

    var patients = new List<Patient>
    {
        new Patient
        {
            Id = 1,
            FullName = "Rahul Sharma",
            Age = 30,
            Gender = "Male"
        },
        new Patient
        {
            Id = 2,
            FullName = "Priya Singh",
            Age = 25,
            Gender = "Female"
        }
    };

    _context.Patients.AddRange(patients);

    await _context.SaveChangesAsync();

    var patientDtos = new List<PatientDto>
    {
        new PatientDto
        {
            Id = 1,
            FullName = "Rahul Sharma",
            Age = 30,
            Gender = "Male"
        },
        new PatientDto
        {
            Id = 2,
            FullName = "Priya Singh",
            Age = 25,
            Gender = "Female"
        }
    };

    _mockMapper
        .Setup(x => x.Map<List<PatientDto>>(It.IsAny<List<Patient>>()))
        .Returns(patientDtos);

    var service = new PatientService(
        _context,
        _mockMapper.Object,
        _mockLogger.Object
    );

    // Act

    var result = await service.GetAllAsync();

    // Assert

    Assert.NotNull(result);
    Assert.Equal(2, result.Count);

    Assert.Equal("Rahul Sharma", result[0].FullName);
    Assert.Equal("Priya Singh", result[1].FullName);
}
  
   [Fact]
public async Task GetAboveAgeAsync_ReturnsPatientsAboveGivenAge()
{
    // Arrange

    var patients = new List<Patient>
    {
        new Patient
        {
            Id = 1,
            FullName = "Rahul Sharma",
            Age = 30,
            Gender = "Male"
        },
        new Patient
        {
            Id = 2,
            FullName = "Priya Singh",
            Age = 20,
            Gender = "Female"
        },
        new Patient
        {
            Id = 3,
            FullName = "Amit Kumar",
            Age = 40,
            Gender = "Male"
        }
    };

    _context.Patients.AddRange(patients);

    await _context.SaveChangesAsync();

    var patientDtos = new List<PatientDto>
    {
        new PatientDto
        {
            Id = 1,
            FullName = "Rahul Sharma",
            Age = 30,
            Gender = "Male"
        },
        new PatientDto
        {
            Id = 3,
            FullName = "Amit Kumar",
            Age = 40,
            Gender = "Male"
        }
    };

    _mockMapper
        .Setup(x => x.Map<List<PatientDto>>(It.IsAny<List<Patient>>()))
        .Returns(patientDtos);

    var service = new PatientService(
        _context,
        _mockMapper.Object,
        _mockLogger.Object
    );

    // Act

    var result = await service.GetAboveAgeAsync(25);

    // Assert

    Assert.NotNull(result);
    Assert.Equal(2, result.Count);

    Assert.All(result, patient =>
        Assert.True(patient.Age > 25)
    );
}

[Fact]
public async Task GetByGenderAsync_ReturnsPatientsWithMatchingGender()
{
    // Arrange

    var patients = new List<Patient>
    {
        new Patient
        {
            Id = 1,
            FullName = "Rahul Sharma",
            Age = 30,
            Gender = "Male"
        },
        new Patient
        {
            Id = 2,
            FullName = "Priya Singh",
            Age = 25,
            Gender = "Female"
        },
        new Patient
        {
            Id = 3,
            FullName = "Amit Kumar",
            Age = 40,
            Gender = "Male"
        }
    };

    _context.Patients.AddRange(patients);

    await _context.SaveChangesAsync();

    var patientDtos = new List<PatientDto>
    {
        new PatientDto
        {
            Id = 1,
            FullName = "Rahul Sharma",
            Age = 30,
            Gender = "Male"
        },
        new PatientDto
        {
            Id = 3,
            FullName = "Amit Kumar",
            Age = 40,
            Gender = "Male"
        }
    };

    _mockMapper
        .Setup(x => x.Map<List<PatientDto>>(It.IsAny<List<Patient>>()))
        .Returns(patientDtos);

    var service = new PatientService(
        _context,
        _mockMapper.Object,
        _mockLogger.Object
    );

    // Act

    var result = await service.GetByGenderAsync("Male");

    // Assert

    Assert.NotNull(result);
    Assert.Equal(2, result.Count);

    Assert.All(result, patient =>
        Assert.Equal("Male", patient.Gender)
    );
}

[Fact]
public async Task SearchByNameAsync_MatchingName_ReturnsPatients()
{
    // Arrange

    var patients = new List<Patient>
    {
        new Patient
        {
            Id = 1,
            FullName = "Rahul Sharma",
            Age = 30,
            Gender = "Male"
        },
        new Patient
        {
            Id = 2,
            FullName = "Rakesh Kumar",
            Age = 35,
            Gender = "Male"
        },
        new Patient
        {
            Id = 3,
            FullName = "Priya Singh",
            Age = 25,
            Gender = "Female"
        }
    };

    _context.Patients.AddRange(patients);

    await _context.SaveChangesAsync();

    var patientDtos = new List<PatientDto>
    {
        new PatientDto
        {
            Id = 1,
            FullName = "Rahul Sharma",
            Age = 30,
            Gender = "Male"
        }
        
    };

    _mockMapper
        .Setup(x => x.Map<List<PatientDto>>(It.IsAny<List<Patient>>()))
        .Returns(patientDtos);

    var service = new PatientService(
        _context,
        _mockMapper.Object,
        _mockLogger.Object
    );

    // Act

    var result = await service.SearchByNameAsync("Rah");

    // Assert

    Assert.NotNull(result);
    Assert.Single(result);

    Assert.Equal(
        "Rahul Sharma",
        result[0].FullName
    );
}

[Fact]
public async Task GetSortedByAgeAsync_Ascending_ReturnsPatientsSortedByAge()
{
    // Arrange

    var patients = new List<Patient>
    {
        new Patient
        {
            Id = 1,
            FullName = "Rahul Sharma",
            Age = 40,
            Gender = "Male"
        },
        new Patient
        {
            Id = 2,
            FullName = "Priya Singh",
            Age = 25,
            Gender = "Female"
        },
        new Patient
        {
            Id = 3,
            FullName = "Amit Kumar",
            Age = 30,
            Gender = "Male"
        }
    };

    _context.Patients.AddRange(patients);

    await _context.SaveChangesAsync();

    var patientDtos = new List<PatientDto>
    {
        new PatientDto
        {
            Id = 2,
            FullName = "Priya Singh",
            Age = 25,
            Gender = "Female"
        },
        new PatientDto
        {
            Id = 3,
            FullName = "Amit Kumar",
            Age = 30,
            Gender = "Male"
        },
        new PatientDto
        {
            Id = 1,
            FullName = "Rahul Sharma",
            Age = 40,
            Gender = "Male"
        }
    };

    _mockMapper
        .Setup(x => x.Map<List<PatientDto>>(It.IsAny<List<Patient>>()))
        .Returns(patientDtos);

    var service = new PatientService(
        _context,
        _mockMapper.Object,
        _mockLogger.Object
    );

    // Act

    var result = await service.GetSortedByAgeAsync(true);

    // Assert

    Assert.NotNull(result);
    Assert.Equal(3, result.Count);

    Assert.Equal(25, result[0].Age);
    Assert.Equal(30, result[1].Age);
    Assert.Equal(40, result[2].Age);
}

[Fact]
public async Task GetSortedByAgeAsync_Descending_ReturnsPatientsSortedByAge()
{
    // Arrange

    var patients = new List<Patient>
    {
        new Patient
        {
            Id = 1,
            FullName = "Rahul Sharma",
            Age = 40,
            Gender = "Male"
        },
        new Patient
        {
            Id = 2,
            FullName = "Priya Singh",
            Age = 25,
            Gender = "Female"
        },
        new Patient
        {
            Id = 3,
            FullName = "Amit Kumar",
            Age = 30,
            Gender = "Male"
        }
    };

    _context.Patients.AddRange(patients);

    await _context.SaveChangesAsync();

    var patientDtos = new List<PatientDto>
    {
        new PatientDto
        {
            Id = 1,
            FullName = "Rahul Sharma",
            Age = 40,
            Gender = "Male"
        },
        new PatientDto
        {
            Id = 3,
            FullName = "Amit Kumar",
            Age = 30,
            Gender = "Male"
        },
        new PatientDto
        {
            Id = 2,
            FullName = "Priya Singh",
            Age = 25,
            Gender = "Female"
        }
    };

    _mockMapper
        .Setup(x => x.Map<List<PatientDto>>(It.IsAny<List<Patient>>()))
        .Returns(patientDtos);

    var service = new PatientService(
        _context,
        _mockMapper.Object,
        _mockLogger.Object
    );

    // Act

    var result = await service.GetSortedByAgeAsync(false);

    // Assert

    Assert.NotNull(result);
    Assert.Equal(3, result.Count);

    Assert.Equal(40, result[0].Age);
    Assert.Equal(30, result[1].Age);
    Assert.Equal(25, result[2].Age);
}

[Fact]
public async Task GetPagedAsync_ValidPage_ReturnsCorrectPage()
{
    // Arrange

    var patients = new List<Patient>
    {
        new Patient
        {
            Id = 1,
            FullName = "Patient One",
            Age = 20,
            Gender = "Male"
        },
        new Patient
        {
            Id = 2,
            FullName = "Patient Two",
            Age = 25,
            Gender = "Female"
        },
        new Patient
        {
            Id = 3,
            FullName = "Patient Three",
            Age = 30,
            Gender = "Male"
        },
        new Patient
        {
            Id = 4,
            FullName = "Patient Four",
            Age = 35,
            Gender = "Female"
        },
        new Patient
        {
            Id = 5,
            FullName = "Patient Five",
            Age = 40,
            Gender = "Male"
        }
    };

    _context.Patients.AddRange(patients);

    await _context.SaveChangesAsync();

    var patientDtos = new List<PatientDto>
    {
        new PatientDto
        {
            Id = 3,
            FullName = "Patient Three",
            Age = 30,
            Gender = "Male"
        },
        new PatientDto
        {
            Id = 4,
            FullName = "Patient Four",
            Age = 35,
            Gender = "Female"
        }
    };

    _mockMapper
        .Setup(x => x.Map<List<PatientDto>>(It.IsAny<List<Patient>>()))
        .Returns(patientDtos);

    var service = new PatientService(
        _context,
        _mockMapper.Object,
        _mockLogger.Object
    );

    // Act

    var result = await service.GetPagedAsync(2, 2);

    // Assert

    Assert.NotNull(result);

    Assert.Equal(2, result.Page);
    Assert.Equal(2, result.PageSize);
    Assert.Equal(5, result.TotalRecords);
    Assert.Equal(3, result.TotalPages);

    Assert.Equal(2, result.Data.Count);

    Assert.Equal(3, result.Data[0].Id);
    Assert.Equal(4, result.Data[1].Id);
}

[Fact]
public async Task GetPagedAsync_InvalidPage_ThrowsBadRequestException()
{
    // Arrange

    var service = new PatientService(
        _context,
        _mockMapper.Object,
        _mockLogger.Object
    );

    // Act

    var exception = await Assert.ThrowsAsync<BadRequestException>(
        () => service.GetPagedAsync(0, 10)
    );

    // Assert

    Assert.Equal(
        "Page and PageSize must be greater than zero",
        exception.Message
    );
}

[Fact]
public async Task AddAsync_ValidPatient_CreatesPatient()
{
    // Arrange

    var createDto = new CreatePatientDto
    {
        FullName = "  Rahul Sharma  ",
        Age = 30,
        Gender = "  Male  "
    };

    var patientDto = new PatientDto
    {
        Id = 1,
        FullName = "Rahul Sharma",
        Age = 30,
        Gender = "Male"
    };

    _mockMapper
        .Setup(x => x.Map<PatientDto>(It.IsAny<Patient>()))
        .Returns(patientDto);

    var service = new PatientService(
        _context,
        _mockMapper.Object,
        _mockLogger.Object
    );

    // Act

    var result = await service.AddAsync(createDto);

    // Assert

    Assert.NotNull(result);

    Assert.Equal("Rahul Sharma", result.FullName);
    Assert.Equal(30, result.Age);
    Assert.Equal("Male", result.Gender);

    var savedPatient = await _context.Patients
        .FirstOrDefaultAsync();

    Assert.NotNull(savedPatient);

    Assert.Equal("Rahul Sharma", savedPatient.FullName);
    Assert.Equal(30, savedPatient.Age);
    Assert.Equal("Male", savedPatient.Gender);
}

[Fact]
public async Task UpdateAsync_ExistingPatient_UpdatesPatient()
{
    // Arrange

    var patient = new Patient
    {
        Id = 1,
        FullName = "Rahul Sharma",
        Age = 30,
        Gender = "Male"
    };

    _context.Patients.Add(patient);

    await _context.SaveChangesAsync();

    var updateDto = new UpdatePatientDto
    {
        FullName = "  Rahul Kumar  ",
        Age = 35,
        Gender = "  Male  "
    };

    var updatedPatientDto = new PatientDto
    {
        Id = 1,
        FullName = "Rahul Kumar",
        Age = 35,
        Gender = "Male"
    };

    _mockMapper
        .Setup(x => x.Map<PatientDto>(It.IsAny<Patient>()))
        .Returns(updatedPatientDto);

    var service = new PatientService(
        _context,
        _mockMapper.Object,
        _mockLogger.Object
    );

    // Act

    var result = await service.UpdateAsync(1, updateDto);

    // Assert

    Assert.NotNull(result);

    Assert.Equal(1, result.Id);
    Assert.Equal("Rahul Kumar", result.FullName);
    Assert.Equal(35, result.Age);
    Assert.Equal("Male", result.Gender);

    var updatedPatient = await _context.Patients
        .FirstAsync(p => p.Id == 1);

    Assert.Equal("Rahul Kumar", updatedPatient.FullName);
    Assert.Equal(35, updatedPatient.Age);
    Assert.Equal("Male", updatedPatient.Gender);
}

[Fact]
public async Task UpdateAsync_PatientNotFound_ThrowsNotFoundException()
{
    // Arrange

    var service = new PatientService(
        _context,
        _mockMapper.Object,
        _mockLogger.Object
    );

    var updateDto = new UpdatePatientDto
    {
        FullName = "Updated Patient",
        Age = 35,
        Gender = "Male"
    };

    // Act

    var exception = await Assert.ThrowsAsync<NotFoundException>(
        () => service.UpdateAsync(999, updateDto)
    );

    // Assert

    Assert.Equal(
        "Patient not found",
        exception.Message
    );
}

[Fact]
public async Task DeleteAsync_ExistingPatient_DeletesPatient()
{
    // Arrange

    var patient = new Patient
    {
        Id = 1,
        FullName = "Rahul Sharma",
        Age = 30,
        Gender = "Male"
    };

    _context.Patients.Add(patient);

    await _context.SaveChangesAsync();

    var service = new PatientService(
        _context,
        _mockMapper.Object,
        _mockLogger.Object
    );

    // Act

    var result = await service.DeleteAsync(1);

    // Assert

    Assert.True(result);

    var deletedPatient = await _context.Patients
        .FirstOrDefaultAsync(p => p.Id == 1);

    Assert.Null(deletedPatient);
}

[Fact]
public async Task DeleteAsync_PatientNotFound_ThrowsNotFoundException()
{
    // Arrange

    var service = new PatientService(
        _context,
        _mockMapper.Object,
        _mockLogger.Object
    );

    // Act

    var exception = await Assert.ThrowsAsync<NotFoundException>(
        () => service.DeleteAsync(999)
    );

    // Assert

    Assert.Equal(
        "Patient not found",
        exception.Message
    );
}

}