using System;
namespace Test_Search_Api.V1.Interfaces
{
    public interface IPagingHelper
    {
        int GetPageOffset(int pageSize, int currentPage);
    }
}

