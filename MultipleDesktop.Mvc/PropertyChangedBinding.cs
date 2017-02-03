using System;
using System.ComponentModel;

namespace MultipleDesktop.Mvc
{
    public class PropertyChangedBinding : IPropertyChangedBinding
    {
        private readonly Action _target;

        public INotifyPropertyChanged Source { get; private set; }

        public PropertyChangedBinding(INotifyPropertyChanged source, Action target)
        {
            source.PropertyChanged += Source_PropertyChanged;

            Source = source;
            _target = target;
        }

        public void Unbind()
        {
            Source.PropertyChanged -= Source_PropertyChanged;
            Source = null;
        }

        private void Source_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _target();
        }
    }
}
