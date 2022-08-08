using Test_Search_Api.V1.Boundary.Response;

namespace Test_Search_Api.V1.UseCase.Interfaces
{
    public interface IGetByIdUseCase
    {
        ResponseObject Execute(int id);
    }
}
