using System.ComponentModel;

namespace ProjectManagement.ViewModels.Base
{
    public interface IViewModelsResolver
    {
        INotifyPropertyChanged GetViewModelInstance(string alias);
    }
}
