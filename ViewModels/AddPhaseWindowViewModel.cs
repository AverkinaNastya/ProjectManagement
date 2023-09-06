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
    class AddPhaseWindowViewModel: ViewModel
    {
        private String _name = "";
        public String Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ICommand AddPhaseSystemCommand { get; }
        private void OnAddPhaseCommandExecuted(object p)
        {
            try
            {
                using (ProjectManagementContext db = new())
                {
                    if (db.Phases.Where(e => e.Name == Name && e.Project == ChangingProject.project.Id).FirstOrDefault() == null)
                    {
                        Phase phase = new()
                        {
                            Name = Name,
                            Project = ChangingProject.project.Id,
                            Id = db.Phases.Select(e => e.Id).ToList().Max() + 1
                        };
                        db.Phases.Add(phase);
                        db.SaveChanges();
                        MessageBox.Show("Новая фаза создана", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        OnDialogComplete(EventArgs.Empty);
                    }
                    else
                    {
                        MessageBox.Show("Фаза с таким названием уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private bool CanAddPhaseCommandExecute(object p) => Name != "";

        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object p) => OnDialogComplete(EventArgs.Empty);
        private bool CanBackCommandExecute(object p) => true;

        public AddPhaseWindowViewModel()
        {
            AddPhaseSystemCommand = new RelayCommand(OnAddPhaseCommandExecuted, CanAddPhaseCommandExecute);
            BackCommand = new RelayCommand(OnBackCommandExecuted, CanBackCommandExecute);
        }
    }
}
