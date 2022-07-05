using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using heitech.ShopwareIntegration.State.DetailModels;

namespace ShopwareIntegration.Ui.Definition;

public sealed class MasterViewColumns
{
    public DataTable DataTable { get; }

    public MasterViewColumns(IEnumerable<DetailsEntity> entity)
    {
        DataTable = new DataTable();

        var c = Columns(entity);
        c.ColumnNames.ToList()
            .ForEach(x => DataTable.Columns.Add(x));

        foreach (var row in c.Values)
            DataTable.Rows.Add(row);
    }

    private static ColumnValuePairs Columns(IEnumerable<DetailsEntity> details)
    {
        var evaluated = details!.ToArray();
        var first = evaluated.FirstOrDefault();
        if (first is null) return new ColumnValuePairs(Array.Empty<string>(), Array.Empty<object>());

        string[] columns = first switch
        {
            ProductDetails productDetails => new[] { nameof(ProductDetails.Id), nameof(ProductDetails.Ean) },
            OrderDetails orderDetails => Array.Empty<string>(),
            ProductManufacturerDetails productManufacturerDetails => Array.Empty<string>(),
            CategoryDetails categoryDetails => Array.Empty<string>(),
            _ => throw new NotSupportedException($"Type {first.GetType()?.Name} is not allowed in MasterViewColumns")
        };

        // todo
        return new ColumnValuePairs(columns,
            evaluated!.Cast<ProductDetails>().Select(x => new object[] { x.Id, x.Ean }).ToArray());
    }

    private sealed record ColumnValuePairs(string[] ColumnNames, object[] Values);
}