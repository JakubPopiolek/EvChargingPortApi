using EvApplicationApi.Models;

namespace tests.TestTools.Doubles
{
    public class ApplicationDoubleFactory
    {
        public static ApplicationItem CreateApplicationItem()
        {
            return new ApplicationItem
            {
                FirstName = "testFirstName",
                LastName = "testLastName",
                Email = "testEmail",
                Vrn = "testVrn",
                Address = CreateAddressItem(),
            };
        }

        public static Address CreateAddressItem()
        {
            return new Address
            {
                Line1 = "testLine1",
                Line2 = "testLine2",
                City = "testCity",
                Province = "testProvince",
                Postcode = "testPostcode",
            };
        }
    }
}
