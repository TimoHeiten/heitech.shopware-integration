
using System;
using System.ComponentModel.DataAnnotations;

namespace ShopwareIntegration.Models
{
    ///<summary>
    /// Address Model for Shopware Integration
    ///</summary>
    public class Address : BaseModel
    {
        #region mandatory Properties
        // if null a new Entity will be created on POST/PUT
        public int? Id { get; set; }  = default!;
        [Required]
        public int Customer { get; set; }  = default!;
        [Required]
        public string Salutation { get; set; }  = default!;
        [Required]
        public string FirstName { get; set; }  = default!;
        [Required]
        public string LastName { get; set; }  = default!;
        [Required]
        public string Street { get; set; }  = default!;
        [Required]
        public string ZipCode { get; set; }  = default!;
        [Required]
        public string City { get; set; }  = default!;
        [Required]
        public int Country { get; set; }  = default!;
        #endregion

        #region  optional Properties
        public string? Company { get; set; }  = default!;
        public string? Department { get; set; }  = default!;
        public string? Phone { get; set; }  = default!;
        public string? VatId { get; set; }  = default!;
        public string? AdditionalAddressLine1 { get; set; }  = default!;
        public string? AdditionalAddressLine2 { get; set; }  = default!;
        public int State { get; set; }  = default!;
        public object?[] Attribute { get; set; }  = Array.Empty<object?>();
        #endregion

        public override string TableName => "s_user_addresses";

        ///<summary>
        /// Use the Constructor to set all Required Properties correctly
        ///</summary>
        public Address(int? id, int customer, string salutation, string firstName, string lastName, string street, string zipCode, string city, int country)
            => (Id, Customer, Salutation, FirstName, LastName, Street, ZipCode, City, Country) = (id, customer, salutation, firstName, lastName, street, zipCode, city, country);

        ///<summary>
        /// default ctor for serialization / deserialization
        ///</summary>
        public Address()
        { }
    }
}
