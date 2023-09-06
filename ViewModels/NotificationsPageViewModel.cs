using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Infrastructure.Commands;
using ProjectManagement.Models;
using ProjectManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectManagement.ViewModels
{
    internal class NotificationsPageViewModel:ViewModel
    {
        private Notification[] _notifications;
        public Notification[] Notifications
        {
            get => _notifications;
            set => Set(ref _notifications, value);
        }

        public ICommand ResetCommand { get; }
        private void OnResetCommandExecuted(object p)
        {
            using (ProjectManagementContext db = new())
            {
                Notifications = db.Notifications.Where(e => e.Employee == CurrentEmployee.currentEmployee.Id).OrderByDescending(e => e.Id).ToArray();
            }
        }
        private bool CanResetCommandExecute(object p) => true;


        public NotificationsPageViewModel()
        {
            ResetCommand = new RelayCommand(OnResetCommandExecuted, CanResetCommandExecute);
            using (ProjectManagementContext db = new ())
            {
                Notifications = db.Notifications.Where(e => e.Employee == CurrentEmployee.currentEmployee.Id).OrderByDescending(e => e.Id).ToArray();
            }
        }
    }
}
