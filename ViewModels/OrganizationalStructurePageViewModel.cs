using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Infrastructure.Classes;
using ProjectManagement.Infrastructure.Commands;
using ProjectManagement.Models;
using ProjectManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ProjectManagement.ViewModels
{
    internal class OrganizationalStructurePageViewModel: ViewModel
    {
        private readonly bool _adminCurrent = CurrentEmployee.currentEmployee.Admin;
        public Visibility AdminCurrent
        {
            get => _adminCurrent ? Visibility.Visible : Visibility.Collapsed;
        }

        private List<Employee> _employees = new ();
        public List<Employee> Employees
        {
            get => _employees;
            set => Set(ref _employees, value);
        }

        private String _filterStr = "";
        public String FilterStr
        {
            get => _filterStr;
            set { Set(ref _filterStr, value);
                using (ProjectManagementContext db = new())
                {
                    db.Posts.Load();
                    db.Departments.Load();
                    Employees = db.Employees.Include(e => e.Projects).Where(e => !e.Blocked && !e.New.Value).ToList();
                    Employees = Employees.Where(e => (e.Name.StartsWith(FilterStr) || LevenshteinDistance.Distance(e.Name, FilterStr) <= 3 ||
                                                     e.Surname.StartsWith(FilterStr) || LevenshteinDistance.Distance(e.Surname, FilterStr) <= 3 ||
                                                     (e.Patronymic != null && (e.Patronymic.StartsWith(FilterStr) || LevenshteinDistance.Distance(e.Patronymic, FilterStr) <= 3)) ||
                                                     e.PostNavigation.DepartmentNavigation.Name.StartsWith(FilterStr) || LevenshteinDistance.Distance(e.PostNavigation.DepartmentNavigation.Name, FilterStr) <= 3)).ToList();
                    SelectEmployee = Employees.FirstOrDefault();
                }
            }
        }

        private Employee? _selectEmployee;
        public Employee? SelectEmployee
        {
            get => _selectEmployee;
            set => Set(ref _selectEmployee, value);
        }

        private List<Department> _departments = new ();
        public List<Department> Departments
        {
            get => _departments;
            set
            {
                Set(ref _departments, value);
                SelectDepartament = Departments.FirstOrDefault();
            }
        }

        private Department? _selectDepartament;
        public Department? SelectDepartament
        {
            get => _selectDepartament;
            set {
                Set(ref _selectDepartament, value);
                if (SelectDepartament != null)
                    SelectPost = SelectDepartament.Posts.FirstOrDefault();
            }
        }

        private Post? _selectPost;
        public Post? SelectPost
        {
            get => _selectPost;
            set
            {
                Set(ref _selectPost, value);
                if (SelectPost != null)
                    SelectPostEmployees = SelectPost.Employees.Where(e => !e.Blocked && !e.New.Value).ToList();
            }
        }

        private List<Employee> _selectPostEmployees;
        public List<Employee> SelectPostEmployees
        {
            get => _selectPostEmployees;
            set => Set(ref _selectPostEmployees, value);
        }

        #region Команды

        #region AddEmployeeSystemCommand - создание нового сотрудника
        public ICommand AddEmployeeSystemCommand { get; }
        private void OnAddEmployeeCommandExecuted(object p)
        {
            App.Services.GetRequiredService<IUserDialog>().OpenAddEmployeeWindow();
            using (ProjectManagementContext db = new ())
            {
                db.Departments.Load();
                db.Posts.Load();
                db.Projects.Load();
                db.Employees.Load();
                Employees = db.Employees.Include(e => e.Projects).Where(e => !e.Blocked && !e.New.Value).ToList();
                SelectEmployee = Employees.FirstOrDefault();
            }
        }
        private bool CanAddEmployeeCommandExecute(object p) => true;
        #endregion

        #region EditEmployeeSystemCommand - изменение сотрудника
        public ICommand EditEmployeeSystemCommand { get; }
        private void OnEditEmployeeCommandExecuted(object p)
        {
            ChangingEmployee.changingEmployee = SelectEmployee;
            App.Services.GetRequiredService<IUserDialog>().OpenEditEmployeeWindow();
            using (ProjectManagementContext db = new())
            {
                db.Departments.Load();
                db.Projects.Load();
                db.Posts.Load();
                db.Employees.Load();
                Employees = db.Employees.Include(e => e.Projects).Where(e => !e.Blocked && !e.New.Value).ToList();
                SelectEmployee = Employees.FirstOrDefault();
            }
        }
        private bool CanEditEmployeeCommandExecute(object p) => SelectEmployee != null;
        #endregion

        #region BlockEmployeeSystemCommand - создание нового сотрудника
        public ICommand BlockEmployeeSystemCommand { get; }
        private void OnBlockEmployeeCommandExecuted(object p)
        {
            if (MessageBox.Show($"Вы уверены, что хотите заблокировать пользователя {SelectEmployee.Surname} {SelectEmployee.Name} {SelectEmployee.Patronymic}? Данное действие нельзя будет отменить",
                "Подтверждение", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation) == MessageBoxResult.OK)
            {
                using (ProjectManagementContext db = new ())
                {
                    Employee employee = db.Employees.Where(e => e.Id == SelectEmployee.Id).First();
                    employee.Blocked = true;
                    db.SaveChanges();
                    db.Departments.Load();
                    db.Posts.Load();
                    db.Employees.Load();
                    Employees = db.Employees.Where(e => !e.Blocked && !e.New.Value).ToList();
                    SelectEmployee = Employees.FirstOrDefault();
                }
            }
        }
        private bool CanBlockEmployeeCommandExecute(object p) => SelectEmployee != null && SelectEmployee.Id != CurrentEmployee.currentEmployee.Id;
        #endregion

        #region AddDepartamentSystemCommand - создание нового департамента
        public ICommand AddDepartamentSystemCommand { get; }
        private void OnAddDepartamentCommandExecuted(object p) {
            App.Services.GetRequiredService<IUserDialog>().OpenAddDepartmentWindow();
            using(ProjectManagementContext db = new ProjectManagementContext())
            {
                db.Posts.Load();
                db.Employees.Load();
                Departments = db.Departments.ToList();
                SelectDepartament = Departments.FirstOrDefault();
            }
        }
        private bool CanAddDepartamentCommandExecute(object p) => true;
        #endregion

        #region EditDepartamentSystemCommand - изменение департамента
        public ICommand EditDepartamentSystemCommand { get; }
        private void OnEditDepartamentCommandExecuted(object p)
        {
            ChangingDepartment.changingDepartment = SelectDepartament;
            App.Services.GetRequiredService<IUserDialog>().OpenEditDepartmentWindow();
            using (ProjectManagementContext db = new ProjectManagementContext())
            {
                db.Posts.Load();
                db.Employees.Load();
                Departments = db.Departments.ToList();
                SelectDepartament = Departments.FirstOrDefault();
            }
        }
        private bool CanEditDepartamentCommandExecute(object p) => SelectDepartament != null;
        #endregion

        #region DeleteDepartamentCommand - удаление департамента
        public ICommand DeleteDepartamentCommand { get; }
        private void OnDeleteDepartamentCommandExecuted(object p) {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить департамент " + SelectDepartament.Name + "?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (ProjectManagementContext db = new ProjectManagementContext())
                    {
                        db.Posts.Load();
                        db.Employees.Load();
                        if(db.Departments.Where(e => e.Id == SelectDepartament.Id).FirstOrDefault() != null)
                        {
                            Department dep = db.Departments.Where(e => e.Id == SelectDepartament.Id).First();
                            if (dep.Posts.Count == 0)
                            {
                                db.Departments.Remove(dep);
                                db.SaveChanges();
                            }
                            else
                            {
                                if (dep.Posts.All(e => e.Employees.Count == 0))
                                {
                                    db.Posts.RemoveRange(dep.Posts);
                                    db.Departments.Remove(dep);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    MessageBox.Show("Нельзая удалить департамент с сотрудниками", "Ошибка",
                                        MessageBoxButton.OK, MessageBoxImage.Warning);
                                }
                            }
                        }
                        db.Posts.Load();
                        db.Employees.Load();
                        Departments = db.Departments.ToList();
                        SelectDepartament = Departments.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CanDeleteDepartamentCommandExecute(object p) => SelectDepartament != null;
        #endregion

        #region AddPostSystemCommand - создание новой должности
        public ICommand AddPostSystemCommand { get; }
        private void OnAddPostCommandExecuted(object p)
        {
            ChangingDepartment.changingDepartment = SelectDepartament;
            App.Services.GetRequiredService<IUserDialog>().OpenAddPostWindow();
            using (ProjectManagementContext db = new ProjectManagementContext())
            {
                db.Posts.Load();
                db.Employees.Load();
                Departments = db.Departments.ToList();
                SelectDepartament = Departments.FirstOrDefault();
            }
        }
        private bool CanAddPostCommandExecute(object p) => true;
        #endregion

        #region EditPostCommand - изменение должности
        public ICommand EditPostCommand { get; }
        private void OnEditPostCommandExecuted(object p)
        {
            ChangingPost.changingPost = SelectPost;
            App.Services.GetRequiredService<IUserDialog>().OpenEditPostWindow();
            using (ProjectManagementContext db = new ProjectManagementContext())
            {
                db.Posts.Load();
                db.Employees.Load();
                Departments = db.Departments.ToList();
                SelectDepartament = Departments.FirstOrDefault();
            }
        }
        private bool CanEditPostCommandExecute(object p) => SelectPost != null;
        #endregion

        #region DeletePostCommand - удаление должности
        public ICommand DeletePostCommand { get; }
        private void OnDeletePostCommandExecuted(object p)
        {
            try
            {
                if (MessageBox.Show("Вы уверены, что хотите удалить должность " + SelectPost.Name + "?", "Подтверждение",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (ProjectManagementContext db = new ProjectManagementContext())
                    {
                        if (db.Posts.Where(e => e.Id == SelectPost.Id).FirstOrDefault() != null)
                        {
                            Post post = db.Posts.Where(e => e.Id == SelectPost.Id).First();

                            if (post.Employees.Count == 0)
                            {
                                db.Posts.Remove(post);
                                db.SaveChanges();
                            }
                            else
                            {
                                MessageBox.Show("Нельзя удалить должность с сотрудниками", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        db.Posts.Load();
                        db.Employees.Load();
                        Departments = db.Departments.ToList();
                        SelectDepartament = Departments.FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CanDeletePostCommandExecute(object p) => SelectPost != null;
        #endregion

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
                    db.Departments.Load();
                    db.Posts.Load();
                    db.Projects.Load();
                    db.Employees.Load();
                    Employees = db.Employees.Include(e => e.Projects).Where(e => !e.Blocked && !e.New.Value).ToList();
                    SelectEmployee = Employees.FirstOrDefault();
                }

            }
        }
        private bool CanOpenInfoProjectCommandExecute(object p) => true;
        #endregion


        #endregion

        public OrganizationalStructurePageViewModel()
        {
            AddEmployeeSystemCommand = new RelayCommand(OnAddEmployeeCommandExecuted, CanAddEmployeeCommandExecute);
            EditEmployeeSystemCommand = new RelayCommand(OnEditEmployeeCommandExecuted, CanEditEmployeeCommandExecute);
            BlockEmployeeSystemCommand = new RelayCommand(OnBlockEmployeeCommandExecuted, CanBlockEmployeeCommandExecute);

            AddDepartamentSystemCommand = new RelayCommand(OnAddDepartamentCommandExecuted, CanAddDepartamentCommandExecute);
            DeleteDepartamentCommand = new RelayCommand(OnDeleteDepartamentCommandExecuted, CanDeleteDepartamentCommandExecute);
            EditDepartamentSystemCommand = new RelayCommand(OnEditDepartamentCommandExecuted, CanEditDepartamentCommandExecute);

            AddPostSystemCommand = new RelayCommand(OnAddPostCommandExecuted, CanAddPostCommandExecute);
            EditPostCommand = new RelayCommand(OnEditPostCommandExecuted, CanEditPostCommandExecute);
            DeletePostCommand = new RelayCommand(OnDeletePostCommandExecuted, CanDeletePostCommandExecute);

            OpenInfoProjectCommand = new RelayCommand(OnOpenInfoProjectCommandExecuted, CanOpenInfoProjectCommandExecute);

            using (ProjectManagementContext db = new())
            {
                db.Posts.Load();
                db.Projects.Load();
                db.Departments.Load();
                Employees = db.Employees.Include(e => e.Projects).Where(e => !e.Blocked && !e.New.Value).ToList();
                SelectEmployee = Employees.FirstOrDefault();

                Departments = db.Departments.ToList();
                SelectDepartament = Departments.FirstOrDefault();
            }
        }
    }
}
