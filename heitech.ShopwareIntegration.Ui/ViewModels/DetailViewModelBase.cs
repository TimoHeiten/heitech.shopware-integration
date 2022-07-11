using System.Windows.Controls;
using heitech.ShopwareIntegration.State;

namespace ShopwareIntegration.Ui.ViewModels;

public abstract class DetailViewModelBase
{
    public string Id { get; init; } = default!;
    public DataContext Context { get; protected init; } = default!;
    public abstract string Type { get; }

    // kind of an anti pattern, but was a fast and cheap solution
    public virtual Control GenerateViewData()
    {
        var ctrl = new TextBox()
        {
            Text = "here goes your customized control",
            IsReadOnly = true
        };

        return ctrl;
    }
}