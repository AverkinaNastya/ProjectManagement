using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Infrastructure.Classes;
using ProjectManagement.Infrastructure.Commands;
using ProjectManagement.Models;
using ProjectManagement.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ProjectManagement.ViewModels
{
    class ProjectsPageViewModel: ViewModel
    {
        private readonly Visibility _visibilityAdmin;
        public Visibility VisibilityAdmin
        {
            get => _visibilityAdmin;
        }

        private ObservableCollection<Project> _projects = new();
        public ObservableCollection<Project> Projects
        {
            get => _projects;
            set => Set(ref _projects, value);
        }

        private Visibility _visibilityFilter = Visibility.Collapsed;
        public Visibility VisibilityFilter
        {
            get => _visibilityFilter;
            set => Set(ref _visibilityFilter, value);
        }

        private String? _startDateFirst;
        public String? StartDateFirst
        {
            get => _startDateFirst;
            set {
                Set(ref _startDateFirst, value);
                Filter();
            }
        }

        private String? _startDateSecond;
        public String? StartDateSecond
        {
            get => _startDateSecond;
            set
            {
                Set(ref _startDateSecond, value);
                Filter();
            }
        }

        private String? _completDateFirst;
        public String? CompletDateFirst
        {
            get => _completDateFirst;
            set { 
                Set(ref _completDateFirst, value);
                Filter();
            }
        }

        private String? _completDateSecond;
        public String? CompletDateSecond
        {
            get => _completDateSecond;
            set
            {
                Set(ref _completDateSecond, value);
                Filter();
            }
        }

        private bool _allStatus = true;
        public bool AllStatus
        {
            get => _allStatus;
            set
            {
                Set(ref _allStatus, value);
                Filter();
            }
        }

        private bool _completStatus = false;
        public bool CompletStatus
        {
            get => _completStatus;
            set
            {
                Set(ref _completStatus, value);
                Filter();
            }
        }

        private bool _noCompletStatus = false;
        public bool NoCompletStatus
        {
            get => _noCompletStatus;
            set
            {
                Set(ref _noCompletStatus, value);
                Filter();
            }
        }

        private String _filterStr = "";
        public String FilterStr
        {
            get => _filterStr;
            set
            {
                Set(ref _filterStr, value);
                Filter();
            }
        }

        private Project? _selectProject;
        public Project? SelectProject
        {
            get => _selectProject;
            set => Set(ref _selectProject, value);
        }

        public ICommand VisibilityFilterCommand { get; }
        private void OnVisibilityFilterCommandExecuted(object p) {
            if (VisibilityFilter == Visibility.Collapsed) VisibilityFilter = Visibility.Visible;
            else VisibilityFilter = Visibility.Collapsed;
        }
        private bool CanVisibilityFilterCommandExecute(object p) => true;

        public ICommand OpenInfoProjectCommand { get; }
        private void OnOpenInfoProjectCommandExecuted(object p)
        {
            if (p is Project project)
            {
                ChangingProject.project = project;
                App.Services.GetRequiredService<IUserDialog>().OpenProjectWindow();
                ChangingProject.project = null!;
                Filter();
            }
        }
        private bool CanOpenInfoProjectCommandExecute(object p) => true;

        public ICommand AddProjectCommand { get; }
        private void OnAddProjectCommandExecuted(object p)
        {
            App.Services.GetRequiredService<IUserDialog>().OpenAddProjectWindow();
            Filter();
        }
        private bool CanAddProjectCommandExecute(object p) => true;

        private void Filter()
        {
            Projects.Clear();

            using (ProjectManagementContext db = new ())
                db.Projects.Include(e => e.Employees).Where(e => e.Employees.Contains(CurrentEmployee.currentEmployee) || CurrentEmployee.currentEmployee.Admin).ToList().ForEach(Projects.Add);

            Projects.Where(e => !e.Name.Contains(FilterStr) && LevenshteinDistance.Distance(e.Name, FilterStr) > 3).ToList().ForEach(e => Projects.Remove(e));

            DateOnly date;

            if (Projects.Count != 0 && DateOnly.TryParse(StartDateFirst, out date))
                Projects.Where(e => e.StartDate < date).ToList().ForEach(e => Projects.Remove(e));

            if (Projects.Count != 0 && DateOnly.TryParse(StartDateSecond, out date))
                Projects.Where(e => e.StartDate > date).ToList().ForEach(e => Projects.Remove(e));

            if (Projects.Count != 0 && DateOnly.TryParse(CompletDateFirst, out date))
                Projects.Where(e => e.CompletDate > date).ToList().ForEach(e => Projects.Remove(e));

            if (Projects.Count != 0 && DateOnly.TryParse(CompletDateSecond, out date))
                Projects.Where(e => e.CompletDate < date).ToList().ForEach(e => Projects.Remove(e));

            if (Projects.Count != 0 && CompletStatus)
                Projects.Where(e => !e.Completed).ToList().ForEach(e => Projects.Remove(e));
            else if (Projects.Count != 0 && NoCompletStatus)
                Projects.Where(e => e.Completed).ToList().ForEach(e => Projects.Remove(e));

        }

        public ProjectsPageViewModel()
        {
            _visibilityAdmin = CurrentEmployee.currentEmployee.Admin ? Visibility.Visible : Visibility.Collapsed;
            VisibilityFilterCommand = new RelayCommand(OnVisibilityFilterCommandExecuted, CanVisibilityFilterCommandExecute);
            OpenInfoProjectCommand = new RelayCommand(OnOpenInfoProjectCommandExecuted, CanOpenInfoProjectCommandExecute);
            AddProjectCommand = new RelayCommand(OnAddProjectCommandExecuted, CanAddProjectCommandExecute);
            using (ProjectManagementContext db = new())
            {
                db.Projects.Include(e => e.Employees).Where(e => e.Employees.Contains(CurrentEmployee.currentEmployee) || CurrentEmployee.currentEmployee.Admin).ToList().ForEach(Projects.Add);
            }
        }
    }
}
