using System.Text.Json;
using System.Linq.Expressions;
using heitech.ShopwareIntegration.State.Integration.Filtering.Parameters;
using heitech.ShopwareIntegration.State.Integration.Models;

namespace heitech.ShopwareIntegration.Filtering;

///<summary>
/// Builder Object that allows to create Filters
/// Not threadSafe!
///</summary>
internal class FilterBuilder<T> : IFilterBuilder<T>, IFilter
    where T : BaseEntity
{
    private List<SortParameter<T>> sortparameters = default!;
    private List<string> groupings = default!;
    private List<FilterParameter<T>> filters = default!;
    private int? page = default!;
    private int? limit = default!;
    private AssociationObject? associationObject;

    public FilterBuilder() { }

    public IFilterBuilder<T> Aggregate(AggregateParameter parameter, params AggregateParameter[] other)
    {
        throw new NotImplementedException();
    }

    public IFilterBuilder<T> Association<TOut>(Expression<Func<T, TOut>> propertyExpression, IFilter nestedFilter = null!)
    {
        string getName() => FilterConstants.GetName<T, TOut>(propertyExpression);
        associationObject ??= new(getName(), nestedFilter ?? EmptyFilter.Instance);

        return this;
    }

    public IFilterBuilder<T> Grouping(Expression<Func<T, object>> propExpression, params Expression<Func<T, object>>[] other)
    {
        this.groupings ??= new();
        this.groupings.AddRange(other.Concat(new[] { propExpression }).Select(x => FilterConstants.GetName<T>(x)));

        return this;
    }

    public IFilterBuilder<T> Filter(FilterParameter<T> filter, params FilterParameter<T>[] other)
    {
        this.filters ??= new();
        this.filters.AddRange(other.Concat(new[] { filter }));
        return this;
    }

    public IFilterBuilder<T> Limit(int limit)
    {
        this.limit ??= limit;
        return this;
    }

    public IFilterBuilder<T> Page(int pageNo)
    {
        this.page ??= pageNo;
        return this;
    }

    public IFilterBuilder<T> Sort(SortParameter<T> parameter, params SortParameter<T>[] other)
    {
        sortparameters ??= new();
        sortparameters.AddRange(other.Concat(new[] { parameter }));
        return this;
    }

    public IFilter Build() => this;

    public IFilter BuildAsPostFilter()
    {
        throw new NotImplementedException();
    }

    public object AsSearchInstance()
    {
        var searchObject = new
        {
            page = this.page,
            limit = this.limit,
            grouping = this.groupings?.ToArray(),
            filter = filters?.Select(x => x.ToInstance()).ToArray(),
            sort = sortparameters?.Select(x => x.ToInstance()).ToArray()
        };

        if (associationObject is not null)
            return associationObject.AsSearchInstance(searchObject);

        return searchObject;
    }

    private class AssociationObject
    {
        // todo must support multiple associations... so IParameterObject and multiple objects as key/values of the associations object
        // todo 2 also test if this is even working :D
        private readonly string name;
        private readonly IFilter nested;

        public AssociationObject(string name, IFilter nested)
        {
            this.name = name;
            this.nested = nested;
        }

        public object AsSearchInstance(object otherFilter)
        {
            string serialize(object obj) => JsonSerializer.Serialize(obj);
            // kinda hacky, but we cannot have anonymous objects with dynamic keys, so we turn it into a string
            // then a json and back to an object.
            /*
                result looks like this:
                {
                    "associations" : {
                        "($name)" : {
                            nestedFilter
                        }
                    },
                    otherfilters 
                }
            */
            bool hasAssociationFilter = this.nested.AsSearchInstance() is not null;
            string associationsFilter = hasAssociationFilter
                ? "{ \"associatons\" : { \"{0}\" : {1} }, {2} }"
                : "{ \"associatons\" : { \"{0}\" : {  } }, {1} }";

            string serializedOtherFilters = serialize(otherFilter);
            string completeJsonString = hasAssociationFilter
                ? string.Format(associationsFilter, name, serialize(nested.AsSearchInstance()), serializedOtherFilters)
                : string.Format(associationsFilter, name, serializedOtherFilters);

            return JsonSerializer.Deserialize<object>(completeJsonString)!;
        }
    }
}