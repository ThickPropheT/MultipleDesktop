using System;
using System.ComponentModel;

namespace MultipleDesktop.Mvc
{
    public class PropertyChangedBinding : IPropertyChangedBinding
    {
        private readonly INotifyPropertyChanged _source;
        private readonly Action _target;

        public PropertyChangedBinding(INotifyPropertyChanged source, Action target)
        {
            source.PropertyChanged += Source_PropertyChanged;

            _source = source;
            _target = target;
        }

        public void Unbind()
        {
            _source.PropertyChanged -= Source_PropertyChanged;
        }

        private void Source_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _target();
        }
    }
}
