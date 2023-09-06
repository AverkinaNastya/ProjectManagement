using GongSolutions.Wpf.DragDrop;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using ProjectManagement.Infrastructure.Classes;
using ProjectManagement.Infrastructure.Commands;
using ProjectManagement.Models;
using ProjectManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Excel = Microsoft.Office.Interop.Excel;

namespace ProjectManagement.ViewModels
{
    internal class ProjectWindowViewModel: ViewModel, IDropTarget
    {
        #region Реализация IDropTarget для DragAndDrop задач на канбан
        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo != null && dropInfo.Data is Task task && dropInfo.VisualTarget is System.Windows.Controls.ListBox list)
            {
                if (((Phase)list.DataContext).Id == task.Phase && task.Status == 1 && (list.Name == "ToDo" || list.Name == "Done") && (task.Executor == CurrentEmployee.currentEmployee.Id) ||
                    ((Phase)list.DataContext).Id == task.Phase && task.Status == 2 && (list.Name == "Done") && (task.Executor == CurrentEmployee.currentEmployee.Id))
                {
                    dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                    dropInfo.Effects = DragDropEffects.Move;

                }
            }
        }

        public void Drop(IDropInfo dropInfo)
        {
            if (dropInfo != null && dropInfo.Data is Task task && dropInfo.VisualTarget is System.Windows.Controls.ListBox list)
            {
                using(ProjectManagementContext db = new ())
                {
                    db.Tasks.Load();
                    db.Employees.Load();
                    if (list.Name == "ToDo")
                    {
                        db.Tasks.Single(e => e.Id == task.Id).Status = 2;
                    }
                    else if (list.Name == "Done")
                    {
                        db.Tasks.Single(e => e.Id == task.Id).Status = 3;
                    }
                    db.SaveChanges();
                    Phases = db.Projects.Include(e => e.Phases).Single(e => e.Id == Project.Id).Phases.ToList();
                }
            }
        }
        #endregion

        private readonly Visibility _visibilityAdmin;
        public Visibility VisibilityAdmin
        {
            get => _visibilityAdmin;
        }

        private Project _project;
        public Project Project
        {
            get => _project;
            set => Set(ref  _project, value);
        }

        private String _nameProject;
        public String NameProject
        {
            get => _nameProject;
            set => Set(ref _nameProject, value);
        }

        private ObservableCollection<Employee> _employeesProject = new ();
        public ObservableCollection<Employee> EmployeesProject
        {
            get => _employeesProject;
            set => Set(ref _employeesProject, value);
        }

        private List<Phase> _phases = new ();
        public List<Phase> Phases
        {
            get => _phases;
            set => Set(ref _phases, value);
        }

        private DateOnly _dateStart;
        public DateOnly DateStart
        {
            get => _dateStart;
            set {
                Set(ref _dateStart, value);
                DateOnly[] dateOnly = new DateOnly[14];
                for (int i = 0; i < 14; i++)
                {
                    dateOnly[i] = DateStart.AddDays(i);
                }
                Dates = dateOnly;

            }
        }

        private DateOnly[] _dates;
        public DateOnly[] Dates
        {
            get => _dates;
            set => Set(ref _dates, value);
        }

        private Visibility _visibilityButtons;
        public Visibility VisibilityButtons
        {
            get => _visibilityButtons;
            set => Set(ref _visibilityButtons, value);
        }

        #region Команды

        #region PlusDateCommand - увеличение даты при клике
        public ICommand PlusDateCommand { get; }
        private void OnPlusDateCommandExecuted(object p) => DateStart = DateStart.AddDays(7);
        private bool CanPlusDateCommandExecute(object p) => true;
        #endregion

        #region MinusDateCommand - уменьшение даты при клике
        public ICommand MinusDateCommand { get; }
        private void OnMinusDateCommandExecuted(object p) => DateStart = DateStart.AddDays(-7);
        private bool CanMinusDateCommandExecute(object p) => true;
        #endregion

        #region PressPlusDateCommand - увеличение даты при нажатии
        public ICommand PressPlusDateCommand { get; }
        private void OnPressPlusDateCommandExecuted(object p) => DateStart = DateStart.AddDays(1);
        private bool CanPressPlusDateCommandExecute(object p) => true;
        #endregion

        #region PressMinusDateCommand - уменьшение даты при нажатии
        public ICommand PressMinusDateCommand { get; }
        private void OnPressMinusDateCommandExecuted(object p) => DateStart = DateStart.AddDays(-1);
        private bool CanPressMinusDateCommandExecute(object p) => true;
        #endregion

        #region DownloadeCommand - скачать документ
        public ICommand DownloadeCommand { get; }
        private void OnDownloadeCommandExecuted(object p)
        {
            SaveFileDialog fileDialog = new()
            {
                Filter = "Excel | *.xlsx"
            };
            if (fileDialog.ShowDialog() == true)
            {
                Project pr;
                Excel.Application excel = new Excel.Application();
                Excel.Workbook workbook = excel.Workbooks.Add();
                Excel.Worksheet worksheet;
                using (ProjectManagementContext db = new ())
                {
                    db.Projects.Load();
                    db.Statuses.Load();
                    db.Tasks.Load();
                    db.Phases.Load();
                    db.Employees.Load();

                    pr = db.Projects.Include(e => e.Phases).Single(e => e.Id == ChangingProject.project.Id);

                }
                string sourceFile = fileDialog.FileName;
                string destinationFile = @"../../../Data/шаблон.xlsx";
                try
                {
                    
                    worksheet = workbook.ActiveSheet;
                    string path = sourceFile;
                    workbook.SaveAs(path);
                    workbook.Close();
                    excel.Quit();

                    System.IO.File.Copy(destinationFile, sourceFile, true);

                    excel = new Excel.Application();
                    workbook = excel.Workbooks.Open(sourceFile);
                    worksheet = workbook.ActiveSheet;

                    worksheet.Cells[1, 2] = pr.Name;

                    int idRow = 8;
                    foreach (Phase phase in pr.Phases)
                    {
                        Excel.Range sourceRange = worksheet.Range[$"8:8"];
                        Excel.Range targetRange = worksheet.Range[$"{idRow}:{idRow}"];
                        sourceRange.Copy(targetRange);
                        worksheet.Cells[idRow, 2] = phase.Name;
                        idRow++;
                        foreach (Task task in phase.Tasks.OrderBy(e => e.DateComplet).ToList())
                        {
                            sourceRange = worksheet.Range[$"8:8"];
                            targetRange = worksheet.Range[$"{idRow}:{idRow}"];
                            sourceRange.Copy(targetRange);
                            Task task1 = task;
                            using(ProjectManagementContext db = new ProjectManagementContext())
                            {
                                db.Projects.Load();
                                db.Statuses.Load();
                                db.Tasks.Load();
                                db.Phases.Load();
                                db.Employees.Load();

                                task1 = db.Tasks.Include(e => e.MainTasks).Single(e => e.Id == task.Id);
                            }
                            worksheet.Cells[idRow, 2] = task1.Name;
                            worksheet.Cells[idRow, 3] = $"{task1.ExecutorNavigation.Surname} {task1.ExecutorNavigation.Name} {task1.ExecutorNavigation.Patronymic}";
                            worksheet.Cells[idRow, 4] = task1.StatusNavigation.Name;
                            worksheet.Cells[idRow, 5] = (task1.MainTasks.Count != 0) ? DateTime.Parse(FerialDays.AddDay(task1.MainTasks.Max(e => e.DateComplet), 1).ToString()) :
                                                                                       DateTime.Parse(pr.StartDate.ToString());
                            worksheet.Cells[idRow, 6] = DateTime.Parse(task1.DateComplet.ToString());
                            idRow++;
                        }
                    }
                    workbook.Save();
                    MessageBox.Show("Файл успешно создан и доступен по ссылке " + sourceFile, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                finally
                {
                    workbook.Close();
                    excel.Quit();
                }
            }
        }
        private bool CanDownloadeCommandExecute(object p) => true;
        #endregion

        #region AddEmployeesProjectCommand - команда добавление сотрудника в проекта
        public ICommand AddEmployeesProjectCommand { get; }
        private void OnAddEmployeesProjectCommandExecuted(object p)
        {
            ChangingListEmployees.Employees = EmployeesProject.ToList();
            App.Services.GetRequiredService<IUserDialog>().OpenEditEmployeesProjectWindow();
            ChangingListEmployees.Employees = null!;
            if (ChangingEmployee.changingEmployee != null)
            {
                using (ProjectManagementContext db = new ())
                {
                    db.Employees.Include(e => e.Projects).Single(e => e.Id == ChangingEmployee.changingEmployee.Id).Projects.Add(db.Projects.Single(e => e.Id == Project.Id));
                    
                    db.Notifications.Add(new Notification()
                    {
                        Employee = ChangingEmployee.changingEmployee.Id,
                        Id = (db.Notifications.Any()) ? db.Notifications.Max(e => e.Id) + 1 : 1,
                        Text = "Вы были добавлены в проект " + Project.Name,
                        Date = DateOnly.FromDateTime(DateTime.Now),
                        New = true
                    });
                    db.SaveChanges();
                    EmployeesProject.Clear();
                    db.Employees.Where(e => e.Projects.Contains(Project)).ToList().ForEach(EmployeesProject.Add);
                }
                ChangingEmployee.changingEmployee = null!;
            }
        }
        private bool CanAddEmployeesProjectCommandExecute(object p) => true;
        #endregion

        #region CompletProjectCommand - завершение проекта
        public ICommand CompletProjectCommand { get; }
        private void OnCompletProjectCommandExecuted(object p)
        {
            using (ProjectManagementContext db = new())
            {
                db.Projects.Load();
                db.Tasks.Load();
                db.Statuses.Load();
                Project pr = db.Projects.Single(e => e.Id == Project.Id);
                pr.CompletDate = DateOnly.FromDateTime(DateTime.Now);
                pr.Completed = true;
                db.SaveChanges();
                Project = db.Projects.Single(e => e.Id == pr.Id);
                VisibilityButtons = (Project.Completed) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        private bool CanCompletProjectCommandExecute(object p) => Phases.All(e => e.Tasks.All(i => i.Status == 3));
        #endregion

        #region DeletePhaseCommand - команда удаления фазы из проекта
        public ICommand DeletePhaseCommand { get; }
        private void OnDeletePhaseCommandExecuted(object p)
        {
            if (p is Phase phase)
            {
                try
                {
                    if (MessageBox.Show($"Вы уверены, что хотите удалить фазу \"{phase.Name}\" из проекта \"{Project.Name}\"", "Подтверждение",
                        MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                    {
                        using (ProjectManagementContext db = new())
                        {
                            db.Tasks.Load();
                            db.Phases.Load();
                            db.Statuses.Load();
                            db.Projects.Load();
                            db.Employees.Load();
                            Project pr = db.Projects.Include(e => e.Phases).Where(e => e.Id == Project.Id).First();
                            Phase ph = db.Phases.Include(e => e.Tasks).Where(e => e.Id == phase.Id).First();

                            if (ph.Tasks.Count == 0)
                            {
                                db.Phases.Remove(ph);
                                db.SaveChanges();
                                Phases = db.Projects.Include(e => e.Phases).Single(e => e.Id == Project.Id).Phases.ToList();
                            }
                            else
                            {
                                MessageBox.Show("Нельзя удалить из проекта фазу, которая содержит задачи", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private bool CanDeletePhaseCommandExecute(object p) => true;
        #endregion

        #region AddPhaseProjectCommand - добавление фазы
        public ICommand AddPhaseProjectCommand { get; }
        private void OnAddPhaseProjectCommandExecuted(object p)
        {
            App.Services.GetRequiredService<IUserDialog>().OpenAddPhaseWindow();
            using (ProjectManagementContext db = new())
            {
                db.Tasks.Load();
                db.Statuses.Load();
                db.Employees.Load();
                Phases = db.Projects.Include(e => e.Phases).Single(e => e.Id == Project.Id).Phases.ToList();
            }
        }
        private bool CanAddPhaseProjectCommandExecute(object p) => true;
        #endregion

        #region AddTaskProjectCommand - добавление задачи
        public ICommand AddTaskProjectCommand { get; }
        private void OnAddTaskProjectCommandExecuted(object p)
        {
            App.Services.GetRequiredService<IUserDialog>().OpenAddTaskWindow();
            using (ProjectManagementContext db = new())
            {
                db.Tasks.Load();
                db.Statuses.Load();
                db.Employees.Load();
                Phases = db.Projects.Include(e => e.Phases).Single(e => e.Id == Project.Id).Phases.ToList();
            }
        }
        private bool CanAddTaskProjectCommandExecute(object p) => true;
        #endregion

        #region EditProjectCommand - изменить проект
        public ICommand EditProjectCommand { get; }
        private void OnEditProjectCommandExecuted(object p)
        {
            App.Services.GetRequiredService<IUserDialog>().OpenEditProjectWindow();
            using (ProjectManagementContext db = new())
            {
                db.Tasks.Load();
                db.Statuses.Load();
                db.Employees.Load();
                ChangingProject.project = db.Projects.Single(e => e.Id == ChangingProject.project.Id);
                Project = db.Projects.Include(e => e.Phases).Single(e => e.Id == ChangingProject.project.Id);
                Phases = db.Projects.Include(e => e.Phases).Single(e => e.Id == Project.Id).Phases.ToList();
            }
        }
        private bool CanEditProjectCommandExecute(object p) => true;
        #endregion

        #region EditTaskProjectCommand - изменение задачи
        public ICommand EditTaskProjectCommand { get; }
        private void OnEditTaskProjectCommandExecuted(object p)
        {
            ChangingTask.task = p as Task; 
            App.Services.GetRequiredService<IUserDialog>().OpenEditTaskWindow();
            ChangingTask.task = null;
            using (ProjectManagementContext db = new())
            {
                db.Tasks.Load();
                db.Statuses.Load();
                db.Employees.Load();
                Phases = db.Projects.Include(e => e.Phases).Single(e => e.Id == Project.Id).Phases.ToList();
            }
        }
        private bool CanEditTaskProjectCommandExecute(object p) => p is Task;
        #endregion

        #region EditPhaseCommand - изменение задачи
        public ICommand EditPhaseCommand { get; }
        private void OnEditPhaseCommandExecuted(object p)
        {
            ChangingPhase.phase = p as Phase;
            App.Services.GetRequiredService<IUserDialog>().OpenEditPhaseWindow();
            ChangingPhase.phase = null;
            using (ProjectManagementContext db = new())
            {
                db.Tasks.Load();
                db.Statuses.Load();
                db.Employees.Load();
                Phases = db.Projects.Include(e => e.Phases).Single(e => e.Id == Project.Id).Phases.ToList();
            }
        }
        private bool CanEditPhaseCommandExecute(object p) => p is Phase;
        #endregion


        #region DeleteSelectEmployeesCommand - команда удаления сотрудника из проекта
        public ICommand DeleteSelectEmployeesCommand { get; }
        private void OnDeleteSelectEmployeesCommandExecuted(object p)
        {
            if (p is Employee emp)
            {
                try
                {
                    if (MessageBox.Show($"Вы уверены, что хотите удалить {emp.Surname} {emp.Name} {emp.Patronymic} из проекта \"{Project.Name}\"", "Подтверждение",
                        MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
                    {
                        using (ProjectManagementContext db = new())
                        {
                            db.Tasks.Load();
                            db.Phases.Load();
                            db.Statuses.Load();
                            db.Projects.Load();
                            db.Employees.Load();
                            Project pr = db.Projects.Include(e => e.Phases).Where(e => e.Id == Project.Id).First();

                            if (!pr.Phases.Where(e => e.Tasks.Where(i => i.Executor == emp.Id).Any()).Any())
                            {
                                
                                db.Employees.Where(e => e.Id == emp.Id).Include(e => e.Projects).Single().Projects.Remove(pr);
                                db.Notifications.Add(new Notification()
                                {
                                    Employee = emp.Id,
                                    Id = (db.Notifications.Any()) ? db.Notifications.Max(e => e.Id) + 1 : 1,
                                    Text = "Вы были исключены из проекта " + Project.Name,
                                    Date = DateOnly.FromDateTime(DateTime.Now),
                                    New = true
                                });
                                db.SaveChanges();
                                EmployeesProject.Clear();
                                db.Employees.Where(e => e.Projects.Contains(pr)).ToList().ForEach(EmployeesProject.Add);
                            }
                            else
                            {
                                MessageBox.Show("Нельзя удалить из проекта сотрудника, для которого назначены задачи", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        private bool CanDeleteSelectEmployeesCommandExecute(object p) => true;
        #endregion
        #endregion

        public ProjectWindowViewModel()
        {
            PlusDateCommand = new RelayCommand(OnPlusDateCommandExecuted, CanPlusDateCommandExecute);
            MinusDateCommand = new RelayCommand(OnMinusDateCommandExecuted, CanMinusDateCommandExecute);
            PressPlusDateCommand = new RelayCommand(OnPressPlusDateCommandExecuted, CanPressPlusDateCommandExecute);
            PressMinusDateCommand = new RelayCommand(OnPressMinusDateCommandExecuted, CanPressMinusDateCommandExecute);

            CompletProjectCommand = new RelayCommand(OnCompletProjectCommandExecuted, CanCompletProjectCommandExecute);
            AddPhaseProjectCommand = new RelayCommand(OnAddPhaseProjectCommandExecuted, CanAddPhaseProjectCommandExecute);
            AddTaskProjectCommand = new RelayCommand(OnAddTaskProjectCommandExecuted, CanAddTaskProjectCommandExecute);
            EditTaskProjectCommand = new RelayCommand(OnEditTaskProjectCommandExecuted, CanEditTaskProjectCommandExecute);
            AddEmployeesProjectCommand = new RelayCommand(OnAddEmployeesProjectCommandExecuted, CanAddEmployeesProjectCommandExecute);
            DeleteSelectEmployeesCommand = new RelayCommand(OnDeleteSelectEmployeesCommandExecuted, CanDeleteSelectEmployeesCommandExecute);
            EditPhaseCommand = new RelayCommand(OnEditPhaseCommandExecuted, CanEditPhaseCommandExecute);
            EditProjectCommand = new RelayCommand(OnEditProjectCommandExecuted, CanEditProjectCommandExecute);
            DeletePhaseCommand = new RelayCommand(OnDeletePhaseCommandExecuted, CanDeletePhaseCommandExecute);

            DownloadeCommand = new RelayCommand(OnDownloadeCommandExecuted, CanDownloadeCommandExecute);

            _visibilityAdmin = CurrentEmployee.currentEmployee.Admin ? Visibility.Visible : Visibility.Collapsed;
            Project = ChangingProject.project;
            NameProject = Project.Name;
            using (ProjectManagementContext db = new ProjectManagementContext())
            {
                db.Projects.Load();
                db.Tasks.Load();
                db.Statuses.Load();
                db.Employees.Where(e => e.Projects.Contains(Project)).ToList().ForEach(EmployeesProject.Add);
                Phases = db.Projects.Include(e => e.Phases).Single(e => e.Id == Project.Id).Phases.ToList();
                if (DateOnly.FromDateTime(DateTime.Now) > db.Projects.Include(e => e.Phases).Single(e => e.Id == Project.Id).StartDate) DateStart = DateOnly.FromDateTime(DateTime.Now);
                else DateStart = db.Projects.Include(e => e.Phases).Single(e => e.Id == Project.Id).StartDate;
                
            }

            VisibilityButtons = (Project.Completed) ? Visibility.Collapsed : Visibility.Visible; 

        }
    }
}
