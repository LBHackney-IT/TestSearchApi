using Test_Search_Api.V1.Interfaces;

namespace Test_Search_Api.V1.Infrastructure
{
    public class PagingHelper : IPagingHelper
    {
        public int GetPageOffset(int pageSize, int currentPage)
        {
            return pageSize * (currentPage == 0 ? 0 : currentPage - 1);
        }
    }
}

