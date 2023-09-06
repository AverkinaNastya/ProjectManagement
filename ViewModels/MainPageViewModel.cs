using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Infrastructure.Commands;
using ProjectManagement.Models;
using ProjectManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectManagement.ViewModels
{
    internal class MainPageViewModel: ViewModel
    {
        private string _helloText = "";
        public string HelloText
        {
            get => _helloText;
            set => Set(ref _helloText, value);
        }

        private List<Task> _tasks = new List<Task>();
        public List<Task> Tasks
        {
            get => _tasks;
            set => Set(ref _tasks, value);
        }

        private List<Project> _projects = new List<Project>();
        public List<Project> Projects
        {
            get => _projects;
            set => Set(ref _projects, value);
        }

        #region OpenInfoProjectCommand - открытие окна проекта
        public ICommand OpenInfoProjectCommand { get; }
        private void OnOpenInfoProjectCommandExecuted(object p)
        {
            if (p is Project project)
            {
                ChangingProject.project = project;
                App.Services.GetRequiredService<IUserDialog>().OpenProjectWindow();
                ChangingProject.project = null!;
                using (ProjectManagementContext db = new())
                {
                    Projects = db.Projects.Where(e => e.Employees.Contains(CurrentEmployee.currentEmployee)).ToList();
                    Tasks = db.Tasks.Where(e => e.Executor == CurrentEmployee.currentEmployee.Id && e.Status != 3).ToList();
                }
            }
        }
        private bool CanOpenInfoProjectCommandExecute(object p) => true;
        #endregion

        #region EditTaskProjectCommand - изменение задачи
        public ICommand EditTaskProjectCommand { get; }
        private void OnEditTaskProjectCommandExecuted(object p)
        {
            ChangingTask.task = p as Task;
            App.Services.GetRequiredService<IUserDialog>().OpenEditTaskWindow();
            ChangingTask.task = null;
            using (ProjectManagementContext db = new())
            {
                Tasks = db.Tasks.Where(e => e.Executor == CurrentEmployee.currentEmployee.Id && e.Status != 3).ToList();
            }
        }
        private bool CanEditTaskProjectCommandExecute(object p) => p is Task;
        #endregion


        public MainPageViewModel()
        {
            OpenInfoProjectCommand = new RelayCommand(OnOpenInfoProjectCommandExecuted, CanOpenInfoProjectCommandExecute);
            EditTaskProjectCommand = new RelayCommand(OnEditTaskProjectCommandExecuted, CanEditTaskProjectCommandExecute);

            HelloText = $"Добро пожаловать, {CurrentEmployee.currentEmployee.Surname} {CurrentEmployee.currentEmployee.Name} {CurrentEmployee.currentEmployee.Patronymic}!";

            using (ProjectManagementContext db = new ())
            {
                Projects = db.Projects.Where(e => e.Employees.Contains(CurrentEmployee.currentEmployee)).ToList();
                Tasks = db.Tasks.Where(e => e.Executor == CurrentEmployee.currentEmployee.Id && e.Status != 3).ToList();
            }
        }
    }
}
