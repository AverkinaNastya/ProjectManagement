using Microsoft.EntityFrameworkCore;
using ProjectManagement.Infrastructure.Commands;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ProjectManagement.ViewModels
{
    internal class EditProjectWindowViewModel: ViewModel
    {
        private Project _project;
        public Project Project
        {
            get => _project;
            set => Set(ref _project, value);
        }

        private String _name = "";
        public String Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        private String _comment = "";
        public String Comment
        {
            get => _comment;
            set => Set(ref _comment, value);
        }

        public ICommand EditProjectSystemCommand { get; }
        private void OnEditProjectSystemCommandExecuted(object p)
        {
            try
            {
                using (ProjectManagementContext db = new())
                {
                    db.Departments.Load();
                    if (db.Projects.Where(e => e.Name == Name && e.Id != Project.Id).FirstOrDefault() == null)
                    {
                        db.Projects.Where(e => e.Id == Project.Id).First().Name = Name;
                        db.Projects.Where(e => e.Id == Project.Id).First().Comment = Comment;
                        db.SaveChanges();
                        MessageBox.Show("Проект изменен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        OnDialogComplete(EventArgs.Empty);
                    }
                    else
                    {
                        MessageBox.Show("Проект с таким названием уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        private bool CanEditProjectSystemCommandExecute(object p) => Name != "" && Project != null;

        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object p) => OnDialogComplete(EventArgs.Empty);
        private bool CanBackCommandExecute(object p) => true;

        public EditProjectWindowViewModel()
        {

            if (ChangingProject.project != null)
            {
                Project = ChangingProject.project;
                Name = Project.Name;
            }

            EditProjectSystemCommand = new RelayCommand(OnEditProjectSystemCommandExecuted, CanEditProjectSystemCommandExecute);
            BackCommand = new RelayCommand(OnBackCommandExecuted, CanBackCommandExecute);
        }

    }
}
