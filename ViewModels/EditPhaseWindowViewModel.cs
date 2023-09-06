using ProjectManagement.Infrastructure.Commands;
using ProjectManagement.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ProjectManagement.ViewModels
{
    class EditPhaseWindowViewModel: ViewModel
    {
        private Phase _editPhase = new Phase();
        public Phase EditPhase
        {
            get => _editPhase;
            set => Set(ref _editPhase, value);
        }

        private String _name = "";
        public String Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ICommand EditPhaseSystemCommand { get; }
        private void OnEditPhaseSystemCommandExecuted(object p)
        {
            try
            {
                using (ProjectManagementContext db = new())
                {
                    if (db.Phases.Where(e => e.Name == Name && e.Id != EditPhase.Id && e.Project == EditPhase.Project).FirstOrDefault() == null)
                    {
                        db.Phases.Where(e => e.Id == EditPhase.Id).First().Name = Name;
                        db.SaveChanges();
                        ChangingPhase.phase = null;
                        MessageBox.Show("Фаза изменена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
        private bool CanEditPhaseSystemCommandExecute(object p) => Name != "";

        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object p) => OnDialogComplete(EventArgs.Empty);
        private bool CanBackCommandExecute(object p) => true;

        public EditPhaseWindowViewModel()
        {

            if (ChangingPhase.phase != null)
            {
                EditPhase = ChangingPhase.phase;
                Name = EditPhase.Name;
            }

            EditPhaseSystemCommand = new RelayCommand(OnEditPhaseSystemCommandExecuted, CanEditPhaseSystemCommandExecute);
            BackCommand = new RelayCommand(OnBackCommandExecuted, CanBackCommandExecute);
        }
    }
}
