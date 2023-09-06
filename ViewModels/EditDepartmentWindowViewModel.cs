using ProjectManagement.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using ProjectManagement.Models;

namespace ProjectManagement.ViewModels
{
    class EditDepartmentWindowViewModel: ViewModel
    {
        private Department _editDepartment = new Department();
        public Department EditDepartment
        {
            get => _editDepartment;
            set => Set(ref  _editDepartment, value);
        }

        private String _name = "";
        public String Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ICommand EditDepartamentSystemCommand { get; }
        private void OnEditDepartamentCommandExecuted(object p)
        {
            try
            {
                using (ProjectManagementContext db = new())
                {
                    if (db.Departments.Where(e => e.Name == Name && e.Id != EditDepartment.Id).FirstOrDefault() == null)
                    {
                        db.Departments.Where(e => e.Id == EditDepartment.Id).First().Name = Name;
                        db.SaveChanges();
                        ChangingDepartment.changingDepartment = null;
                        MessageBox.Show("Департамент изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
        private bool CanEditDepartamentCommandExecute(object p) => Name != "";

        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object p) => OnDialogComplete(EventArgs.Empty);
        private bool CanBackCommandExecute(object p) => true;

        public EditDepartmentWindowViewModel()
        {

            if (ChangingDepartment.changingDepartment != null)
            {
                EditDepartment = ChangingDepartment.changingDepartment;
                Name = EditDepartment.Name;
            }

            EditDepartamentSystemCommand = new RelayCommand(OnEditDepartamentCommandExecuted, CanEditDepartamentCommandExecute);
            BackCommand = new RelayCommand(OnBackCommandExecuted, CanBackCommandExecute);
        }
    }
}
