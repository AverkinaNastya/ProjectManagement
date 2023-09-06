using ProjectManagement.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ProjectManagement.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для LeftMenuBarUI.xaml
    /// </summary>
    public partial class LeftMenuBarUI : UserControl
    {
        public LeftMenuBarUI()
        {
            InitializeComponent();

            Loaded += LeftMenuBarUI_Loaded;
        }

        private void LeftMenuBarUI_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new LeftMenuBarUIViewModel(new ViewModelsResolver());
        }
    }
}
