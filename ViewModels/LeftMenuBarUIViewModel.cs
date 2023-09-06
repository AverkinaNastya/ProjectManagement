using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Infrastructure.Commands;
using ProjectManagement.Models;
using ProjectManagement.Services;
using ProjectManagement.Services.NavigatorPages;
using ProjectManagement.ViewModels.Base;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace ProjectManagement.ViewModels
{
    class LeftMenuBarUIViewModel: ViewModel
    {
        #region Поля

        public static readonly string MainPageViewModelAlias = "MainPage";
        public static readonly string OrganizationalStructurePageViewModelAlias = "OrganizationalStructurePage";
        public static readonly string TasksPageAlias = "TasksPage";
        public static readonly string ProjectsPageViewModelAlias = "ProjectsPageViewModel";
        public static readonly string NotificationsPageViewModelAlias = "NotificationsPageViewModel";
        public static readonly string NotFoundPageViewModelAlias = "Page404ViewModel";



        private readonly IViewModelsResolver _resolver;

        private readonly INotifyPropertyChanged _mainPageViewModel;
        public INotifyPropertyChanged MainPage
        {
            get => _mainPageViewModel;
        }

        private readonly INotifyPropertyChanged _organizationalStructurePageViewModel;
        public INotifyPropertyChanged OrganizationalStructurePage
        {
            get => _organizationalStructurePageViewModel;
        }


        private readonly INotifyPropertyChanged _projectsPageViewModel;
        public INotifyPropertyChanged ProjectsPage
        {
            get => _projectsPageViewModel;
        }

        private readonly INotifyPropertyChanged _tasksPageViewModel;
        public INotifyPropertyChanged TasksPage
        {
            get => _tasksPageViewModel;
        }

        private readonly INotifyPropertyChanged _notificationsPageViewModel;
        public INotifyPropertyChanged NotificationsPage
        {
            get => _notificationsPageViewModel;
        }

        #region StatusMenu: Bool - Состояние меню (true - развернуто, false - свернуто)
        private bool _StatusMenu;

        public bool StatusMenu
        {
            get => _StatusMenu;
            set => Set(ref _StatusMenu, value);
        }
        #endregion

        #endregion

        #region Команды

        public ICommand EditStatusMenuCommand { get; }
        private void OnEditStatusMenuCommandExecuted(object p)
        {
            StatusMenu = !StatusMenu;
        }
        private bool CanEditStatusMenuCommandExecute(object p) => true;

        public ICommand GoToPageOrgStractCommand { get; }
        private void OnGoToPageOrgStractCommandExecuted(object p)
        {
            Navigation.Navigate(Navigation.OrganizationalStructurePageAlias, OrganizationalStructurePage);
        }
        private bool CanGoToPageOrgStractCommandExecute(object p) => true;

        public ICommand GoToPageProjectsCommand { get; }
        private void OnGoToPageProjectCommandExecuted(object p)
        {
            Navigation.Navigate(Navigation.ProjectsPageAlias, ProjectsPage);
        }
        private bool CanGoToPageProjectCommandExecute(object p) => true;

        public ICommand GoToPageTasksCommand { get; }
        private void OnGoToPageTasksCommandExecuted(object p)
        {
            Navigation.Navigate(Navigation.TasksPageAlias, TasksPage);
        }
        private bool CanGoToPageTasksCommandExecute(object p) => true;

        public ICommand GoToPageMainCommand { get; }
        private void OnGoToPageMainCommandExecuted(object p)
        {
            Navigation.Navigate(Navigation.MainPageViewModelAlias, MainPage);
        }
        private bool CanGoToPageMainCommandExecute(object p) => true;

        public ICommand GoToPageNotificationsCommand { get; }
        private void OnGoToPageNotificationsCommandExecuted(object p)
        {
            Navigation.Navigate(Navigation.NotificationsPageAlias, NotificationsPage);
        }
        private bool CanGoToPageNotificationsCommandExecute(object p) => true;

        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object p)
        {
            CurrentEmployee.currentEmployee = new Employee();
            App.Services.GetRequiredService<IUserDialog>().OpenAuthorizationWindow();
            App.Services.GetRequiredService<IUserDialog>().CloseMainWindow();
        }
        private bool CanBackCommandExecute(object p) => true;
        #endregion

        #region Конструктор
        public LeftMenuBarUIViewModel()
        {
            StatusMenu = false;
            EditStatusMenuCommand = new RelayCommand(OnEditStatusMenuCommandExecuted, CanEditStatusMenuCommandExecute);
            GoToPageOrgStractCommand = new RelayCommand(OnGoToPageOrgStractCommandExecuted, CanGoToPageOrgStractCommandExecute);
            GoToPageProjectsCommand = new RelayCommand(OnGoToPageProjectCommandExecuted, CanGoToPageProjectCommandExecute);
            GoToPageTasksCommand = new RelayCommand(OnGoToPageTasksCommandExecuted, CanGoToPageTasksCommandExecute);
            GoToPageMainCommand = new RelayCommand(OnGoToPageMainCommandExecuted, CanGoToPageMainCommandExecute);
            BackCommand = new RelayCommand(OnBackCommandExecuted, CanBackCommandExecute);
            GoToPageNotificationsCommand = new RelayCommand(OnGoToPageNotificationsCommandExecuted, CanGoToPageNotificationsCommandExecute);

            _mainPageViewModel = App.Services.GetRequiredService<IViewModelsResolver>().GetViewModelInstance(MainPageViewModelAlias);
            Navigation.Navigate(Navigation.MainPageViewModelAlias, MainPage);
        }

        public LeftMenuBarUIViewModel(IViewModelsResolver viewModelsResolver) : this()
        {
            _resolver = viewModelsResolver;

            _mainPageViewModel = _resolver.GetViewModelInstance(MainPageViewModelAlias);
            _organizationalStructurePageViewModel = _resolver.GetViewModelInstance(OrganizationalStructurePageViewModelAlias);
            _projectsPageViewModel = _resolver.GetViewModelInstance(ProjectsPageViewModelAlias);
            _tasksPageViewModel = _resolver.GetViewModelInstance(TasksPageAlias);
            _notificationsPageViewModel = _resolver.GetViewModelInstance(NotificationsPageViewModelAlias);
        }
        #endregion
    }
}
