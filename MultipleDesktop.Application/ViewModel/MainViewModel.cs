using MultipleDesktop.Mvc.Controller;
using System;
using System.Windows.Input;
using System.Windows.Input.Extended;

namespace MultipleDesktop.Application.ViewModel
{
    public class MainViewModel : IMainViewModel, IWindowViewModel
    {
        public string Title => "Virtual Desktop";

        public bool CanMinimize { get; set; }
        public bool CanMaximize { get; set; }

        public ICommand LoadCommand { get; }

        public MainViewModel(IAppController controller)
        {
            LoadCommand = new DelegateCommand(() => controller.Load());
        }
    }
}
