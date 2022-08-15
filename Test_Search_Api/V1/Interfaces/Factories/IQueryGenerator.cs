using Nest;

namespace Test_Search_Api.V1.Interfaces.Factories
{
    public interface IQueryGenerator<T> where T : class
    {
        QueryContainer Create<TRequest>(TRequest request, QueryContainerDescriptor<T> q);
    }
}
