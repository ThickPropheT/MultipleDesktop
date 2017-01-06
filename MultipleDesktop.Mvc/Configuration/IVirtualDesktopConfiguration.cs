﻿using MultipleDesktop.Mvc.Desktop;
using System;
using System.ComponentModel;

namespace MultipleDesktop.Mvc.Configuration
{
    public interface IVirtualDesktopConfiguration : INotifyPropertyChanged
    {
        Guid Guid { get; set; }

        string BackgroundPath { get; set; }

        Fit Fit { get; set; }

        IVirtualDesktop TargetDesktop { get; }

        void BindToTarget(IVirtualDesktop target);
    }
}
