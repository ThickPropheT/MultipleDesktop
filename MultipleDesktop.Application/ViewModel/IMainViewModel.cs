using System.Windows.Input;

namespace MultipleDesktop.Application.ViewModel
{
    public interface IMainViewModel
    {
        string Title { get; }

        bool CanMinimize { get; }
        bool CanMaximize { get; }

        ICommand LoadCommand { get; }
    }
}
