using ProjectManagement.Views.Pages;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ProjectManagement.Services.NavigatorPages
{
    public class PagesResolver : IPageResolver
    {

        private readonly Dictionary<string, Func<Page>> _pagesResolvers = new Dictionary<string, Func<Page>>();

        public PagesResolver()
        {
            _pagesResolvers.Add(Navigation.MainPageViewModelAlias, () => new MainPage());
            _pagesResolvers.Add(Navigation.OrganizationalStructurePageAlias, () => new OrganizationalStructurePage());
            _pagesResolvers.Add(Navigation.TasksPageAlias, () => new MyTasksPage());
            _pagesResolvers.Add(Navigation.ProjectsPageAlias, () => new ProjectsPage());
            _pagesResolvers.Add(Navigation.NotificationsPageAlias, () => new NotificationsPage());
        }

        public Page GetPageInstance(string alias)
        {
            if (_pagesResolvers.ContainsKey(alias))
            {
                return _pagesResolvers[alias]();
            }

            return _pagesResolvers[Navigation.NotFoundPageAlias]();
        }
    }
}
