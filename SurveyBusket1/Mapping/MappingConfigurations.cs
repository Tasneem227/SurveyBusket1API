using Mapster;
//using SurveyBusket8.Contracts.Responses;


namespace SurveyBusket8.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //    config.NewConfig<Student, StudentResponse>()
        //        .Map(dest => dest.FullName, src => $"{src.FirstName} {src.MiddleName} {src.LastName}")
        //        .Map(dest => dest.age, src => DateTime.Now.Year - src.dateofbirth.Value.Year)
        //        .Map(dest => dest.DepartmentName, src => src.Department.Name);
        
    }
}