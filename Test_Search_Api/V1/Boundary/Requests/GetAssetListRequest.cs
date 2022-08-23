using Microsoft.AspNetCore.Mvc;
using HousingSearchApi.V1.Boundary.Requests;

namespace Test_Search_Api.V1.Boundary.Requests
{
    public class GetAssetListRequest : HousingSearchRequest
    {
        [FromQuery(Name = "assetTypes")]
        public string AssetTypes { get; set; }

        [FromQuery(Name = "numberOfBedrooms")]
        public string NumberOfBedrooms { get; set; }
    }
}
