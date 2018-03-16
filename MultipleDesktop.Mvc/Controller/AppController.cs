using MultipleDesktop.Mvc.Configuration;
using MultipleDesktop.Mvc.Desktop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace MultipleDesktop.Mvc.Controller
{
    public class AppController : IAppController
    {
        #region AllDesktopsChanging

        private class WhenAllDesktopsHasChanged
        {
            private readonly IVirtualDesktopState _provider;

            private readonly IEnumerable<IVirtualDesktop> _changing;

            private Action<IEnumerable<IVirtualDesktop>, IEnumerable<IVirtualDesktop>> _completionCallback;

            private WhenAllDesktopsHasChanged(IVirtualDesktopState provider)
            {
                _provider = provider;

                _changing = _provider.AllDesktops;
            }

            public static WhenAllDesktopsHasChanged For(IVirtualDesktopState provider)
                => new WhenAllDesktopsHasChanged(provider);

            public void DoCallback(Action<IEnumerable<IVirtualDesktop>, IEnumerable<IVirtualDesktop>> completionCallback)
            {
                _completionCallback = completionCallback;

                _provider.PropertyChanged += Provider_PropertyChanged;
            }

            private void Provider_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                switch (e.PropertyName)
                {
                    case nameof(_provider.AllDesktops):
                        _provider.PropertyChanged -= Provider_PropertyChanged;

                        _completionCallback(_changing, _provider.AllDesktops);
                        break;
                }
            }
        }

        #endregion AllDesktopsChanging

        private readonly IFileSystem _fileSystem;
        private readonly IVirtualDesktopState _desktopState;
        private readonly IConfigurationController _configurationController;
        private readonly IConfigurationFactory _configurationFactory;

        private IEnumerable<IVirtualDesktopConfiguration> _desktopConfigurations =
            Enumerable.Empty<IVirtualDesktopConfiguration>();

        public IEnumerable<IVirtualDesktopConfiguration> DesktopConfigurations
        {
            get { return _desktopConfigurations; }
            set
            {
                if (value == null || _desktopConfigurations.SequenceEqual(value))
                    return;

                _desktopConfigurations = value;

                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public AppController(
            IFileSystem fileSystem,
            IVirtualDesktopState desktopProvider,
            IConfigurationController configurationController,
            IConfigurationFactory configurationFactory)
        {
            _fileSystem = fileSystem;

            _desktopState = desktopProvider;
            desktopProvider.PropertyChanging += DesktopProvider_PropertyChanging;

            _configurationController = configurationController;
            _configurationFactory = configurationFactory;
        }

        private void DesktopProvider_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (changingArgs.PropertyName != nameof(_desktopState.AllDesktops))
                return;

            WhenAllDesktopsHasChanged.For(_desktopState)
                .DoCallback(UpdateDesktopConfigurations);
        }

        /// <summary>
        /// 
        /// Updates <see cref="_desktopConfigurations"/> with <see cref="IVirtualDesktopConfiguration"/>s
        /// corresponding to <see cref="IVirtualDesktop"/>s within <paramref name="after"/>.
        /// 
        /// <see cref="IVirtualDesktop"/>s with no matched <see cref="IVirtualDesktopConfiguration"/> will be bound
        /// to one created by <see cref="IConfigurationFactory.ConfigurationFor(IVirtualDesktop)"/>.
        /// 
        /// <see cref="IVirtualDesktopConfiguration"/>s with no matched <see cref="IVirtualDesktop"/> will be bound
        /// to <see langword="null"/> (i.e. unbound).
        /// 
        /// </summary>
        /// <param name="before"></param>
        /// <param name="after"></param>
        private void UpdateDesktopConfigurations(IEnumerable<IVirtualDesktop> before, IEnumerable<IVirtualDesktop> after)
        {
            var pairs = _desktopConfigurations
                .DefaultIfEmpty()
                .Join(
                    after.DefaultIfEmpty(),

                    configuration => configuration.Guid, desktop => desktop.Guid,

                    (configuration, desktop) =>
                    new
                    {
                        Desktop = desktop,

                        Configuration =
                            configuration
                            // when desktop does not have matching configuration, one is created.
                            ?? _configurationFactory.ConfigurationFor(desktop)
                    })
                .ToArray();

            foreach (var pair in pairs)
            {
                // should NOT overwrite configuration values
                // when pair.Desktop is null (did not match an enabled desktop)
                // the configuration becomes unbound.
                pair.Configuration.BindToTarget(pair.Desktop, _configurationFactory);
            }

            DesktopConfigurations = pairs.Select(pair => pair.Configuration);
        }

        public void Load()
        {
            var desktopConfigurations =
                _configurationController.Load()
                    .GetAll()
                    .ToList();

            MapConfigurationToVirtualDesktop(
                _desktopState.AllDesktops,
                desktopConfigurations,
                _configurationFactory);

            DesktopConfigurations = desktopConfigurations;

            _desktopState.Load();
        }

        private static void MapConfigurationToVirtualDesktop(
            IEnumerable<IVirtualDesktop> desktops,
            IList<IVirtualDesktopConfiguration> configurations,
            IConfigurationFactory configurationFactory)
        {

            foreach (var desktop in desktops)
            {
                var matchingConfiguration = configurations.FirstOrDefault(
                    configuration =>
                        configuration.IsConfigurationFor(desktop));

                if (matchingConfiguration != null)
                    // should NOT overwrite configuration values
                    matchingConfiguration.BindToTarget(desktop, configurationFactory);

                else
                    configurations.Add(configurationFactory.ConfigurationFor(desktop));
            }
        }

        public void Save()
            => _configurationController.Save(
                _configurationFactory.AppConfigurationFrom(_desktopConfigurations));

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
