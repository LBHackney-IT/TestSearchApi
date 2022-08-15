using System.Collections.Generic;
//using Hackney.Shared.HousingSearch.Domain.Asset;


// TODO: how to solve using issue above?
namespace Test_Search_Api.V1.Boundary.Response
{
    public class GetAssetListResponse
    {
        private long _total;

        public List<Asset> Assets { get; set; }

        public GetAssetListResponse()
        {
            Assets = new List<Asset>();
        }

        public void SetTotal(long total)
        {
            _total = total < 0 ? 0 : total;
        }

        public long Total()
        {
            return _total;
        }
    }
}
