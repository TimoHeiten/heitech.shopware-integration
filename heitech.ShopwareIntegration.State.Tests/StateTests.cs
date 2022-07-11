using FluentAssertions;
using heitech.ShopwareIntegration.State;
using heitech.ShopwareIntegration.State.Cache;
using heitech.ShopwareIntegration.State.DetailModels;
using heitech.ShopwareIntegration.State.Interfaces;
using heitech.ShopwareIntegration.State.Logging;
using NSubstitute;

namespace heitech.Shopware.tests
{
    public class StateTests
    {
        private readonly StateManager _sut;
        private readonly IStateManager _mock;
        private readonly IStateManager _logger;
        private readonly CacheSpy _cache;
        private readonly List<string> _logs;

        private readonly Action<CacheItem> _unlistDummy = _ => { };

        public StateTests()
        {
            _logs = new();
            _mock = Substitute.For<IStateManager>();
            _logger = new Logger(s => _logs.Add(s));
            _cache = new CacheSpy(_mock);

            _sut = new StateManager(_logger, _cache);
        }

        [Fact]
        public async Task GetPage_Gets_From_Active_Cache()
        {
            // Arrange
            var initial = DataContext.GetPage<TestEntity>(pageNo: 1);
            var context = DataContext.FromRetrievePage<TestEntity>(new TestEntity[] { new(), new(), new() }, initial);

            var item = CacheItem.Create(context, _unlistDummy);
            _cache.Pages.Add(item.Key, item);

            // Act
            var result = await _sut.RetrievePage<TestEntity>(context);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);
            _logs.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetPage_Calls_Api_On_Cache_Miss()
        {
            // Arrange
            var context = DataContext.GetPage<TestEntity>(42);
            _mock.RetrievePage<TestEntity>(context).Returns(new TestEntity[] { new(), new(), new() });

            // Act
            var result = await _sut.RetrievePage<TestEntity>(context);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(3);

            await _mock.Received().RetrievePage<TestEntity>(context);
            _logs.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetDetails_Returns_Details_on_Cache_Hit()
        {
            // Arrange
            var initial = DataContext.GetDetail<TestEntity>("should be guid for api", pageNo: 1);
            var context = DataContext.FromRetrieveDetails<TestEntity>(new TestEntity(), initial);

            var item = CacheItem.Create(context, _unlistDummy);
            _cache.Details.Add(item.Key, item);

            // Act
            var result = await _sut.RetrieveDetails<TestEntity>(context);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(context.Entity);
            _logs.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetDetails_Returns_Details_on_Cache_Miss_Via_API_Call()
        {
            // Arrange
            var context = DataContext.GetDetail<TestEntity>("any-id", pageNo: 42);
            _mock.RetrieveDetails<TestEntity>(context).Returns(new TestEntity());

            // Act
            var result = await _sut.RetrieveDetails<TestEntity>(context);

            // Assert
            await _mock.Received().RetrieveDetails<TestEntity>(context);
            _logs.Should().NotBeEmpty();
        }

        [Fact]
        public async Task CreateEntity_Adds_To_Cache_And_Calls_Mock()
        {
            // Arrange
            var context = DataContext.Create(new TestEntity(), 12);
            _mock.CreateAsync<TestEntity>(context).Returns((TestEntity)context.Entity);

            // Act
            var result = await _sut.CreateAsync<TestEntity>(context);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(context.Entity);
            _logs.Should().NotBeEmpty();
            _cache.Details.Should().ContainKey(CacheItem.Create(context, _unlistDummy).Key);
        }

        [Fact]
        public async Task DeleteEntity_Removes_From_Page_and_Detail_Cache_And_CallsApi()
        {
            // Arrange
            var context = DataContext.Delete<TestEntity>("abc", 12);
            var expected = new TestEntity { Id = "abc" };
            _mock.DeleteAsync<TestEntity>(context).Returns(expected);
            var item = CacheItem.Create(context, _unlistDummy);
            _cache.Details.Add(item.Key, item);

            var pageContext = DataContext.FromRetrievePage<TestEntity>(
                new TestEntity[]
                {
                    new() { Id = "abc" }, 
                    new(), new() 
                }, 
                context
            );
            var pageItem = CacheItem.Create(pageContext, _unlistDummy);
            _cache.Pages.Add(pageItem.Key, pageItem);
            _cache.Pages.Should().HaveCount(1); // sanity check

            // Act
            var result = await _sut.DeleteAsync<TestEntity>(context);

            // Assert
            _logs.Should().NotBeEmpty();
            result.Should().NotBeNull();
            result.Should().Be(expected);
            _cache.Details.Should().NotContainKey(CacheItem.Create(context, _unlistDummy).Key);
            _cache.Pages.Should().HaveCount(1);
            _cache.Pages.Single().Value.Context.Should().HaveCount(2);
        }


        public class TestEntity : DetailsEntity
        {
            public TestEntity()
            { }
        }

        internal sealed class CacheSpy : CacheStorage
        {
            public CacheSpy(IStateManager client) : base(client)
            { }

            public Dictionary<string, CacheItem> Pages => _pages;
            public Dictionary<string, CacheItem> Details => _cache;

        }
    }
}