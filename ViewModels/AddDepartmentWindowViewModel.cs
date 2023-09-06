using ProjectManagement.Infrastructure.Commands;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ProjectManagement.ViewModels
{
    class AddDepartmentWindowViewModel: ViewModel
    {

        private String _name = "";
        public String Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        #region Команды
        #region AddDepartamentSystemCommand - добавление департамента
        public ICommand AddDepartamentSystemCommand { get; }
        private void OnAddDepartamentCommandExecuted(object p)
        {
            try
            {
                using (ProjectManagementContext db = new())
                {
                    if (db.Departments.Where(e => e.Name == Name).FirstOrDefault() == null)
                    {
                        Department department = new()
                        {
                            Name = Name,
                            Id = db.Departments.Select(e => e.Id).ToList().Max() + 1
                        };
                        db.Departments.Add(department);
                        db.SaveChanges();
                        MessageBox.Show("Новый департамент создан", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        OnDialogComplete(EventArgs.Empty);
                    }
                    else
                    {
                        MessageBox.Show("Департамент с таким названием уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CanAddDepartamentCommandExecute(object p) => Name != "";
        #endregion
        #region BackCommand - команда отмены
        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object p) => OnDialogComplete(EventArgs.Empty);
        private bool CanBackCommandExecute(object p) => true;
        #endregion
        #endregion
        public AddDepartmentWindowViewModel()
        {
            AddDepartamentSystemCommand = new RelayCommand(OnAddDepartamentCommandExecuted, CanAddDepartamentCommandExecute);
            BackCommand = new RelayCommand(OnBackCommandExecuted, CanBackCommandExecute);
        }
    }
}
