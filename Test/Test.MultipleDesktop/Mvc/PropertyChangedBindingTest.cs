using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;
using Moq;
using MultipleDesktop.Mvc;
using Should.Fluent;

namespace Test.MultipleDesktop.Mvc
{
    [TestClass]
    public class PropertyChangedBindingTest
    {
        public const string AnyString = "MOCK_VALUE";

        [TestClass]
        public sealed class WhenPropertyChanges : PropertyChangedBindingTest
        {
            [TestMethod]
            public void ShouldExecuteCallback()
            {
                var sourceMock = new Mock<INotifyPropertyChanged>();

                bool invoked = false;
                Action action = () => invoked = true;

                var binding = new PropertyChangedBinding(sourceMock.Object, action);

                sourceMock.Raise(source => source.PropertyChanged += null, sourceMock.Object, new PropertyChangedEventArgs(AnyString));

                invoked.Should().Be.True();
            }
        }

        [TestClass]
        public sealed class WhenUnbound : PropertyChangedBindingTest
        {
            [TestMethod]
            public void SourceShouldBeNull()
            {
                var sourceMock = new Mock<INotifyPropertyChanged>();

                var binding = new PropertyChangedBinding(sourceMock.Object, null);
                binding.Unbind();

                binding.Source.Should().Be.Null();
            }

            [TestMethod]
            public void ShouldNotExecuteCallback()
            {
                var sourceMock = new Mock<INotifyPropertyChanged>();

                bool invoked = false;
                Action action = () => invoked = true;

                var binding = new PropertyChangedBinding(sourceMock.Object, action);
                binding.Unbind();

                sourceMock.Raise(source => source.PropertyChanged += null, sourceMock.Object, new PropertyChangedEventArgs(AnyString));

                invoked.Should().Be.False();
            }
        }
    }
}
