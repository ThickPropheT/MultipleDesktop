﻿using MultipleDesktop.Mvc.Configuration;
using MultipleDesktop.Mvc.Desktop;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using MultipleDesktop.Mvc;
using XmlTag = MultipleDesktop.Configuration.Xml.Tag.VirtualDesktopConfiguration;

namespace MultipleDesktop.Configuration.Xml
{
    [XmlType(XmlTag.Name)]
    public class VirtualDesktopConfiguration : IVirtualDesktopConfiguration
    {
        private IConfigurationFactory _configurationFactory;

        private IPropertyChangedBinding _propertyChangedBinding;

        private FilePath _backgroundPath;
        private Action<FilePath> _backgroundPathSetter;

        private Fit _fit;
        private Action<Fit> _fitSetter;

        [XmlAttribute(XmlTag.Guid)]
        public Guid Guid { get; set; }

        [XmlElement(XmlTag.BackgroundPath)]
        public string BackgroundPathElement
        {
            get { return _backgroundPath; }
            set { _backgroundPath = value; }
        }

        [XmlIgnore]
        public FilePath BackgroundPath
        {
            get { return _backgroundPath; }
            set { _backgroundPathSetter(value); }
        }

        private void SetBackgroundPath(FilePath value)
        {
            if (Equals(_backgroundPath, value))
                return;

            _backgroundPath = value;

            TargetDesktop.Background =
                    _configurationFactory
                        .BackgroundFrom(value, _fit);
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
            set { _fitSetter(value); }

        }

        private void SetFit(Fit value)
        {
            if (Equals(_fit, value))
                return;

            _fit = value;

            TargetDesktop.Background =
                _configurationFactory
                    .BackgroundFrom(_backgroundPath, value);
        }

        [XmlIgnore]
        public IVirtualDesktop TargetDesktop { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Xml serialization constructor.
        /// </summary>
        public VirtualDesktopConfiguration()
        {
            _backgroundPathSetter = value => ThrowWhenDeserializing(nameof(BackgroundPath));
            _fitSetter = value => ThrowWhenDeserializing(nameof(Fit));
        }

        public VirtualDesktopConfiguration(IVirtualDesktop targetDesktop, IConfigurationFactory factory)
        {
            Guid = targetDesktop.Guid;

            var background = targetDesktop.Background;

            _backgroundPath = background.Path;
            _fit = background.Fit;

            TargetDesktop = targetDesktop;

            _backgroundPathSetter = SetBackgroundPath;
            _fitSetter = SetFit;

            _propertyChangedBinding = factory.Bind(UpdateFromTarget, targetDesktop);

            _configurationFactory = factory;
        }

        // TODO
        //public VirtualDesktopConfiguration(IVirtualDesktop targetDesktop, IConfigurationFactory factory)
        //{
        //    BindToTarget(targetDesktop, factory);
        //    // should take values from desktop
        //    UpdateFromTarget();
        //}

        public void BindToTarget(IVirtualDesktop value, IConfigurationFactory factory)
        {
            _propertyChangedBinding?.Unbind();

            TargetDesktop = value;
            _configurationFactory = factory;

            if (value == null)
            {
                _backgroundPathSetter = path => ThrowWhenUnbound(nameof(BackgroundPath));
                _fitSetter = fit => ThrowWhenUnbound(nameof(Fit));

                return;
            }

            _backgroundPathSetter = SetBackgroundPath;
            _fitSetter = SetFit;

            _propertyChangedBinding = factory.Bind(UpdateFromTarget, value);

            var updateTargetBackground = true;

            // TODO this should have tests for it causing a return
            if (!_backgroundPath.HasValue)
            {
                _backgroundPath = value.Background.Path;
                updateTargetBackground = false;
            }

            // TODO be sure to fully test that this returns
            if (_fit.Equals(default(Fit)))
            {
                _fit = value.Background.Fit;
                updateTargetBackground = false;
            }

            if (!updateTargetBackground)
                return;

            value.Background = factory.BackgroundFrom(_backgroundPath, _fit);
        }

        [SuppressMessage("ReSharper", "ExplicitCallerInfoArgument")]
        public void UpdateFromTarget()
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

        private void ThrowWhenDeserializing(string propertyName)
        {
            throw NoTargetBoundException.InvocationRequiresTarget(
                propertyName,
                $"After Xml deserialization is complete, call '{nameof(BindToTarget)}' to enable setter for '{propertyName}'.");
        }

        private void ThrowWhenUnbound(string propertyName)
        {
            throw NoTargetBoundException.InvocationRequiresTarget(
                propertyName,
                $"{nameof(VirtualDesktopConfiguration)} has been unbound. Bind it to an {nameof(IVirtualDesktop)} using the {nameof(BindToTarget)} method.");
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"{Guid}, {BackgroundPath}, {Fit}";
        }
    }
}
