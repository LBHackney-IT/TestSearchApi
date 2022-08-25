using Nest;

namespace Test_Search_Api.V1.Interfaces.Sorting
{
    public interface ISort<T> where T : class
    {
        SortDescriptor<T> GetSortDescriptor(SortDescriptor<T> descriptor);
    }
}
