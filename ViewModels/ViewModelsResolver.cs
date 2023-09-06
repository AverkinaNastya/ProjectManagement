using ProjectManagement.Services;
using ProjectManagement.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace ProjectManagement.ViewModels
{
    public class ViewModelsResolver : IViewModelsResolver
    {

        private readonly Dictionary<string, Func<INotifyPropertyChanged>> _vmResolvers = new Dictionary<string, Func<INotifyPropertyChanged>>();

        public ViewModelsResolver()
        {
            _vmResolvers.Add(LeftMenuBarUIViewModel.MainPageViewModelAlias, () => new MainPageViewModel());
            _vmResolvers.Add(LeftMenuBarUIViewModel.OrganizationalStructurePageViewModelAlias, () => new OrganizationalStructurePageViewModel());
            _vmResolvers.Add(LeftMenuBarUIViewModel.ProjectsPageViewModelAlias, () => new ProjectsPageViewModel());
            _vmResolvers.Add(LeftMenuBarUIViewModel.TasksPageAlias, () => new MyTasksPageViewModel());
            _vmResolvers.Add(LeftMenuBarUIViewModel.NotificationsPageViewModelAlias, () => new NotificationsPageViewModel());
            
            _vmResolvers.Add(LeftMenuBarUIViewModel.NotFoundPageViewModelAlias, () => new Page404ViewModel());
        }

        public INotifyPropertyChanged GetViewModelInstance(string alias)
        {
            if (_vmResolvers.ContainsKey(alias))
            {
                return _vmResolvers[alias]();
            }

            return _vmResolvers[LeftMenuBarUIViewModel.NotFoundPageViewModelAlias]();
        }
    }
}
