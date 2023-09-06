namespace ProjectManagement.Services
{
    public interface IUserDialog
    {
        void OpenMainWindow();
        void CloseMainWindow();
        void OpenNewEmployeeWindow();
        void OpenAuthorizationWindow();
        void OpenAddDepartmentWindow();
        void OpenEditDepartmentWindow();
        void OpenAddPostWindow();
        void OpenEditPostWindow();
        void OpenAddEmployeeWindow();
        void OpenEditEmployeeWindow();
        void OpenAddProjectWindow();
        void OpenProjectWindow();
        void OpenEditEmployeesProjectWindow();
        void OpenAddPhaseWindow();
        void OpenAddTaskWindow();
        void OpenAddEmployeeTaskWindow();
        void OpenEditTaskWindow();
        void OpenEditPhaseWindow();
        void OpenEditProjectWindow();
    }
}
