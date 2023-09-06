using Microsoft.EntityFrameworkCore;
using Microsoft.Office.Interop.Excel;
using ProjectManagement.Infrastructure.Classes;
using ProjectManagement.Infrastructure.Commands;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ProjectManagement.ViewModels
{
    class EditTaskWindowViewModel: ViewModel
    {
        private Task _editTask;
        public Task EditTask
        {
            get => _editTask;
            set => Set(ref _editTask, value);
        }

        private String _dateStart;
        public String DateStart
        {
            get => _dateStart;
            set => Set(ref _dateStart, value);
        }

        private String _empExecutor;
        public String EmpExecutor
        {
            get => _empExecutor;
            set => Set(ref _empExecutor, value);
        }

        private String _empResponsible;
        public String EmpResponsible
        {
            get => _empResponsible;
            set => Set(ref _empResponsible, value);
        }

        private Visibility _visibilityBtnWork;
        public Visibility VisibilityBtnWork
        {
            get => _visibilityBtnWork;
            set => Set(ref _visibilityBtnWork, value);
        }

        private Visibility _visibilityBtnComplit;
        public Visibility VisibilityBtnComplit
        {
            get => _visibilityBtnComplit;
            set => Set(ref _visibilityBtnComplit, value);
        }

        private Visibility _visibilityMess;
        public Visibility VisibilityMess
        {
            get => _visibilityMess;
            set => Set(ref _visibilityMess, value);
        }

        private String _textMess = "";
        public String TextMess
        {
            get => _textMess;
            set => Set(ref _textMess, value);
        }

        private List<Message> _listMessages = new ();
        public List<Message> ListMessages
        {
            get => _listMessages;
            set => Set(ref _listMessages, value);
        }

        #region AddMessageCommand - новое сообщение
        public ICommand AddMessageCommand { get; }
        private void OnAddMessageCommandExecuted(object p)
        {
            using(ProjectManagementContext db = new ())
            {
                db.Messages.Load();
                db.Employees.Load();

                Message message = new ()
                {
                    Text = TextMess,
                    Task = ChangingTask.task!.Id,
                    Employee = CurrentEmployee.currentEmployee.Id,
                    Id = (!db.Messages.Any()) ? 1 : db.Messages.Max(e => e.Id) + 1
                };
                db.Messages.Add(message);
                int n = 1;
                foreach(Employee emp in db.Projects.Include(e => e.Employees).Single(e => e.Id == ChangingProject.project.Id).Employees)
                {
                    db.Notifications.Add(new Notification()
                    {
                        Employee = emp.Id,
                        Id = (db.Notifications.Any()) ? db.Notifications.Max(e => e.Id) + n : n,
                        Text = "Новое сообщение \"" + TextMess +"\" в задаче \"" + ChangingTask.task.Name + "\" в проект " + ChangingProject.project.Name,
                        Date = DateOnly.FromDateTime(DateTime.Now),
                        New = true
                    });
                    n++;
                }
                db.SaveChanges();
                TextMess = "";
                ListMessages = db.Tasks.Single(e => e.Id == EditTask.Id).Messages.ToList();
            }
        }
        private bool CanAddMessageCommandExecute(object p) => TextMess != "";
        #endregion

        #region WorkCommand - взять в работу задачу
        public ICommand WorkCommand { get; }
        private void OnWorkCommandExecuted(object p)
        {
            using (ProjectManagementContext db = new ())
            {
                db.Tasks.Single(e => e.Id == EditTask.Id).Status = 2;
                db.SaveChanges();
            }
            EditTask.Status = 2;
            if (EditTask.Executor == CurrentEmployee.currentEmployee.Id)
                switch (EditTask.Status)
                {
                    case 1:
                        {
                            VisibilityBtnComplit = VisibilityBtnWork = Visibility.Visible;
                            VisibilityMess = Visibility.Collapsed;
                        }
                        break;
                    case 2:
                        {
                            VisibilityBtnComplit = Visibility.Visible;
                            VisibilityMess = VisibilityBtnWork = Visibility.Collapsed;
                        }
                        break;
                    default:
                        {
                            VisibilityMess = Visibility.Visible;
                            VisibilityBtnComplit = VisibilityBtnWork = Visibility.Collapsed;
                        }
                        break;
                }
            else VisibilityMess = VisibilityBtnComplit = VisibilityBtnWork = Visibility.Collapsed;
        }
        private bool CanWorkCommandExecute(object p) => EditTask.Status == 1;
        #endregion

        #region CompletCommand - завершить задачу
        public ICommand CompletCommand { get; }
        private void OnCompletCommandExecuted(object p)
        {
            using(ProjectManagementContext db = new ProjectManagementContext())
            {
                db.Tasks.Single(e => e.Id == EditTask.Id).Status = 3;
                db.SaveChanges();
            }
            EditTask.Status = 3;
            if (EditTask.Executor == CurrentEmployee.currentEmployee.Id)
                switch (EditTask.Status)
            {
                case 1:
                    {
                        VisibilityBtnComplit = VisibilityBtnWork = Visibility.Visible;
                        VisibilityMess = Visibility.Collapsed;
                    }
                    break;
                case 2:
                    {
                        VisibilityBtnComplit = Visibility.Visible;
                        VisibilityMess = VisibilityBtnWork = Visibility.Collapsed;
                    }
                    break;
                default:
                    {
                        VisibilityMess = Visibility.Visible;
                        VisibilityBtnComplit = VisibilityBtnWork = Visibility.Collapsed;
                    }
                    break;
            }
            else VisibilityMess = VisibilityBtnComplit = VisibilityBtnWork = Visibility.Collapsed;
        }
        private bool CanCompletCommandExecute(object p) => EditTask.Status == 2;
        #endregion

        public EditTaskWindowViewModel()
        {
            WorkCommand = new RelayCommand(OnWorkCommandExecuted, CanWorkCommandExecute);
            CompletCommand = new RelayCommand(OnCompletCommandExecuted, CanCompletCommandExecute);
            AddMessageCommand = new RelayCommand(OnAddMessageCommandExecuted, CanAddMessageCommandExecute);


            using (ProjectManagementContext db = new ())
            {
                db.Messages.Load();
                db.Employees.Load();
                db.Phases.Load();

                EditTask = db.Tasks.Single(e => e.Id == ChangingTask.task!.Id);
                ListMessages = EditTask.Messages.ToList();
                DateStart = (EditTask.DependentTasks.Count != 0) ?  FerialDays.AddDay(EditTask.DependentTasks.Max(e => e.DateComplet), 1).ToString() : db.Projects.Where(e => e.Phases.Contains(EditTask.PhaseNavigation)).Single().StartDate.ToString();
                EmpExecutor = $"{EditTask.ExecutorNavigation.Surname} {EditTask.ExecutorNavigation.Name} {EditTask.ExecutorNavigation.Patronymic}";
                EmpResponsible = $"{EditTask.ResponsibleNavigation.Surname} {EditTask.ResponsibleNavigation.Name} {EditTask.ResponsibleNavigation.Patronymic}";

                if (EditTask.Executor == CurrentEmployee.currentEmployee.Id)
                    switch (EditTask.Status)
                    {
                        case 1:
                            {
                                VisibilityBtnComplit = VisibilityBtnWork = Visibility.Visible;
                                VisibilityMess = Visibility.Collapsed;
                            }
                            break;
                        case 2:
                            {
                                VisibilityBtnComplit = Visibility.Visible;
                                VisibilityMess = VisibilityBtnWork = Visibility.Collapsed;
                            }
                            break;
                        default:
                            {
                                VisibilityMess = Visibility.Visible;
                                VisibilityBtnComplit = VisibilityBtnWork = Visibility.Collapsed;
                            }
                            break;
                    }
                else VisibilityMess = VisibilityBtnComplit = VisibilityBtnWork = Visibility.Collapsed;
            }
            
        }
    }
}
