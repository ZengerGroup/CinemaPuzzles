using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPuzzles
{
    internal class Address
    {
        public string Name;
        public string Street;
        public string City;
        public string State;
        public string Zip;
        public string Country;

        public Address(string name, string street, string city, string state, string zip, string country)
        {
            Name = name;
            Street = street;
            City = city;
            State = state;
            Zip = zip;
            Country = country;
        }

        public string[] GenerateAddressBlock()
        {
            return [
                Name,
                Street,
                String.Format("{0} {1} {2}", City, State, Zip)
            ];
        }
    }
}
