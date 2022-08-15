using Test_Search_Api.V1.Boundary.Request;
using Test_Search_Api.V1.Boundary.Response;
using System.Threading.Tasks;
// using Test_Search_Api.V1.Boundary.Responses.Transactions;

namespace Test_Search_Api.V1.Gateways.Interfaces
{
    public interface ISearchGateway
    {
        Task<GetAssetListResponse> GetListOfAssets(GetAssetListRequest query);
        // Task<GetAllAssetListResponse> GetListOfAssetsSets(GetAllAssetListRequest query);
    }
}