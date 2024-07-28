namespace HealthCare.Mapping
{
    using AutoMapper;
    using HealthCare.Models;
    using HealthCare.Dtos;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map Department to DepartmentDTO
            CreateMap<Department, DepartmentDTO>().ReverseMap();

            // Map Doctor to DoctorDTO
            CreateMap<Doctor, DoctorDTO>().ReverseMap();

            // Map Patient to PatientDTO
            CreateMap<Patient, PatientDTO>().ReverseMap();

            // Map Test to TestDTO
            CreateMap<Test, TestDTO>().ReverseMap();

            // Map Visit to VisitDTO
            CreateMap<Visit, VisitDTO>().ReverseMap();

            // Map AppUser to AppUserDTO
            CreateMap<AppUser, AppUserDTO>().ReverseMap();
        }
    }

}
