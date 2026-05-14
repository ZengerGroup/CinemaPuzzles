using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaPuzzles
{
    internal class Address
    {
        public string Name;
        public string Company;
        public string Address1;
        public string Address2;
        public string City;
        public string State;
        public string Zip;
        public string Country;

        public Address(string name, string company, string address1, string address2, string city, string state, string zip, string country)
        {
            Name = name;
            Company = company;
            Address1 = address1;
            Address2 = address2;
            City = city;
            State = state;
            Zip = zip;
            Country = country;
        }

        public string[] GenerateAddressBlock()
        {
            List<string> block = new List<string>();
            if(Name.Length > 0) block.Add(Name);
            if(Company.Length > 0) block.Add(Company);
            if(Address2.Length > 0) block.Add(Address2);
            if(Address1.Length > 0) block.Add(Address1);
            if(City.Length > 0 && State.Length > 0 && Zip.Length > 0) block.Add(String.Format("{0}, {1} {2}", City, State, Zip));
            if(Country != "United States" && Country.Length > 0) block.Add(Country);
            return block.ToArray();
        }
    }
}
