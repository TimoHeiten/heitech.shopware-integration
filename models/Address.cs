using System.Text.Json.Serialization;

namespace models
{
    public class Address
    {
        public int Id { get; set; }
        public int Customer { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Phone { get; set; }
        public string VatId { get; set; }
        public string AdditionalAddressLine1 { get; set; }
        public string AdditionalAddressLine2 { get; set; }
        public int Country { get; set; }
        public int State { get; set; }
        public object[] Attribute { get; set; }


        [JsonConstructor]
        public Address(int id,
                       int customer,
                       string company,
                       string department,
                       string salutation,
                       string firstName,
                       string lastName,
                       string street,
                       string zipCode,
                       string phone,
                       string vatId,
                       string additionalAddressLine1,
                       string additionalAddressLine2,
                       int country,
                       int state,
                       object[] attribute)

        => (Id, Customer, Company, Department, Salutation, FirstName, LastName, Street, ZipCode, Phone, VatId, AdditionalAddressLine1, AdditionalAddressLine2, Country, State, Attribute)
            = (id, customer, company, department, salutation, firstName, lastName, street, zipCode, phone, vatId, additionalAddressLine1, additionalAddressLine2, country, state, attribute);
    }
}
