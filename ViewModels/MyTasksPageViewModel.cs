using GongSolutions.Wpf.DragDrop;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Services;
using System.Windows.Input;
using ProjectManagement.Infrastructure.Commands;
using System.Collections.ObjectModel;

namespace ProjectManagement.ViewModels
{
    class MyTasksPageViewModel: ViewModel, IDropTarget
    {
        #region Реализация IDropTarget для DragAndDrop задач на канбан
        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo != null && dropInfo.Data is Task task && dropInfo.VisualTarget is ListBox list)
            {
                if (task.Status == 1 && (list.Name == "ToDo" || list.Name == "Done") && (task.Executor == CurrentEmployee.currentEmployee.Id) ||
                    task.Status == 2 && (list.Name == "Done") && (task.Executor == CurrentEmployee.currentEmployee.Id))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Move;
                }
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo != null && dropInfo.Data is Task task && dropInfo.VisualTarget is ListBox list)
            {
                using (ProjectManagementContext db = new())
                {
                    db.Tasks.Load();
                    db.Employees.Load();
                    if (list.Name == "ToDo")
                    {
                        db.Tasks.Single(e => e.Id == task.Id).Status = 2;
                    }
                    else if (list.Name == "Done")
                    {
                        db.Tasks.Single(e => e.Id == task.Id).Status = 3;
                    }
                    db.SaveChanges();
                    db.Tasks.Load();
                    db.Employees.Load();
                    db.Projects.Load();
                    db.Phases.Load();
                    db.Employees.Load();
                    Tasks.Clear();
                    if (ToggleResponsible)
                        Tasks = db.Tasks.Where(e => e.Responsible == CurrentEmployee.currentEmployee.Id).ToList();
                    else Tasks = db.Tasks.Where(e => e.Executor == CurrentEmployee.currentEmployee.Id).ToList();
                }
            }
        }
        #endregion

        private List<Task> _tasks = new ();
        public List<Task> Tasks
        {
            get => _tasks;
            set => Set(ref _tasks, value);
        }

        private bool _toggleExecutor = true;
        public bool ToggleExecutor
        {
            get => _toggleExecutor;
            set {
                Set(ref _toggleExecutor, value);
                _toggleResponsible = !ToggleExecutor;
                if (value)
                {
                    using (ProjectManagementContext db = new())
                    {
                        db.Projects.Load();
                        db.Phases.Load();
                        db.Employees.Load();
                        Tasks = db.Tasks.Where(e => e.Executor == CurrentEmployee.currentEmployee.Id).ToList();
                    }
                }
            }
        }

        private bool _toggleResponsible = false;
        public bool ToggleResponsible
        {
            get => _toggleResponsible;
            set
            {
                Set(ref _toggleResponsible, value);
                _toggleExecutor = !ToggleResponsible;
                if (value)
                {
                    using (ProjectManagementContext db = new())
                    {
                        db.Projects.Load();
                        db.Phases.Load();
                        db.Employees.Load();
                        Tasks = db.Tasks.Where(e => e.Responsible == CurrentEmployee.currentEmployee.Id).ToList();
                    }
                }
            }
        }

        #region EditTaskProjectCommand - изменение задачи
        public ICommand EditTaskProjectCommand { get; }
        private void OnEditTaskProjectCommandExecuted(object p)
        {
            ChangingTask.task = p as Task;
            App.Services.GetRequiredService<IUserDialog>().OpenEditTaskWindow();
            ChangingTask.task = null;
            using (ProjectManagementContext db = new())
            {
                db.Tasks.Load();
                db.Employees.Load();
                db.Projects.Load();
                db.Phases.Load();
                db.Employees.Load();
                Tasks.Clear();
                if (ToggleResponsible)
                    Tasks = db.Tasks.Where(e => e.Responsible == CurrentEmployee.currentEmployee.Id).ToList();
                else Tasks = db.Tasks.Where(e => e.Executor == CurrentEmployee.currentEmployee.Id).ToList();
            }
        }
        private bool CanEditTaskProjectCommandExecute(object p) => p is Task;
        #endregion


        public MyTasksPageViewModel()
        {
            EditTaskProjectCommand = new RelayCommand(OnEditTaskProjectCommandExecuted, CanEditTaskProjectCommandExecute);
            using (ProjectManagementContext db = new ())
            {
                db.Projects.Load();
                db.Phases.Load();
                db.Employees.Load();
                Tasks = db.Tasks.Where(e => e.Executor == CurrentEmployee.currentEmployee.Id).ToList();
            }
        }
    }
}
