using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.ViewModels;
using ProjectManagement.Views.Windows;
using System;

namespace ProjectManagement.Services
{
    internal class UserDialogServices: IUserDialog
    {
        private readonly IServiceProvider _Services;

        public UserDialogServices(IServiceProvider Services) => _Services = Services;

        private MainWindow? _MainWindow;
        public void OpenMainWindow()
        {
            if (_MainWindow is { } window)
            {
                window.Show();
                return;
            }

            window = _Services.GetRequiredService<MainWindow>();
            window.Closed += (_, _) => _MainWindow = null;

            _MainWindow = window;
            window.Show();
        }

        public void CloseMainWindow()
        {
            _MainWindow.Close();
        }

        private AuthorizationWindow? _AuthorizationWindow;
        public void OpenAuthorizationWindow()
        {
            if (_AuthorizationWindow is { } window)
            {
                window.Show();
                return;
            }

            window = _Services.GetRequiredService<AuthorizationWindow>();
            window.Closed += (_, _) => _AuthorizationWindow = null;

            _AuthorizationWindow = window;
            window.Show();
        }

        private AddDepartmentWindow? _AddDepartmentWindow;
        public void OpenAddDepartmentWindow()
        {
            if (_AddDepartmentWindow is { } window)
            {
                window.ShowDialog();
                return;
            }

            window = _Services.GetRequiredService<AddDepartmentWindow>();
            window.Closed += (_, _) => _AddDepartmentWindow = null;

            _AddDepartmentWindow = window;
            window.ShowDialog();
        }

        private EditDepartmentWindow? _EditDepartmentWindow;
        public void OpenEditDepartmentWindow()
        {
            if (_EditDepartmentWindow is { } window)
            {
                window.ShowDialog();
                return;
            }

            window = _Services.GetRequiredService<EditDepartmentWindow>();
            window.Closed += (_, _) => _EditDepartmentWindow = null;

            _EditDepartmentWindow = window;
            window.ShowDialog();
        }

        private AddPostWindow? _AddPostWindow;
        public void OpenAddPostWindow()
        {
            if (_AddPostWindow is { } window)
            {
                window.ShowDialog();
                return;
            }

            window = _Services.GetRequiredService<AddPostWindow>();
            window.Closed += (_, _) => _AddPostWindow = null;

            _AddPostWindow = window;
            window.ShowDialog();
        }

        private EditPostWindow? _EditPostWindow;
        public void OpenEditPostWindow()
        {
            if (_EditPostWindow is { } window)
            {
                window.ShowDialog();
                return;
            }

            window = _Services.GetRequiredService<EditPostWindow>();
            window.Closed += (_, _) => _EditPostWindow = null;

            _EditPostWindow = window;
            window.ShowDialog();
        }

        private AddEmployeeWindow? _AddEmployeeWindow;
        public void OpenAddEmployeeWindow()
        {
            if (_AddEmployeeWindow is { } window)
            {
                window.ShowDialog();
                return;
            }

            window = _Services.GetRequiredService<AddEmployeeWindow>();
            window.Closed += (_, _) => _AddEmployeeWindow = null;

            _AddEmployeeWindow = window;
            window.ShowDialog();
        }

        private EditEmployeeWindow? _EditEmployeeWindow;
        public void OpenEditEmployeeWindow()
        {
            if (_EditEmployeeWindow is { } window)
            {
                window.ShowDialog();
                return;
            }

            window = _Services.GetRequiredService<EditEmployeeWindow>();
            window.Closed += (_, _) => _EditEmployeeWindow = null;

            _EditEmployeeWindow = window;
            window.ShowDialog();
        }

        private AddProjectWindow? _AddProjectWindow;
        public void OpenAddProjectWindow()
        {
            if (_AddProjectWindow is { } window)
            {
                window.ShowDialog();
                return;
            }

            window = _Services.GetRequiredService<AddProjectWindow>();
            window.Closed += (_, _) => _AddProjectWindow = null;

            _AddProjectWindow = window;
            window.ShowDialog();
        }

