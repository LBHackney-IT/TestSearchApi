using System.Threading.Tasks;
using HousingSearchApi.V1.Boundary.Requests;
using Nest;


namespace Test_Search_Api.V1.Interfaces
{
    public interface IElasticSearchWrapper
    {
        Task<ISearchResponse<T>> Search<T, TRequest>(TRequest request) where T : class where TRequest : class;

        Task<ISearchResponse<T>> SearchSets<T, TRequest>(TRequest request) where T : class where TRequest : class;
    }
}