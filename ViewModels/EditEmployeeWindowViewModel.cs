using Microsoft.EntityFrameworkCore;
using ProjectManagement.Infrastructure.Commands;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ProjectManagement.ViewModels
{
    class EditEmployeeWindowViewModel: ViewModel
    {
        private Employee _employee;
        public Employee Employee
        {
            get => _employee;
            set => Set(ref _employee, value);
        }

        private String _Login = "";
        public String Login
        {
            get => _Login;
            set => Set(ref _Login, value);
        }

        private List<Department> _DepartmentList = new();
        public List<Department> DepartmentList
        {
            get => _DepartmentList;
            set => Set(ref _DepartmentList, value);
        }

        private Department? _selectDepartment;
        public Department? SelectDepartment
        {
            get => _selectDepartment;
            set => Set(ref _selectDepartment, value);
        }

        private Post? _SelectPosts;
        public Post? SelectPost
        {
            get => _SelectPosts;
            set
            {
                Set(ref _SelectPosts, value);
                if (SelectPost != null)
                {
                    if (SelectPost.UserGroup == true)
                    {
                        VisibilityError = Visibility.Hidden;
                    }
                    else
                    {
                        VisibilityError = (SelectPost.Employees.Count == 0 || (SelectPost.Employees.Count == 1 && SelectPost.Employees.FirstOrDefault().Id == Employee.Id)) ? Visibility.Hidden : Visibility.Visible;
                    }
                }
                else
                {
                    VisibilityError = Visibility.Hidden;
                }
            }
        }

        private Visibility _VisibilityError = Visibility.Hidden;
        public Visibility VisibilityError
        {
            get => _VisibilityError;
            set => Set(ref _VisibilityError, value);
        }

        private bool _admin = false;
        public bool Admin
        {
            get => _admin;
            set => Set(ref _admin, value);
        }

        public ICommand EditEmployeeSystemCommand { get; }
        private void OnEditEmployeeCommandExecuted(object p)
        {
            try
            {
                using (ProjectManagementContext db = new())
                {
                    Employee emp = db.Employees.Where(e => e.Id == Employee.Id).First();
                    emp.Post = SelectPost.Id;
                    emp.Admin = Admin;
                    db.SaveChanges();

                    ChangingEmployee.changingEmployee = null;
                    MessageBox.Show("Cотрудник изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    OnDialogComplete(EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CanEditEmployeeCommandExecute(object p) => Login != "" && SelectDepartment != null && SelectPost != null && VisibilityError != Visibility.Visible;

        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object p) => OnDialogComplete(EventArgs.Empty);
        private bool CanBackCommandExecute(object p) => true;


        public EditEmployeeWindowViewModel()
        {
            EditEmployeeSystemCommand = new RelayCommand(OnEditEmployeeCommandExecuted, CanEditEmployeeCommandExecute);
            BackCommand = new RelayCommand(OnBackCommandExecuted, CanBackCommandExecute);

            if (ChangingEmployee.changingEmployee != null)
            {
                Employee = ChangingEmployee.changingEmployee;

                using (ProjectManagementContext db = new())
                {
                    db.Departments.Load();
                    db.Posts.Load();
                    db.Employees.Load();
                    DepartmentList = db.Departments.ToList();
                    SelectDepartment = DepartmentList.Where(e => e.Id == Employee.PostNavigation.DepartmentNavigation.Id).First();
                    SelectPost = SelectDepartment.Posts.Where(e => e.Id == Employee.PostNavigation.Id).First();
                    Login = Employee.Login;
                    Admin = Employee.Admin;
                    
                }
            }
        }
    }
}