        private NewEmployeeWindow? _NewEmployeeWindow;
        public void OpenNewEmployeeWindow()
        {
            if (_NewEmployeeWindow is { } window)
            {
                window.Show();
                return;
            }

            window = _Services.GetRequiredService<NewEmployeeWindow>();
            window.Closed += (_, _) => _NewEmployeeWindow = null;

            _NewEmployeeWindow = window;
            window.Show();
        }

        private ProjectWindow? _ProjectWindow;
        public void OpenProjectWindow()
        {
            if (_ProjectWindow is { } window)
            {
                window.ShowDialog();
                return;
            }

            window = _Services.GetRequiredService<ProjectWindow>();
            window.Closed += (_, _) => _ProjectWindow = null;

            _ProjectWindow = window;
            window.ShowDialog();
        }

        private EditEmployeesProjectWindow? _EditEmployeesProjectWindow;
        public void OpenEditEmployeesProjectWindow()
        {
            if (_EditEmployeesProjectWindow is { } window)
            {
                window.ShowDialog();
                return;
            }

            window = _Services.GetRequiredService<EditEmployeesProjectWindow>();
            window.Closed += (_, _) => _EditEmployeesProjectWindow = null;

            _EditEmployeesProjectWindow = window;
            window.ShowDialog();
        }

        private AddPhaseWindow? _AddPhaseWindow;
        public void OpenAddPhaseWindow()
        {
            if (_AddPhaseWindow is { } window)
            {
                window.ShowDialog();
                return;
            }

            window = _Services.GetRequiredService<AddPhaseWindow>();
            window.Closed += (_, _) => _AddPhaseWindow = null;

            _AddPhaseWindow = window;
            window.ShowDialog();
        }

        private AddTaskWindow? _AddTaskWindow;
        public void OpenAddTaskWindow()
        {
            if (_AddTaskWindow is { } window)
            {
                window.ShowDialog();
                return;
            }

            window = _Services.GetRequiredService<AddTaskWindow>();
            window.Closed += (_, _) => _AddTaskWindow = null;

            _AddTaskWindow = window;
            window.ShowDialog();
        }

        private AddEmployeeTaskWindow? _AddEmployeeTaskWindow;
        public void OpenAddEmployeeTaskWindow()
        {
            if (_AddEmployeeTaskWindow is { } window)
            {
                window.ShowDialog();
                return;
            }

            window = _Services.GetRequiredService<AddEmployeeTaskWindow>();
            window.Closed += (_, _) => _AddEmployeeTaskWindow = null;

            _AddEmployeeTaskWindow = window;
            window.ShowDialog();
        }

        private EditTaskWindow? _EditTaskWindow;
        public void OpenEditTaskWindow()
        {
            if (_EditTaskWindow is { } window)
            {
                window.ShowDialog();
                return;
            }

            window = _Services.GetRequiredService<EditTaskWindow>();
            window.Closed += (_, _) => _EditTaskWindow = null;

            _EditTaskWindow = window;
            window.ShowDialog();
        }

        private EditPhaseWindow? _EditPhaseWindow;
        public void OpenEditPhaseWindow()
        {
            if (_EditPhaseWindow is { } window)
            {
                window.ShowDialog();
                return;
            }

            window = _Services.GetRequiredService<EditPhaseWindow>();
            window.Closed += (_, _) => _EditPhaseWindow = null;

            _EditPhaseWindow = window;
            window.ShowDialog();
        }

        private EditProjectWindow? _EditProjectWindow;
        public void OpenEditProjectWindow()
        {
            if (_EditProjectWindow is { } window)
            {
                window.ShowDialog();
                return;
            }

            window = _Services.GetRequiredService<EditProjectWindow>();
            window.Closed += (_, _) => _EditProjectWindow = null;

            _EditProjectWindow = window;
            window.ShowDialog();
        }
    }
}
