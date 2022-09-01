using Hackney.Shared.Asset.Domain;
using Nest;
using Test_Search_Api.V1.Domain;

namespace Test_Search_Api.V1.Gateways
{
    public class QueryableAsset
    {
        public DomainAsset Create()
        {
            return DomainAsset.Create(Title, FirstName, LastName, DOB, AssetType,NumberOfBedrooms);
        }

        [Text(Name = "id")]
        public string Id { get; set; }

        [Text(Name = "title")]
        public string Title { get; set; }

        [Text(Name = "firstName")]
        public string FirstName { get; set; }

        [Text(Name = "lastName")]
        public string LastName { get; set; }

        [Text(Name = "dob")]
        public string DOB { get; set; }

        [Text(Name = "assetType")]
        public string AssetType { get; set; }

        [Text(Name = "numberOfBedrooms")]
        public int NumberOfBedrooms { get; set; }
    }
}