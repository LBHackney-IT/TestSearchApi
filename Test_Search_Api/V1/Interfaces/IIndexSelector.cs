using Nest;

namespace Test_Search_Api.V1.Interfaces
{
    public interface IIndexSelector
    {
        Indices.ManyIndices Create<T>();
    }
}

