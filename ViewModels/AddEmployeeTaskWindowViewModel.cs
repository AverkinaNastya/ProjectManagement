using Microsoft.EntityFrameworkCore;
using ProjectManagement.Infrastructure.Classes;
using ProjectManagement.Infrastructure.Commands;
using ProjectManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjectManagement.ViewModels
{
    class AddEmployeeTaskWindowViewModel: ViewModel
    {

        private Employee[] _employees;
        public Employee[] Employees
        {
            get => _employees;
            set => Set(ref _employees, value);
        }

        private Employee? _selectEmployee;
        public Employee? SelectEmployee
        {
            get => _selectEmployee;
            set => Set(ref _selectEmployee, value);
        }

        private String _filterStr;
        public String FilterStr
        {
            get => _filterStr;
            set
            {
                Set(ref _filterStr, value);
                using (ProjectManagementContext db = new())
                {
                    Project pr = db.Projects.Include(e => e.Employees).Single(e => e.Id == ChangingProject.project.Id);
                    Employees = pr.Employees.Where(e => !e.Blocked).ToArray();
                    Employees = Employees.Where(e => (e.Surname.StartsWith(FilterStr) || LevenshteinDistance.Distance(e.Surname, FilterStr) <= 3 ||
                                                     e.Name.StartsWith(FilterStr) || LevenshteinDistance.Distance(e.Name, FilterStr) <= 3 ||
                                                     (e.Patronymic != null && (e.Patronymic.StartsWith(FilterStr) || LevenshteinDistance.Distance(e.Patronymic, FilterStr) <= 3)))).ToArray();
                }
            }
        }

        #region AddEmployeeCommand - команда передачи сотрудника
        public ICommand AddEmployeeCommand { get; }
        private void OnAddEmployeeCommandExecuted(object p)
        {
            ChangingEmployee.changingEmployee = SelectEmployee;
            OnDialogComplete(EventArgs.Empty);
        }
        private bool CanAddEmployeeCommandExecute(object p) => SelectEmployee != null;
        #endregion

        #region BackCommand - команда отмены
        public ICommand BackCommand { get; }
        private void OnBackCommandExecuted(object p) => OnDialogComplete(EventArgs.Empty);
        private bool CanBackCommandExecute(object p) => true;
        #endregion

        public AddEmployeeTaskWindowViewModel()
        {
            BackCommand = new RelayCommand(OnBackCommandExecuted, CanBackCommandExecute);
            AddEmployeeCommand = new RelayCommand(OnAddEmployeeCommandExecuted, CanAddEmployeeCommandExecute);

            using (ProjectManagementContext db = new())
            {
                Project pr = db.Projects.Include(e => e.Employees).Single(e => e.Id == ChangingProject.project.Id);
                Employees = pr.Employees.Where(e => !e.Blocked).ToArray();
            }
        }
    }
}
