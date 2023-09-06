using Microsoft.EntityFrameworkCore;
using ProjectManagement.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace ProjectManagement.ViewModels
{
    public class AddEmployeeWindowViewModel: ViewModel
    {
        private String _Login = "";
        public String Login
        {
            get => _Login;
            set => Set(ref  _Login, value);
        }

        private String _Password = "";
        public String Password
        {
            get => _Password;
            set => Set(ref _Password, value);
        }

        private List<Department> _DepartmentList = new ();
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
                        VisibilityError = (SelectPost.Employees.Count != 0) ? Visibility.Visible : Visibility.Hidden;
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

        public ICommand AddEmployeeSystemCommand { get; }
        private void OnAddEmployeeCommandExecuted(object p)
        {
            try
            {
                using (ProjectManagementContext db = new())
                {
                    if (db.Employees.Where(e => e.Login == Login).FirstOrDefault() == null)
                    {
                        Employee employee = new()
                        {
                            Login = Login,
                            Password = Password,
                            Blocked = false,
                            New = true,
                            Admin = Admin,
                            Post = SelectPost!.Id,
                            Id = db.Employees.Select(e => e.Id).ToList().Max() + 1
                        };
                        db.Employees.Add(employee);
                        db.SaveChanges();
                        MessageBox.Show("Новый сотрудник создан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        OnDialogComplete(EventArgs.Empty);
                    }
                    else
                    {
                        MessageBox.Show("Сотрудник с таким логином уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CanAddEmployeeCommandExecute(object p) => Login != "" && Password != "" && SelectDepartment != null && SelectPost != null && VisibilityError != Visibility.Visible;

        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object p) => OnDialogComplete(EventArgs.Empty);
        private bool CanBackCommandExecute(object p) => true;


        public AddEmployeeWindowViewModel()
        {
            AddEmployeeSystemCommand = new RelayCommand(OnAddEmployeeCommandExecuted, CanAddEmployeeCommandExecute);
            BackCommand = new RelayCommand(OnBackCommandExecuted, CanBackCommandExecute);

            using (ProjectManagementContext db = new())
            {
                db.Posts.Load();
                db.Employees.Load();
                DepartmentList = db.Departments.ToList();
                SelectDepartment = DepartmentList.FirstOrDefault();
            }
        }
    }
}
