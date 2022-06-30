namespace heitech.ShopwareIntegration.State.DetailModels
{
    ///<summary>
    /// Value Object for the RessoruceId of the Shopware Backend
    ///</summary>
    public sealed class RessourceId
    {
        const string FORMAT = "N";
        public string Id { get; }
        public RessourceId(string id) => Id = id;

        public RessourceId(Guid id) => Id = id.ToString(FORMAT);
        public RessourceId GenerateNew() => new(Guid.NewGuid().ToString(FORMAT));

        public static RessourceId From(Guid guid)
            => new RessourceId(guid);

        public static RessourceId From(string id)
            => new(id);

        public static implicit operator string(RessourceId id) => id.Id;
    }
}