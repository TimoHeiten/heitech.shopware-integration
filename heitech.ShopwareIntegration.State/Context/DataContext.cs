using System.Collections;
using System.Reflection;
using heitech.ShopwareIntegration.State.DetailModels;

namespace heitech.ShopwareIntegration.State
{
    /// <summary>
    /// Provides the Data for the specified CRUD operation on the Shopware API
    /// </summary>
    public abstract class DataContext : IEnumerable<DetailsEntity>
    {
        public int PageNo { get; }
        public RessourceId Id { get; }
        internal Dictionary<string, object>? AdditionalData { get; set; }
        protected DataContext(RessourceId id, int pageNo, Dictionary<string, object>? additionalData)
        {
            Id = id;
            PageNo = pageNo;
            AdditionalData = additionalData;
        }

        /// <summary>
        /// The Entity this DataContext wraps. Be aware this 
        /// </summary>
        public abstract DetailsEntity? Entity { get; }

        /// <summary>
        /// Create the DataContext used to retrieve a single Entity (aka Detail) via its pageNo. 
        /// </summary>
        /// <param name="id">the id of the Entity in question</param>
        /// <param name="pageNo">the pageNo of the item to update the cache</param>
        /// <param name="additionalData">Reserved for future use</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataContext GetDetail<T>(string id, int pageNo, Dictionary<string, object> additionalData = default!)
            where T : DetailsEntity
            => new DetailsContext<T>(RessourceId.From(id), pageNo, additionalData);

        internal static DataContext FromRetrieveDetails<T>(T entity, DataContext context)
            where T : DetailsEntity
            => new DetailsContext<T>(entity, context.Id, context.PageNo, context.AdditionalData!);

        /// <summary>
        /// Create the DataContext to update (via HttpPatch) an Entity. Supply the PatchedValue and all the fields and their new values. 
        /// </summary>
        /// <param name="patch"></param>
        /// <param name="pageNo"></param>
        /// <param name="additionalData"></param>
        /// <returns></returns>
        public static DataContext Update(PatchedValue patch, int pageNo,
            Dictionary<string, object> additionalData = default!)
        {
            var context = new PatchedValueContext(patch, pageNo, additionalData);
            context.AddUpdate(patch.Values);
            return context;
        }

        internal static DataContext FromUpdateResult<T>(T entity, DataContext patched)
            where T : DetailsEntity
            => new PatchedValueContext(PatchedValue.From(entity, null!), patched.PageNo, patched!.AdditionalData);

        /// <summary>
        /// Create a DataContext to Create a new Entity of Type T with the Shopware API 
        /// </summary>
        /// <param name="entity">The actual instance of the entity. Make sure all Required fields are set (as specified in the shopware6 api documentation)</param>
        /// <param name="pageNo">for caching purposes</param>
        /// <param name="additionalData">Reserved for future use</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataContext Create<T>(T entity, int pageNo, Dictionary<string, object> additionalData = default!)
            where T : DetailsEntity
            => new CreateContext<T>(entity, pageNo, additionalData);

        /// <summary>
        /// Create a DataContext to Delete an existing Entity of Type T with the Shopware API 
        /// </summary>
        /// <param name="id">The id of the entity</param>
        /// <param name="pageNo">for caching purposes</param>
        /// <param name="additionalData">Reserved for future use</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataContext Delete<T>(string id, int pageNo, Dictionary<string, object> additionalData = default!)
            where T : DetailsEntity
        {
            var deleteContext = new DeleteContext(RessourceId.From(id), pageNo, additionalData);
            deleteContext.AddIsDelete();
            return deleteContext;
        }

        internal static DataContext FromDelete<T>(DetailsEntity entity, DataContext deleteContext)
            where T : DetailsEntity
            => new DeleteContext(entity, deleteContext.Id, deleteContext.PageNo, deleteContext.AdditionalData);

        /// <summary>
        /// Create a DataContext for a retrieval of a Page of any given Type. Make sure to provide a Filter via the DataContext.SetFilter method to limit the results, since the api cannot handle an unfiltered Request. Yikes!
        /// </summary>
        /// <param name="pageNo">for caching purposes</param>
        /// <param name="additionalData">Reserved for future use</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataContext GetPage<T>(int pageNo, Dictionary<string, object>? additionalData = default!)
            where T : DetailsEntity
            => new PageContext(typeof(T), pageNo, additionalData);

        internal static DataContext FromRetrievePage<T>(IEnumerable<DetailsEntity> page, DataContext context)
            where T : DetailsEntity
            => new PageContext(typeof(T), page, context.PageNo, context.AdditionalData);


        public virtual IEnumerator<DetailsEntity> GetEnumerator()
        {
            yield return Entity;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class CreateContext<T> : DataContext
            where T : DetailsEntity
        {
            public CreateContext(T entity, int pageNo, Dictionary<string, object> additionalData)
                : base(RessourceId.From(entity.Id), pageNo, additionalData)
            { Entity = entity; }

            public override DetailsEntity Entity { get; }
        }

        ///<summary>
        /// GetDetails request DataContext
        ///</summary>
        private class DetailsContext<T> : DataContext
            where T : DetailsEntity
        {
            public override DetailsEntity Entity { get; }
            public DetailsContext(T entity, RessourceId id, int pageNo, Dictionary<string, object> additionalData = default!)
                : base(id, pageNo, additionalData)
            { Entity = entity; }

            public DetailsContext(RessourceId id, int pageNo, Dictionary<string, object> additionalData = default!)
                : base(id, pageNo, additionalData)
            { Entity = default!; }
        }

        ///<summary>
        /// Update Request DataContext
        ///</summary>
        private class PatchedValueContext : DataContext
        {
            public override DetailsEntity Entity { get; }
            public PatchedValueContext(PatchedValue entity, int pageNo,
                Dictionary<string, object>? additionalData = default!)
                : base(RessourceId.From(entity.Id), pageNo, additionalData)
            {
                Entity = entity.Source;
            }
        }

        ///<summary>
        /// Delete Request DataContext
        ///</summary>
        private class DeleteContext : DataContext
        {
            // request version w Entity
            public DeleteContext(DetailsEntity entity, RessourceId id, int pageNo, Dictionary<string, object>? additionalData)
                : base(id, pageNo, additionalData)
            { Entity = entity; }

            // use for the result entity value --> override the request version
            public DeleteContext(RessourceId id, int pageNo, Dictionary<string, object>? additionalData)
                : base(id, pageNo, additionalData)
            {
                Entity = null!;
                this.AddIsDelete();
            }

            public override DetailsEntity Entity { get; }
        }

        ///<summary>
        /// GetPage Request DataContext
        ///</summary>
        private class PageContext : DataContext
        {
            private static RessourceId Generate(MemberInfo type, int pageNo) => RessourceId.From($"{pageNo}-{type.Name}");
            private readonly IEnumerable<DetailsEntity> _page = Array.Empty<DetailsEntity>();
            public PageContext(MemberInfo type, int pageNo, Dictionary<string, object>? additionalData)
                : base(Generate(type, pageNo), pageNo, additionalData)
            { }

            public PageContext(Type type, IEnumerable<DetailsEntity> page, int pageNo, Dictionary<string, object>? additionalData)
                : base(Generate(type, pageNo), pageNo, additionalData)
            { _page = page; }

            public override DetailsEntity Entity => throw new NotSupportedException("use the enumeration on the PageContext!");
            public override IEnumerator<DetailsEntity> GetEnumerator() => _page.GetEnumerator();
        }
    }
}