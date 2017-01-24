using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultipleDesktop.Mvc.Configuration;
using Should.Fluent;

namespace Test.MultipleDesktop.Mvc.Configuration
{
    [TestClass]
    public class FilePathTest
    {
        public const string StringWithCharacters = "MOCK_VALUE";

        [TestClass]
        public class WhenInitializing : FilePathTest
        {
            private FilePath _filePath;

            [TestClass]
            public sealed class WithDefaultConstructor : WhenInitializing
            {
                [TestInitialize]
                public void UsingThisConfiguration()
                {
                    _filePath = new FilePath();
                }

                [TestMethod]
                public void ValueShouldBeEmptyString()
                {
                    _filePath.Value.Should().Be.Empty();
                }

                [TestMethod]
                public void HasValueShouldBeFalse()
                {
                    _filePath.HasValue.Should().Be.False();
                }
            }

            [TestClass]
            public sealed class WhenValueIsNull : WhenInitializing
            {
                [TestInitialize]
                public void UsingThisConfiguration()
                {
                    _filePath = new FilePath(null);
                }

                [TestMethod]
                public void ValueShouldBeEmptyString()
                {
                    _filePath.Value.Should().Be.Empty();
                }

                [TestMethod]
                public void HasValueShouldBeFalse()
                {
                    _filePath.HasValue.Should().Be.False();
                }
            }

            [TestClass]
            public sealed class WhenValueIsEmpty : WhenInitializing
            {
                [TestInitialize]
                public void UsingThisConfiguration()
                {
                    _filePath = new FilePath(string.Empty);
                }

                [TestMethod]
                public void ValueShouldBeEmptyString()
                {
                    _filePath.Value.Should().Be.Empty();
                }

                [TestMethod]
                public void HasValueShouldBeFalse()
                {
                    _filePath.HasValue.Should().Be.False();
                }
            }

            [TestClass]
            public sealed class WhenValueHasCharacters : WhenInitializing
            {
                [TestInitialize]
                public void UsingThisConfiguration()
                {
                    _filePath = new FilePath(StringWithCharacters);
                }

                [TestMethod]
                public void ValueShouldHaveSameCharacters()
                {
                    _filePath.Value.Should().Equal(StringWithCharacters);
                }

                [TestMethod]
                public void HasValueShouldBeTrue()
                {
                    _filePath.HasValue.Should().Be.True();
                }
            }
        }

        [TestClass]
        public class WhenCasting : FilePathTest
        {
            [TestClass]
            public class FromStringToFilePath : WhenCasting
            {
                private FilePath _filePath;

                [TestClass]
                public sealed class WhenStringIsNull : FromStringToFilePath
                {
                    [TestInitialize]
                    public void UsingThisConfiguration()
                    {
                        _filePath = (FilePath)null;
                    }

                    [TestMethod]
                    public void ValueShouldBeEmptyString()
                    {
                        _filePath.Value.Should().Be.Empty();
                    }

                    [TestMethod]
                    public void HasValueShouldBeFalse()
                    {
                        _filePath.HasValue.Should().Be.False();
                    }
                }

                [TestClass]
                public sealed class WhenStringIsEmpty : FromStringToFilePath
                {
                    [TestInitialize]
                    public void UsingThisConfiguration()
                    {
                        _filePath = (FilePath)string.Empty;
                    }

                    [TestMethod]
                    public void ValueShouldBeEmptyString()
                    {
                        _filePath.Value.Should().Be.Empty();
                    }

                    [TestMethod]
                    public void HasValueShouldBeFalse()
                    {
                        _filePath.HasValue.Should().Be.False();
                    }
                }
            }

            [TestClass]
            public class FromFilePathToString : WhenCasting
            {
                [TestClass]
                public sealed class WhenFilePathIsDefault : FromFilePathToString
                {
                    [TestMethod]
                    public void StringShouldBeEmpty()
                    {
                        var @string = (string)new FilePath();

                        @string.Should().Be.Empty();
                    }
                }

