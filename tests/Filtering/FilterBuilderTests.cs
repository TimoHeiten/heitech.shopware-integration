using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using FluentAssertions;
using heitech.ShopwareIntegration.Filtering;
using heitech.ShopwareIntegration.Models;
using Xunit;

namespace heitech.Shopware.tests
{
    public class FilterBuilderTests
    {
        [Theory]
        [MemberData(nameof(FilterExamples))]
        public void Filter_Examples(Func<IFilter> builder, object expected)
        {
            // Given
            IFilter filter = builder();

            // When
            var result = filter.AsSearchInstance();

            // Then
            result.Should().BeEquivalentTo(expected);
        }


        public static IEnumerable<object[]> FilterExamples
        {
            get
            {
                // all single filters
                yield return new object[] {
                    () => FilterFactory.CreateBuilder<TestEntity>()
                                       .Sort(new SortParameter<TestEntity>(x => x.SortProperty))
                                       .Build(),
                     new {
                         filter = new {
                            sort = new object[] {
                                new {
                                    field = "sortProperty",
                                    order = "asc",
                                    naturalSorting = false
                                }
                            }
                         }
                     }
                 };

                yield return new object[] {
                    () => FilterFactory.CreateBuilder<TestEntity>()
                                       .Sort(new SortParameter<TestEntity>(x => x.SortProperty),
                                             new SortParameter<TestEntity>(x => x.SortProperty, order: FilterConstants.DESC))
                                       .Build(),
                     new {
                         filter = new {
                            sort = new object[] {
                                new {
                                    field = "sortProperty",
                                    order = "asc",
                                    naturalSorting = false
                                },
                                new {
                                    field = "sortProperty",
                                    order = "desc",
                                    naturalSorting = false
                                }
                            }
                         }
                     }
                };
                yield return new object[] {
                   () => FilterFactory.CreateBuilder<TestEntity>()
                                       .Limit(10)
                                       .Build(),
                     new {
                         filter = new {
                             limit = 10
                         }
                     }
                };
                yield return new object[] {
                   () => FilterFactory.CreateBuilder<TestEntity>()
                                       .Page(2)
                                       .Build(),
                     new {
                         filter = new {
                            page = 2
                         }
                     }
                };
                yield return new object[] {
                   () => FilterFactory.CreateBuilder<TestEntity>()
                                       .Grouping(x => x.Id, x => x.SortProperty)
                                       .Build(),
                     new {
                         filter = new {
                            grouping = new [] { "id", "sortProperty" }
                         }
                     }
                };

                // combined filters
                yield return new object[] {
                  () => FilterFactory.CreateBuilder<TestEntity>()
                                       .Sort(new SortParameter<TestEntity>(x => x.SortProperty))
                                       .Limit(10)
                                       .Page(2)
                                       .Grouping(x => x.Id)
                                       .Build(),
                     new {
                         filter = new {
                            limit = 10,
                            page = 2,
                            sort = new object[] {
                                new {
                                    field = "sortProperty",
                                    order = "asc",
                                    naturalSorting = false
                                }
                            },
                            grouping = new object[] {
                                "id"
                            }
                         }
                     }
                };


                // associations are always first
                // post filters with aggregations
                // 
            }
        }

        private sealed class TestEntity : BaseEntity
        {
            [JsonPropertyName("sortProperty")]
            public string SortProperty { get; set; } = default!;

            [JsonPropertyName("nestedEntity")]
            public NestedTestEntity NestedEntity { get; set; } = default!;
            public TestEntity() : base()
            { }
        }

        private sealed class NestedTestEntity : BaseEntity
        {
            public NestedTestEntity() : base()
            { }

            [JsonPropertyName("nestedName")]
            public string NestedName { get; set; } = "Nested!";
        }
    }
}