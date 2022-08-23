using System;
namespace Test_Search_Api.V1.Interfaces.Factories
{
    public interface IQueryFactory
    {
        IQueryGenerator<T> CreateQuery<T>() where T : class;
    }
}

