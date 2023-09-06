using Microsoft.EntityFrameworkCore;
using ProjectManagement.Infrastructure.Classes;
using ProjectManagement.Infrastructure.Commands;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ProjectManagement.ViewModels
{
    class AddProjectWindowViewModel: ViewModel
    {
        private String _name;
        public String Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private DateOnly _dateOnly;
        private String _date;
        public String Date
        {
            get => _date;
            set => Set(ref _date, value);
        }


        private String _comment;
        public String Comment
        {
            get => _comment;
            set => Set(ref _comment, value);
        }

        private String _filterStr = "";
        public String FilterStr
        {
            get => _filterStr;
            set {
                Set(ref _filterStr, value); 
                
                using(ProjectManagementContext db = new())
                {
                    Employees = db.Employees.Where(e => !e.Blocked && !e.New.Value).ToList();
                    Employees = Employees.Where(e => (e.Name.StartsWith(FilterStr) || LevenshteinDistance.Distance(e.Name, FilterStr) <= 3 ||
                                                      e.Surname.StartsWith(FilterStr) || LevenshteinDistance.Distance(e.Surname, FilterStr) <= 3 ||
                                                      (e.Patronymic != null && (e.Patronymic.StartsWith(FilterStr) || LevenshteinDistance.Distance(e.Patronymic, FilterStr) <= 3))) && !EmployeesListBox.Contains(e)).ToList();
                    DropDownOpen = !(Employees.Count == 0);
                }
            }
        }

        private bool _dropDownOpen = false;
        public bool DropDownOpen
        {
            get => _dropDownOpen;
            set => Set(ref _dropDownOpen, value);
        }

        private Employee? _selectEmployee = null;
        public Employee? SelectEmployee
        {
            get => _selectEmployee;
            set {
                Set(ref _selectEmployee, value);
                if (SelectEmployee != null)
                {
                    EmployeesListBox.Add(SelectEmployee);
                }
                _selectEmployee = null;
                using (ProjectManagementContext db = new())
                {
                    Employees = db.Employees.Where(e => !e.Blocked && !e.New.Value).ToList();
                    Employees = Employees.Where(e => (e.Name.StartsWith(FilterStr) || LevenshteinDistance.Distance(e.Name, FilterStr) <= 3 ||
                                                      e.Surname.StartsWith(FilterStr) || LevenshteinDistance.Distance(e.Surname, FilterStr) <= 3 ||
                                                      (e.Patronymic != null && (e.Patronymic.StartsWith(FilterStr) || LevenshteinDistance.Distance(e.Patronymic, FilterStr) <= 3))) && !EmployeesListBox.Where(i => i.Id == e.Id).Any()).ToList();
                }
            }
        }

        private List<Employee> _employees = new();
        public List<Employee> Employees
        {
            get => _employees;
            set => Set(ref _employees, value);
        }

        private ObservableCollection<Employee> _employeesListBox = new();
        public ObservableCollection<Employee> EmployeesListBox
        {
            get => _employeesListBox;
            set => Set(ref _employeesListBox, value);
        }

        private Employee? _selectedEmployeesListBox;
        public Employee? SelectedEmployeesListBox
        {
            get => _selectedEmployeesListBox;
            set => Set(ref _selectedEmployeesListBox, value);
        }


        #region Команды

        #region DeleteSelectEmployeesCommand - команда удаления сотрудников
        public ICommand DeleteSelectEmployeesCommand { get; }
        private void OnDeleteSelectEmployeesCommandExecuted(object p) {
            if (SelectedEmployeesListBox != null)
                EmployeesListBox.Remove(SelectedEmployeesListBox);
            using (ProjectManagementContext db = new())
            {
                Employees = db.Employees.Where(e => !e.Blocked && !e.New.Value).ToList();
                Employees = Employees.Where(e => (e.Name.StartsWith(FilterStr) || LevenshteinDistance.Distance(e.Name, FilterStr) <= 3 ||
                                                  e.Surname.StartsWith(FilterStr) || LevenshteinDistance.Distance(e.Surname, FilterStr) <= 3 ||
                                                  (e.Patronymic != null && (e.Patronymic.StartsWith(FilterStr) || LevenshteinDistance.Distance(e.Patronymic, FilterStr) <= 3)) && !EmployeesListBox.Contains(e))).ToList();
            }
        }
        private bool CanDeleteSelectEmployeesCommandExecute(object p) => SelectedEmployeesListBox != null;
        #endregion

        #region AddProjectCommand - команда создания проекта
        public ICommand AddProjectCommand { get; }
        private void OnAddProjectCommandExecuted(object p)
        {
            try
            {
                using (ProjectManagementContext db = new())
                {
                    db.Employees.Load();
                    if (db.Projects.Where(e => e.Name == Name).FirstOrDefault() == null)
                    {
                        Project project = new Project
                        {
                            Id = (db.Projects.Any()) ? db.Projects.Select(e => e.Id).Max() + 1 : 1,
                            Name = Name,
                            StartDate = _dateOnly,
                            Comment = Comment
                        };
                        int n = db.Notifications.Max(e => e.Id);
                        foreach (Employee emp in EmployeesListBox)
                        {
                            db.Employees.Where(e => e.Id == emp.Id).Single().Projects.Add(project);
                            db.Notifications.Add(new Notification()
                            {
                                Employee = emp.Id,
                                Id = (db.Notifications.Any()) ? n + 1 : 1,
                                Text = "Вы были добавлены в проект " + Name,
                                Date = DateOnly.FromDateTime(DateTime.Now),
                                New = true
                            });
                            n++;
                        }
                        db.Projects.Add(project);
                        db.SaveChanges();
                        MessageBox.Show("Новый проект успешно создан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        OnDialogComplete(EventArgs.Empty);
                    }
                    else
                    {
                        MessageBox.Show("Проект с таким именем уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private bool CanAddProjectCommandExecute(object p) => DateOnly.TryParse(_date, out _dateOnly) && Name != "" && EmployeesListBox != null && EmployeesListBox.Count != 0;
        #endregion

        #region ClickFilterCommand - обновление фильтра
        public ICommand ClickFilterCommand { get; }
        private void OnClickFilterCommandExecuted(object p) => DropDownOpen = !(Employees.Count == 0);
        private bool CanClickFilterCommandExecute(object p) => true;
        #endregion

        #region BackCommand - команда отмены
        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object p) => OnDialogComplete(EventArgs.Empty);
        private bool CanBackCommandExecute(object p) => true;
        #endregion

        #endregion

        public AddProjectWindowViewModel()
        {
            DeleteSelectEmployeesCommand = new RelayCommand(OnDeleteSelectEmployeesCommandExecuted, CanDeleteSelectEmployeesCommandExecute);
            AddProjectCommand = new RelayCommand(OnAddProjectCommandExecuted, CanAddProjectCommandExecute);
            ClickFilterCommand = new RelayCommand(OnClickFilterCommandExecuted, CanClickFilterCommandExecute);
            BackCommand = new RelayCommand(OnBackCommandExecuted, CanBackCommandExecute);
            using(ProjectManagementContext db = new())
            {
                Employees = db.Employees.Where(e => !e.Blocked && !e.New.Value).ToList();
            }
        }
    }
}
