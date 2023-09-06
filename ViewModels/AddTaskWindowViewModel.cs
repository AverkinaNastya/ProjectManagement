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
    class AddTaskWindowViewModel: ViewModel
    {
        private String _name;
        public String Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private String _filterStr = "";
        public String FilterStr
        {
            get => _filterStr;
            set
            {
                Set(ref _filterStr, value);
                using (ProjectManagementContext db = new())
                {
                    Tasks.Clear();
                    if (SelectPhase != null)
                    {
                        db.Tasks.Where(e => e.Phase == SelectPhase.Id && !MainTasks.Contains(e)).ToList().ForEach(Tasks.Add);
                        Tasks.Where(e => !e.Name.StartsWith(FilterStr) && !(LevenshteinDistance.Distance(e.Name, FilterStr) <= 3)).ToList().ForEach(e => Tasks.Remove(e));
                    }
                };
                DropDownOpen = (Tasks.Count != 0);
            }
        }

        private String _comment;
        public String Comment
        {
            get => _comment;
            set => Set(ref _comment, value);
        }

        private String _сountDays;
        public String CountDays
        {
            get => _сountDays;
            set
            {
                Set(ref _сountDays, value);
                if (int.TryParse(value, out int n) && n > 0 && n < 60){
                    _dateEnd = FerialDays.AddDay(_dateStart, n);
                    DateEndStr = _dateEnd.ToString();
                }
                else
                {
                    DateEndStr = "";
                }
                
            }
        }

        private DateOnly _dateStart;
        private String _dateStartStr;
        public String DateStartStr
        {
            get => _dateStartStr;
            set => Set(ref _dateStartStr, value);
        }

        private DateOnly _dateEnd;
        private String _dateEndStr;
        public String DateEndStr
        {
            get => _dateEndStr;
            set => Set(ref _dateEndStr, value);
        }

        private ObservableCollection<Task> _mainTasks = new ();
        public ObservableCollection<Task> MainTasks
        {
            get => _mainTasks;
            set
            {
                Set(ref _mainTasks, value);

            }
        }

        private ObservableCollection<Phase> _phases = new();
        public ObservableCollection<Phase> Phases
        {
            get => _phases;
            set => Set(ref _phases, value);
        }

        private Phase? _selectPhase;
        public Phase? SelectPhase
        {
            get => _selectPhase;
            set
            {
                Set(ref _selectPhase, value);
                using (ProjectManagementContext db = new())
                {
                    Tasks.Clear();
                    if (SelectPhase != null)
                    {
                        db.Tasks.Where(e => e.Phase == SelectPhase.Id && !MainTasks.Contains(e)).ToList().ForEach(Tasks.Add);
                        Tasks.Where(e => !e.Name.StartsWith(FilterStr) && !(LevenshteinDistance.Distance(e.Name, FilterStr) <= 3)).OrderByDescending(e => e.DateComplet).ToList().ForEach(e => Tasks.Remove(e));

                    }
                    MainTasks.Clear();
                }
            }
        }



        private ObservableCollection<Task> _tasks = new();
        public ObservableCollection<Task> Tasks
        {
            get => _tasks;
            set => Set(ref _tasks, value);
        }

        private Task _selectTask;
        public Task SelectTask
        {
            get => _selectTask;
            set 
            { 
                Set(ref _selectTask, value); 
                if (SelectTask != null)
                {
                    MainTasks.Add(SelectTask);
                }
                using (ProjectManagementContext db = new())
                {
                    Tasks.Clear();
                    if (SelectPhase != null)
                    {
                        db.Tasks.Where(e => e.Phase == SelectPhase.Id && !MainTasks.Contains(e)).ToList().ForEach(Tasks.Add);
                        Tasks.Where(e => !e.Name.StartsWith(FilterStr) && !(LevenshteinDistance.Distance(e.Name, FilterStr) <= 3)).OrderByDescending(e => e.DateComplet).ToList().ForEach(e => Tasks.Remove(e));

                    }
                    if (MainTasks.Count != 0)
                    {
                        _dateStart = FerialDays.AddDay(MainTasks.OrderByDescending(e => e.DateComplet).First().DateComplet, 1);
                        DateStartStr = _dateStart.ToString();
                    }
                    else
                    {
                        _dateStart = ChangingProject.project.StartDate;
                        DateStartStr = _dateStart.ToString();
                    }
                    if (int.TryParse(CountDays, out int n) && n > 0 && n < 60)
                    {
                        _dateEnd = FerialDays.AddDay(_dateStart, n);
                        DateEndStr = _dateEnd.ToString();
                    }
                    else
                    {
                        DateEndStr = "";
                    }
                    _selectTask = null!;
                };
            }
        }

        private Task? _selectMainTask;
        public Task? SelectMainTask
        {
            get => _selectMainTask;
            set => Set(ref _selectMainTask, value);
        }

        private bool _dropDownOpen;
        public bool DropDownOpen
        {
            get => _dropDownOpen;
            set => Set(ref _dropDownOpen, value);
        }

        private Employee ResponsibEmployee;
        private String _responsib;
        public String Responsib 
        { 
            get => _responsib; 
            set => Set(ref _responsib, value); 
        }

        private Employee ExecutorEmployee;
        private String _executor;
        public String Executor
        { 
            get => _executor; 
            set => Set(ref _executor, value); 
        }

        #region Команды
        #region AddResponsibCommand - добавление контролирующего выполнение
        public ICommand AddResponsibCommand { get; }
        private void OnAddResponsibCommandExecuted(object p)
        {
            App.Services.GetRequiredService<IUserDialog>().OpenAddEmployeeTaskWindow();
            if (ChangingEmployee.changingEmployee != null)
            {
                using (ProjectManagementContext db = new())
                {
                    ResponsibEmployee = db.Employees.Single(e => e.Id == ChangingEmployee.changingEmployee.Id);
                    Responsib = ResponsibEmployee.Surname + " " + ResponsibEmployee.Name + " " + ResponsibEmployee.Patronymic;
                    ChangingEmployee.changingEmployee = null!;
                }
            }
        }
        private bool CanAddResponsibCommandExecute(object p) => true;
        #endregion

        #region AddExecutorCommand - добавление ответсвенного
        public ICommand AddExecutorCommand { get; }
        private void OnAddExecutorCommandExecuted(object p)
        {
            App.Services.GetRequiredService<IUserDialog>().OpenAddEmployeeTaskWindow();
            if (ChangingEmployee.changingEmployee != null)
            {
                using (ProjectManagementContext db = new())
                {
                    ExecutorEmployee = db.Employees.Single(e => e.Id == ChangingEmployee.changingEmployee.Id);
                    Executor = ExecutorEmployee.Surname + " " + ExecutorEmployee.Name + " " + ExecutorEmployee.Patronymic;
                    ChangingEmployee.changingEmployee = null!;
                }
            }
        }
        private bool CanAddExecutorCommandExecute(object p) => true;
        #endregion

        #region DeleteMainTaskCommand - удаление связанной задачи
        public ICommand DeleteMainTaskCommand { get; }
        private void OnDeleteMainTaskCommandExecuted(object p)
        {
            if (MainTasks.Contains(SelectMainTask))
            {
                MainTasks.Remove(SelectMainTask);
                using (ProjectManagementContext db = new())
                {
                    Tasks.Clear();
                    if (SelectPhase != null)
                    {
                        db.Tasks.Where(e => e.Phase == SelectPhase.Id && !MainTasks.Contains(e)).ToList().ForEach(Tasks.Add);
                        Tasks.Where(e => !e.Name.StartsWith(FilterStr) && !(LevenshteinDistance.Distance(e.Name, FilterStr) <= 3)).OrderByDescending(e => e.DateComplet).ToList().ForEach(e => Tasks.Remove(e));

                    }
                    if (MainTasks.Count != 0)
                    {
                        _dateStart = FerialDays.AddDay(MainTasks.OrderByDescending(e => e.DateComplet).First().DateComplet, 1);
                        DateStartStr = _dateStart.ToString();
                    }
                    else
                    {
                        _dateStart = ChangingProject.project.StartDate;
                        DateStartStr = _dateStart.ToString();
                    }
                    if (int.TryParse(CountDays, out int n) && n > 0 && n < 60)
                    {
                        _dateEnd = FerialDays.AddDay(_dateStart, n);
                        DateEndStr = _dateEnd.ToString();
                    }
                    else
                    {
                        DateEndStr = "";
                    }
                    _selectTask = null!;
                };

            }
        }
        private bool CanDeleteMainTaskCommandExecute(object p) => SelectMainTask != null;
        #endregion

        #region AddTaskCommand - создание задачи
        public ICommand AddTaskCommand { get; }
        private void OnAddTaskCommandExecuted(object p)
        {
            try
                {
                using(ProjectManagementContext db = new())
                {
                    if (db.Phases.Include(e => e.Tasks).Single(e => e.Id == SelectPhase.Id).Tasks.FirstOrDefault(e => e.Name == Name) == null)
                    {
                        Task task = new ()
                        {
                            Id = (!db.Tasks.Any()) ? 1 : db.Tasks.Max(e => e.Id) + 1,
                            Name = Name,
                            DateComplet = _dateEnd,
                            Comment = Comment,
                            Executor = ExecutorEmployee.Id,
                            Responsible = ResponsibEmployee.Id,
                            Phase = SelectPhase.Id
                        };
                        db.Tasks.Where(e => MainTasks.Contains(e)).ToList().ForEach(e => e.DependentTasks.Add(task));
                        db.Tasks.Add(task);
                        db.Notifications.Add(new Notification()
                        {
                            Employee = ExecutorEmployee.Id,
                            Id = (db.Notifications.Any()) ? db.Notifications.Max(e => e.Id) + 1 : 1,
                            Text = "Вам поставлена новая задача \"" + Name + "\" в проект " + ChangingProject.project.Name,
                            Date = DateOnly.FromDateTime(DateTime.Now),
                            New = true
                        });
                        db.Notifications.Add(new Notification()
                        {
                            Employee = ResponsibEmployee.Id,
                            Id = (db.Notifications.Any()) ? db.Notifications.Max(e => e.Id) + 2 : 1,
                            Text = "Вы назначены ответсвенным за выполнение задачи \"" + Name + "\" в проект " + ChangingProject.project.Name,
                            Date = DateOnly.FromDateTime(DateTime.Now),
                            New = true
                        });
                        db.SaveChanges();
                        MessageBox.Show("Задача успешно добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        OnDialogComplete(EventArgs.Empty);
                    }
                    else
                    {
                        MessageBox.Show("Задача с таким именем уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private bool CanAddTaskCommandExecute(object p) => Name != null && Name != "" && SelectPhase != null && ExecutorEmployee != null && ResponsibEmployee != null &&
                                                           Executor != null && Responsib != null && DateEndStr != "" && DateStartStr != "" && DateEndStr != null && DateStartStr != null;
        #endregion

        #region BackCommand - отмена
        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object p) => OnDialogComplete(EventArgs.Empty);
        private bool CanBackCommandExecute(object p) => true;
        #endregion
        #endregion

        public AddTaskWindowViewModel()
        {
            AddExecutorCommand = new RelayCommand(OnAddExecutorCommandExecuted, CanAddExecutorCommandExecute);
            AddResponsibCommand = new RelayCommand(OnAddResponsibCommandExecuted, CanAddResponsibCommandExecute);
            DeleteMainTaskCommand = new RelayCommand(OnDeleteMainTaskCommandExecuted, CanDeleteMainTaskCommandExecute);
            AddTaskCommand = new RelayCommand(OnAddTaskCommandExecuted, CanAddTaskCommandExecute);
            BackCommand = new RelayCommand(OnBackCommandExecuted, CanBackCommandExecute);

            _dateStart = ChangingProject.project.StartDate;
            DateStartStr = _dateStart.ToString();

            using (ProjectManagementContext db = new ())
            {
                db.Projects.Include(e => e.Phases).Single(e => e.Id == ChangingProject.project.Id).Phases.ToList().ForEach(Phases.Add);
                SelectPhase = Phases.FirstOrDefault();
                
            }
        }
    }
}
