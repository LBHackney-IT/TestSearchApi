using Microsoft.AspNetCore.Mvc;

namespace Test_Search_Api.V1.Boundary.Request
{
    public class GetAssetListRequest : HousingSearchRequest
    {
        [FromQuery(Name = "assetTypes")]
        public string AssetTypes { get; set; }

        [FromQuery(Name = "numberOfBedrooms")]
        public string NumberOfBedrooms { get; set; }
    }
}