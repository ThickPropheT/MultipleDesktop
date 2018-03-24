namespace MultipleDesktop.Application.ViewModel
{
    public class MainViewModel : IMainViewModel, IWindowViewModel
    {
        public string Title => "Virtual Desktop";

        public bool CanMinimize { get; set; }
        public bool CanMaximize { get; set; }
    }
}
