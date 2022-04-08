
namespace ShopwareIntegration.Models
{
    ///<summary>
    /// Address Model for Shopware Integration
    ///</summary>
    public class Address : BaseModel
    {
        public int? Id { get; set; }  = default!;// if null a new Entity will be created on POST/PUT
        public int Customer { get; set; }  = default!;
        public string Company { get; set; }  = default!;
        public string Department { get; set; }  = default!;
        public string Salutation { get; set; }  = default!;
        public string FirstName { get; set; }  = default!;
        public string LastName { get; set; }  = default!;
        public string Street { get; set; }  = default!;
        public string ZipCode { get; set; }  = default!;
        public string Phone { get; set; }  = default!;
        public string VatId { get; set; }  = default!;
        public string AdditionalAddressLine1 { get; set; }  = default!;
        public string AdditionalAddressLine2 { get; set; }  = default!;
        public int Country { get; set; }  = default!;
        public int State { get; set; }  = default!;
        public object[] Attribute { get; set; }  = default!;

        public override string TableName => "s_user_addresses";

        ///<summary>
        /// Use the Constructor to set all Required Properties correctly
        ///</summary>
        public Address()
        {
            
        }

    }
}
