using System;
using System.Collections.Generic;
using Test_Search_Api.V1.Boundary.Requests;


namespace Test_Search_Api.V1.Interfaces.Sorting
{
    public interface ISortFactory
    {
        ISort<T> Create<T, TRequest>(TRequest request) where T : class where TRequest : class;
    }
}

