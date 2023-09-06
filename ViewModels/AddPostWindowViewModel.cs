using ProjectManagement.Infrastructure.Commands;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace ProjectManagement.ViewModels
{
    class AddPostWindowViewModel: ViewModel
    {
        private bool _groupRole = true;
        public bool GroupRole
        {
            get => _groupRole;
            set => Set(ref _groupRole, value);
        }

        private bool _noGroupRole = false;
        public bool NoGroupRole
        {
            get => _noGroupRole;
            set => Set(ref _noGroupRole, value);
        }

        private Department _department = new Department();
        public Department Department
        {
            get => _department;
            set => Set(ref _department, value);
        }

        private String _name = "";
        public String Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ICommand AddPostSystemCommand { get; }
        private void OnAddPostSystemCommandExecuted(object p)
        {
            try
            {
                using (ProjectManagementContext db = new())
                {
                    if (db.Posts.Where(e => e.Name == Name && e.Department == Department.Id).FirstOrDefault() == null)
                    {
                        Post post = new()
                        {
                            Name = Name,
                            Department = Department.Id,
                            UserGroup = GroupRole,
                            Id = db.Posts.Select(e => e.Id).ToList().Max() + 1
                        };
                        db.Posts.Add(post);
                        db.SaveChanges();
                        ChangingDepartment.changingDepartment = null;
                        MessageBox.Show("Новая должность создана", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        OnDialogComplete(EventArgs.Empty);
                    }
                    else
                    {
                        MessageBox.Show("Должность с таким названием уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CanAddPostSystemCommandExecute(object p) => Name != "" && (GroupRole ^ NoGroupRole);

        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object p) => OnDialogComplete(EventArgs.Empty);
        private bool CanBackCommandExecute(object p) => true;

        public AddPostWindowViewModel()
        {

            if (ChangingDepartment.changingDepartment != null)
            {
                Department = ChangingDepartment.changingDepartment;
            }

            AddPostSystemCommand = new RelayCommand(OnAddPostSystemCommandExecuted, CanAddPostSystemCommandExecute);
            BackCommand = new RelayCommand(OnBackCommandExecuted, CanBackCommandExecute);
        }
    }
}
