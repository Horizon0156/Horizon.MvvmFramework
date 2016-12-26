using Horizon.MvvmFramework.Wpf.Converter;
using System.Globalization;
using System.Windows;
using Xunit;

namespace Horizon.MvvmFramework.Wpf.Tests.Converter
{
    public class InvertedBooleanConverterTests
    {
        private InvertedBooleanConverter _testSubject;

        public InvertedBooleanConverterTests()
        {
            _testSubject = new InvertedBooleanConverter();
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void TestConvert(object inputValue, object expectedResult)
        {
            var result = _testSubject.Convert(inputValue, typeof(bool), null, CultureInfo.CurrentCulture);

            Assert.Equal(result, expectedResult);
        }

        [Theory]
        [InlineData(false, true)]
        [InlineData(true, false)]
        public void TestConvertBack(object inputValue, object expectedResult)
        {
            var result = _testSubject.ConvertBack(inputValue, typeof(bool), null, CultureInfo.CurrentCulture);

            Assert.Equal(result, expectedResult);
        }

        [Fact]
        public void TestConvertingNonBooleanValuesShouldReturnUnsetProperties()
        {
            var result = _testSubject.Convert(42, typeof(bool), null, CultureInfo.CurrentCulture);

            Assert.Equal(result, DependencyProperty.UnsetValue);
        }
    }
}