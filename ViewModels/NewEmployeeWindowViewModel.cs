using ProjectManagement.Infrastructure.Commands;
using ProjectManagement.Models;
using ProjectManagement.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ProjectManagement.ViewModels
{
    internal class NewEmployeeWindowViewModel: ViewModel
    {
        private readonly IUserDialog _userDialog;

        private Employee _employee;
        public Employee Employee
        {
            get => _employee;
            set => Set(ref _employee, value);
        }

        private String _login;
        public String Login
        {
            get => _login;
            set => Set(ref _login, value);
        }

        private String _password;
        public String Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        private String _surname = "";
        public String Surname
        {
            get => _surname;
            set => Set(ref _surname, value);
        }

        private String _name = "";
        public String Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private String? _patronymic = "";
        public String? Patronymic
        {
            get => _patronymic;
            set => Set(ref _patronymic, value);
        }

        private DateOnly date;

        private String _dateBirth = "";
        public String DateBirth
        {
            get => _dateBirth;
            set => Set(ref _dateBirth, value);
        }

        #region Команды

        #region SaveInfoSystemCommand - команда сохранения данных
        public ICommand SaveInfoSystemCommand { get; }
        private void OnSaveInfoSystemCommandExecuted(object p)
        {
            try
            {
                using (ProjectManagementContext db = new())
                {
                    Employee emp = db.Employees.Where(e => e.Id == Employee.Id).First();
                    emp.Password = Password;
                    emp.Surname = Surname;
                    emp.Name = Name;
                    if (emp.Patronymic != "") emp.Patronymic = Patronymic;
                    emp.DateBirth = date;
                    emp.New = false;
                    db.SaveChanges();
                    CurrentEmployee.currentEmployee = emp;
                    _userDialog.OpenMainWindow();
                    OnDialogComplete(EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CanSaveInfoSystemCommandExecute(object p) => (Login != "" && Password != "" && Surname != "" && 
                                                                   Name != "" && DateOnly.TryParse(DateBirth, out date));
        #endregion

        #endregion

        public NewEmployeeWindowViewModel()
        {
            SaveInfoSystemCommand = new RelayCommand(OnSaveInfoSystemCommandExecuted, CanSaveInfoSystemCommandExecute);

            Employee = CurrentEmployee.currentEmployee;
            Login = Employee.Login;
            Password = Employee.Password;
        }

        public NewEmployeeWindowViewModel(IUserDialog userDialog) : this()
        {
            _userDialog = userDialog;
        }
    }
}
