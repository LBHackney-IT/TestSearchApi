using System.Collections.Generic;
using Test_Search_Api.V1.Domain;

namespace Test_Search_Api.V1.Gateways
{
    public interface IExampleGateway
    {
        Entity GetEntityById(int id);

        List<Entity> GetAll();
    }
}
