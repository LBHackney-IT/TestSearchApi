using System;
namespace Test_Search_Api.V1.Domain
{
    public class DomainAsset
    {
        public DomainAsset() { }

        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public string AssetType { get; set; }
        public int NumberOfBedrooms { get; set; }


        private DomainAsset(string title, string firstName, string lastName, string dob, string assetType, int numberOfBedrooms)
        {
            Title = title;
            FirstName = firstName;
            LastName = lastName;
            DOB = dob;
            AssetType = assetType;
            NumberOfBedrooms = numberOfBedrooms;
        }

        public static DomainAsset Create(string title, string firstName, string lastName, string dob, string assetType, int numberOfBedrooms)
        {
            return new DomainAsset(title, firstName, lastName, dob, assetType, numberOfBedrooms);
        }        
    }
}

