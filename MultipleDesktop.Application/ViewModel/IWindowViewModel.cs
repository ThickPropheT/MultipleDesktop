namespace MultipleDesktop.Application.ViewModel
{
    public interface IWindowViewModel
    {
        bool CanMinimize { get; set; }
        bool CanMaximize { get; set; }
    }
}
