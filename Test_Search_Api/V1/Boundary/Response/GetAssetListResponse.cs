using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;

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
