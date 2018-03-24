using MultipleDesktop.Application.ViewModel;

namespace MultipleDesktop.Application.View
{
    public interface IMainView
    {
        IMainViewModel ViewModel { get; set; }
    }
}
