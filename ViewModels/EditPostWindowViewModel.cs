using ProjectManagement.Infrastructure.Commands;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagement.ViewModels
{
    internal class EditPostWindowViewModel: ViewModel
    {
        private Post _post;
        public Post Post
        {
            get => _post;
            set => Set(ref _post, value);
        }

        private String _name = "";
        public String Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ICommand EditPostSystemCommand { get; }
        private void OnEditPostSystemCommandExecuted(object p)
        {
            try
            {
                using (ProjectManagementContext db = new())
                {
                    db.Departments.Load();
                    if (db.Posts.Where(e => e.Department == Post.Department && e.Name == Name && e.Id != Post.Id).FirstOrDefault() == null)
                    {
                        db.Posts.Where(e => e.Id == Post.Id).First().Name = Name;
                        db.SaveChanges();
                        ChangingPost.changingPost = null;
                        MessageBox.Show("Должность изменена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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
        private bool CanEditPostSystemCommandExecute(object p) => Name != "" && Post != null;

        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object p) => OnDialogComplete(EventArgs.Empty);
        private bool CanBackCommandExecute(object p) => true;

        public EditPostWindowViewModel()
        {

            if (ChangingPost.changingPost != null)
            {
                Post = ChangingPost.changingPost;
                Name = Post.Name;
            }

            EditPostSystemCommand = new RelayCommand(OnEditPostSystemCommandExecuted, CanEditPostSystemCommandExecute);
            BackCommand = new RelayCommand(OnBackCommandExecuted, CanBackCommandExecute);
        }
    }
}
