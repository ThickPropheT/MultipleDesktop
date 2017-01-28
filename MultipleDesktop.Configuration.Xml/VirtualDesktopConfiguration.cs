﻿using MultipleDesktop.Mvc.Configuration;
using MultipleDesktop.Mvc.Desktop;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace MultipleDesktop.Configuration.Xml
{
    [XmlType("desktop")]
    public class VirtualDesktopConfiguration : IVirtualDesktopConfiguration
    {
        private IConfigurationFactory _configurationFactory;

        private FilePath _backgroundPath;
        private Fit _fit;

        [XmlAttribute("guid")]
        public Guid Guid { get; set; }

        [XmlElement("background-path")]
        public FilePath BackgroundPathElement
        {
            get { return _backgroundPath; }
            set { _backgroundPath = value; }
        }

        [XmlIgnore]
        public FilePath BackgroundPath
        {
            get { return _backgroundPath; }
            set
            {
                if (Equals(_backgroundPath, value))
                    return;

                _backgroundPath = value;

                TargetDesktop.Background =
                    _configurationFactory
                        .BackgroundFrom(value, _fit);
            }
        }

        [XmlElement("background-fit")]
        public Fit FitElement
        {
            get { return _fit; }
            set { _fit = value; }
        }

        [XmlIgnore]
        public Fit Fit
        {
            get { return _fit; }
            set
            {
                if (Equals(_fit, value))
                    return;

                _fit = value;

                TargetDesktop.Background =
                    _configurationFactory
                        .BackgroundFrom(_backgroundPath, value);
            }
        }

        [XmlIgnore]
        public IVirtualDesktop TargetDesktop { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Xml serialization constructor.
        /// </summary>
        public VirtualDesktopConfiguration() { }

        public VirtualDesktopConfiguration(IVirtualDesktop targetDesktop, IConfigurationFactory factory)
        {
            Guid = targetDesktop.Guid;

            var background = targetDesktop.Background;

            _backgroundPath = background.Path;
        }

        //public VirtualDesktopConfiguration(IVirtualDesktop targetDesktop, IConfigurationFactory factory)
        //{
        //    BindToTarget(targetDesktop, factory);
        //    // should take values from desktop
        //    UpdateFromTarget();
        //}

        public void BindToTarget(IVirtualDesktop value, IConfigurationFactory factory)
        {
            _configurationFactory = factory;

            if (TargetDesktop != null)
            {
                TargetDesktop.PropertyChanged -= Target_PropertyChanged;
            }

            TargetDesktop = value;

            if (value != null)
            {
                value.PropertyChanged += Target_PropertyChanged;

                value.Background = factory.BackgroundFrom(_backgroundPath, _fit);
            }
        }

        private void Target_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateFromTarget();
        }

        private void UpdateFromTarget()
        {
            Guid = TargetDesktop.Guid;

            var background = TargetDesktop.Background;

            var updatedBackgroundPath = background.Path;

            if (!Equals(_backgroundPath, updatedBackgroundPath))
            {
                _backgroundPath = updatedBackgroundPath;

                OnPropertyChanged(nameof(BackgroundPath));
            }

            var updatedFit = background.Fit;

            if (!Equals(_fit, updatedFit))
            {
                _fit = updatedFit;

                OnPropertyChanged(nameof(Fit));
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"{Guid.ToString()}, {BackgroundPath}, {Fit}";
        }
    }
}
