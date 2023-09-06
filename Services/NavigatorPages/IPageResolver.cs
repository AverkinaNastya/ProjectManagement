using System.Windows.Controls;

namespace ProjectManagement.Services.NavigatorPages
{
    interface IPageResolver
    {
        Page GetPageInstance(string alias);
    }
}
