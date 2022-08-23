using Nest;

namespace Test_Search_Api.V1.Interfaces.Filtering
{
    public interface IFilter<T> where T : class
    {
        QueryContainerDescriptor<T> GetDescriptor<TRequest>(QueryContainerDescriptor<T> descriptor, TRequest request);
    }
}

