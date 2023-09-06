using Microsoft.Extensions.DependencyInjection;
using ProjectManagement.Services;
using ProjectManagement.Services.NavigatorPages;
using ProjectManagement.ViewModels;
using ProjectManagement.ViewModels.Base;
using ProjectManagement.Views.Windows;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace ProjectManagement
{
    public partial class App : Application
    {
        private static IServiceProvider? _Services;
        public static IServiceProvider Services => _Services ??= InitializeServices().BuildServiceProvider();

        private static IServiceCollection InitializeServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<AuthorizationWindowViewModel>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient<AddDepartmentWindowViewModel>();
            services.AddTransient<EditDepartmentWindowViewModel>();
            services.AddTransient<AddPostWindowViewModel>();
            services.AddTransient<EditPostWindowViewModel>();
            services.AddTransient<AddEmployeeWindowViewModel>();
            services.AddTransient<EditEmployeeWindowViewModel>();
            services.AddTransient<AddProjectWindowViewModel>();
            services.AddTransient<NewEmployeeWindowViewModel>();
            services.AddTransient<ProjectWindowViewModel>();
            services.AddTransient<EditEmployeesProjectWindowViewModel>();
            services.AddTransient<AddPhaseWindowViewModel>();
            services.AddTransient<AddTaskWindowViewModel>();
            services.AddTransient<AddEmployeeTaskWindowViewModel>();
            services.AddTransient<EditTaskWindowViewModel>();
            services.AddTransient<EditPhaseWindowViewModel>();
            services.AddTransient<EditProjectWindowViewModel>();

            services.AddSingleton<IUserDialog, UserDialogServices>();
            services.AddSingleton<IPageResolver, PagesResolver>();
            services.AddSingleton<IViewModelsResolver, ViewModelsResolver>();

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<AuthorizationWindowViewModel>();
                var window = new AuthorizationWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<MainWindowViewModel>();
                var window = new MainWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<AddDepartmentWindowViewModel>();
                var window = new AddDepartmentWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<EditDepartmentWindowViewModel>();
                var window = new EditDepartmentWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<AddPostWindowViewModel>();
                var window = new AddPostWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<EditPostWindowViewModel>();
                var window = new EditPostWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<AddEmployeeWindowViewModel>();
                var window = new AddEmployeeWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<EditEmployeeWindowViewModel>();
                var window = new EditEmployeeWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<AddProjectWindowViewModel>();
                var window = new AddProjectWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<NewEmployeeWindowViewModel>();
                var window = new NewEmployeeWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<ProjectWindowViewModel>();
                var window = new ProjectWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<EditEmployeesProjectWindowViewModel>();
                var window = new EditEmployeesProjectWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<AddPhaseWindowViewModel>();
                var window = new AddPhaseWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<AddTaskWindowViewModel>();
                var window = new AddTaskWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<AddEmployeeTaskWindowViewModel>();
                var window = new AddEmployeeTaskWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<EditTaskWindowViewModel>();
                var window = new EditTaskWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<EditPhaseWindowViewModel>();
                var window = new EditPhaseWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });
            
            services.AddTransient(s =>
            {
                var model = s.GetRequiredService<EditProjectWindowViewModel>();
                var window = new EditProjectWindow() { DataContext = model };
                model.DialogComplete += (_, _) => window.Close();

                return window;
            });

            return services;
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
            Services.GetRequiredService<IUserDialog>().OpenAuthorizationWindow();
        }
    }
}
