using ProjectManagement.Infrastructure.Commands;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProjectManagement.ViewModels
{
    class ViewModelHeaderWindowUI : ViewModel
    {
        #region Поля
        #region HideBtnToolTipText : String - текст для ToolTip кнопки смены состояния окна
        private String _HideBtnToolTipText = "Развернуть";
        public String HideBtnToolTipText
        {
            get => _HideBtnToolTipText;
            set => Set(ref _HideBtnToolTipText, value);
        }
        #endregion
        #region HideBtnToolTipIcon : String - название иконки кнопки смены состояния окна
        private String _HideBtnToolTipIcon = "Regular_WindowMaximize";
        public String HideBtnToolTipIcon
        {
            get => _HideBtnToolTipIcon;
            set => Set(ref _HideBtnToolTipIcon, value);
        }
        #endregion
        #endregion

        #region Команды
        #region CloseWindowCommand - команда закрытия окна
        public ICommand CloseWindowCommand { get; }
        private void OnCloseWindowCommandExecuted(object p) => Window.GetWindow((Button)p).Close();
        private bool CanCloseWindowCommandExecute(object p) => true;
        #endregion
        #region HideWindowCommand - команда сворачивания окна
        public ICommand HideWindowCommand { get; }
        private void OnHideWindowCommandExecuted(object p) => Window.GetWindow((Button)p).WindowState = WindowState.Minimized;
        private bool CanHideWindowCommandExecute(object p) => true;
        #endregion
        #region EditStateWindowCommand - команда смены состояния окна
        public ICommand EditStateWindowCommand { get; }
        private void OnEditStateWindowCommandExecuted(object p)
        {
            Window win = Window.GetWindow((Button)p);
            if (win.WindowState == WindowState.Normal)
            {
                win.WindowState = WindowState.Maximized;
            }
            else
            {
                win.WindowState = WindowState.Normal;
            }
        }
        private bool CanEditStateWindowCommandExecute(object p) => true;
        #endregion
        #endregion

        #region Конструкторы
        public ViewModelHeaderWindowUI()
        {
            #region Команды
            CloseWindowCommand = new RelayCommand(OnCloseWindowCommandExecuted, CanCloseWindowCommandExecute);
            HideWindowCommand = new RelayCommand(OnHideWindowCommandExecuted, CanHideWindowCommandExecute);
            EditStateWindowCommand = new RelayCommand(OnEditStateWindowCommandExecuted, CanEditStateWindowCommandExecute);
            #endregion
        }
        #endregion
    }
}