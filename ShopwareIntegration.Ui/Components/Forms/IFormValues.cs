using System.Threading.Tasks;
using ShopwareIntegration.Ui.Services;

namespace ShopwareIntegration.Ui.Components.Forms;

public interface IFormValues
{
    Task<bool> OnSaveAsync(StateService service);
    Task<bool> OnCancelAsync(StateService service);
}