                [TestClass]
                public sealed class WhenFilePathHasCharacters : FromFilePathToString
                {
                    [TestMethod]
                    public void StringShouldHaveSameCharacters()
                    {
                        var @string = (string)new FilePath(StringWithCharacters);

                        @string.Should().Equal(StringWithCharacters);
                    }
                }
            }
        }

        [TestClass]
        public class WhenComparing : FilePathTest
        {
            [TestClass]
            public class FilePathToString : WhenComparing
            {
                private FilePath _filePath;
                private string _string;

                [TestClass]
                public sealed class WhenFilePathIsDefault : FilePathToString
                {
                    [TestInitialize]
                    public void UsingThisConfiguration()
                    {
                        _filePath = new FilePath();
                    }

                    [TestMethod]
                    public void AndStringIsNull()
                    {
                        _string = null;

                        ShouldReturn(false);
                    }

                    [TestMethod]
                    public void AndStringIsEmpty()
                    {
                        _string = string.Empty;

                        ShouldReturn(true);
                    }

                    [TestMethod]
                    public void AndStringHasCharacters()
                    {
                        _string = StringWithCharacters;

                        ShouldReturn(false);
                    }
                }

                [TestClass]
                public sealed class WhenFilePathHasCharacters : FilePathToString
                {
                    [TestInitialize]
                    public void UsingThisConfiguration()
                    {
                        _filePath = new FilePath(StringWithCharacters);
                    }

                    [TestMethod]
                    public void AndStringIsNull()
                    {
                        _string = null;

                        ShouldReturn(false);
                    }

                    [TestMethod]
                    public void AndStringIsEmpty()
                    {
                        _string = string.Empty;

                        ShouldReturn(false);
                    }

                    [TestMethod]
                    public void AndStringHasSameCharacters()
                    {
                        _string = StringWithCharacters;

                        ShouldReturn(true);
                    }

                    [TestMethod]
                    public void AndStringHasDifferentCharacters()
                    {
                        _string = $"DIFFERENT_{StringWithCharacters}";

                        ShouldReturn(false);
                    }
                }

                private void ShouldReturn(bool value)
                {
                    var areEqual = _filePath.Equals(_string);

                    areEqual.Should().Equal(value);
                }
            }

            [TestClass]
            public class FilePathToFilePath : WhenComparing
            {
                private FilePath _filePath1;
                private FilePath _filePath2;

                [TestClass]
                public sealed class WhenFilePath1IsDefault : FilePathToFilePath
                {
                    [TestInitialize]
                    public void UsingThisConfiguration()
                    {
                        _filePath1 = new FilePath();
                    }

                    [TestMethod]
                    public void AndFilePath2IsDefault()
                    {
                        _filePath2 = new FilePath();

                        ShouldReturn(true);
                    }

                    [TestMethod]
                    public void AndFilePath2HasCharacters()
                    {
                        _filePath2 = new FilePath(StringWithCharacters);

                        ShouldReturn(false);
                    }
                }

                [TestClass]
                public sealed class WhenFilePath1HasCharacters : FilePathToFilePath
                {
                    [TestInitialize]
                    public void UsingThisConfiguration()
                    {
                        _filePath1 = new FilePath(StringWithCharacters);
                    }

                    [TestMethod]
                    public void AndFilePath2IsDefault()
                    {
                        _filePath2 = new FilePath();

                        ShouldReturn(false);
                    }

                    [TestMethod]
                    public void AndFilePath2HasSameCharacters()
                    {
                        _filePath2 = new FilePath(StringWithCharacters);

                        ShouldReturn(true);
                    }

                    [TestMethod]
                    public void AndFilePath2HasDifferentCharacters()
                    {
                        _filePath2 = new FilePath($"DIFFERENT_{StringWithCharacters}");

                        ShouldReturn(false);
                    }
                }

                private void ShouldReturn(bool value)
                {
                    var areEqual = _filePath1.Equals(_filePath2);

                    areEqual.Should().Equal(value);
                }
            }
        }
    }
}
