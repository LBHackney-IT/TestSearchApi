using System.Threading.Tasks;
using Test_Search_Api.V1.Boundary.Request;
using Test_Search_Api.V1.Boundary.Response;

namespace Test_Search_Api.V1.UseCase.Interfaces
{
    public interface IGetAssetListUseCase
    {
        Task<GetAssetListResponse> ExecuteAsync(GetAssetListRequest getPersonListRequest);
    }
}