using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultipleDesktop.Mvc.Configuration;
using Should.Fluent;
using System.Xml;
using System.IO;
using VisualStudio.TestTools.UnitTesting;

namespace Test.MultipleDesktop.Configuration
{
    [TestClass]
    public class IoResultTest
    {
        [TestClass]
        public sealed class WhenResultIsForSuccess : IoResultTest
        {
            private IoResult _result;

            [TestInitialize]
            public void UsingThisConfiguration()
            {
                _result = IoResult.ForSuccess();
            }

            [TestMethod]
            public void DidFailShouldBeFalse()
            {
                _result.DidFail.Should().Be.False();
            }

            [TestMethod]
            public void ExceptionShouldBeNull()
            {
                _result.Exception.Should().Be.Null();
            }

            [TestMethod]
            public void DoesExistShouldBeTrue()
            {
                _result.DoesExist.Should().Be.True();
            }

            [TestMethod]
            public void ReadErrorShouldBeFalse()
            {
                _result.ReadError.Should().Be.False();
            }
        }

        [TestClass]
        public sealed class WhenResultIsForReadError : IoResultTest
        {
            public static readonly InvalidOperationException Error = new InvalidOperationException(string.Empty, new XmlException());

            private IoResult _result;

            [TestInitialize]
            public void UsingThisConfiguration()
            {
                _result = IoResult.ForReadError(Error);
            }

            [TestMethod]
            public void DidFailShouldBeTrue()
            {
                _result.DidFail.Should().Be.True();
            }

            [TestMethod]
            public void ExceptionShouldBeError()
            {
                _result.Exception.Should().Be.SameAs(Error);
            }

            [TestMethod]
            public void DoesExistShouldBeNull()
            {
                _result.DoesExist.Should().Be.Null();
            }

            [TestMethod]
            public void ReadErrorShouldBeTrue()
            {
                _result.ReadError.Should().Be.True();
            }
        }

        [TestClass]
        public abstract class WhenResultIsForNotFound : IoResultTest
        {
            protected abstract IOException Error { get; }

            private IoResult _result;

            [TestInitialize]
            public void UsingThisConfiguration()
            {
                _result = IoResult.ForNotFound(Error);
            }

            [TestMethod]
            public abstract void DidFailShouldBeTrue();
            [TestMethod]
            public abstract void ExceptionShouldBeError();
            [TestMethod]
            public abstract void DoesExistShouldBeFalse();
            [TestMethod]
            public abstract void ReadErrorShouldBeNull();

            protected void DidFailShouldBeTrueImpl()
            {
                _result.DidFail.Should().Be.True();
            }

            protected void ExceptionShouldBeErrorImpl()
            {
                _result.Exception.Should().Be.SameAs(Error);
            }

            protected void DoesExistShouldBeFalseImpl()
            {
                _result.DoesExist.Should().Be.False();
            }

            protected void ReadErrorShouldBeNullImpl()
            {
                _result.ReadError.Should().Be.Null();
            }

            [TestClass]
            public sealed class WhenFileNotFound : WhenResultIsForNotFound
            {
                public static readonly FileNotFoundException FileNotFoundError = new FileNotFoundException();

                protected override IOException Error => FileNotFoundError;

                [TestMethod]
                public override void DidFailShouldBeTrue()
                {
                    DidFailShouldBeTrueImpl();
                }

                [TestMethod]
                public override void DoesExistShouldBeFalse()
                {
                    DoesExistShouldBeFalseImpl();
                }

                [TestMethod]
                public override void ExceptionShouldBeError()
                {
                    ExceptionShouldBeErrorImpl();
                }

                [TestMethod]
                public override void ReadErrorShouldBeNull()
                {
                    ReadErrorShouldBeNullImpl();
                }
            }

            [TestClass]
            public sealed class WhenDirectoryNotFound : WhenResultIsForNotFound
            {
                public static readonly DirectoryNotFoundException DirectoryNotFoundError = new DirectoryNotFoundException();

                protected override IOException Error => DirectoryNotFoundError;

                [TestMethod]
                public override void DidFailShouldBeTrue()
                {
                    DidFailShouldBeTrueImpl();
                }

                [TestMethod]
                public override void DoesExistShouldBeFalse()
                {
                    DoesExistShouldBeFalseImpl();
                }

                [TestMethod]
                public override void ExceptionShouldBeError()
                {
                    ExceptionShouldBeErrorImpl();
                }

                [TestMethod]
                public override void ReadErrorShouldBeNull()
                {
                    ReadErrorShouldBeNullImpl();
                }
            }
        }

        [TestClass]
        public sealed class WhenResultIsForAnyException : IoResultTest
        {
            public static readonly ExceptionMock Error = new ExceptionMock();

            private IoResult _result;

            [TestInitialize]
            public void UsingThisConfiguration()
            {
                _result = IoResult.ForException(Error);
            }

            [TestMethod]
            public void DidFailShouldBeTrue()
            {
                _result.DidFail.Should().Be.True();
            }

            [TestMethod]
            public void ExceptionShouldBeError()
            {
                _result.Exception.Should().Be.SameAs(Error);
            }

            [TestMethod]
            public void DoesExistShouldBeNull()
            {
                _result.DoesExist.Should().Be.Null();
            }

            [TestMethod]
            public void ReadErrorShouldBeNull()
            {
                _result.ReadError.Should().Be.Null();
            }
        }
    }
}
