using Cargo4You.Controllers;


namespace CargoXUnit.Test
{
    public class CalculationTest
    {

        private readonly CalculationController _systemUnderTest;
       


        public CalculationTest()
        {

            _systemUnderTest = new CalculationController();


        }

        //calculate volume test
        [Theory]
        [InlineData(1000, 10, 10, 10)]
        [InlineData(1200, 10, 10, 12)]
        public void CalculateVolumeTheory(double expected, double depth, double width, double height)
        {

            Assert.Equal(expected, _systemUnderTest.CalculateVolume(depth, width, height));

        }


        //get bigger-final price test
        [Theory]
        [InlineData(20, 10, 20)]
        [InlineData(20.5, 20.5, 20.4)]
        [InlineData(20, 20, 20)]
        [InlineData(20, -20, 20)]
        public void GetFinalPriceTheory(double expected, double price1, double price2)
        {

            Assert.Equal(expected, _systemUnderTest.GetFinalPrice(price1, price2));

        }


        //validate test
        [Theory]
        [InlineData(true, 20, "<=20")]
        [InlineData(true, 10, "<=20")]
        [InlineData(false, 30, "<=20")]
        [InlineData(true, 20, ">=20")]
        [InlineData(false, 10, ">=20")]
        [InlineData(true, 30, ">=20")]
        [InlineData(false, 20, "<20")]
        [InlineData(true, 10, "<20")]
        [InlineData(false, 30, "<20")]
        [InlineData(false, 20, ">20")]
        [InlineData(false, 10, ">20")]
        [InlineData(true, 30, ">20")]
        public void ValidateTheory(bool expected, double weightOrDimension, string validate)
        {

            Assert.Equal(expected, _systemUnderTest.Validate(weightOrDimension, validate));

        }

        [Theory]
        //dimension price
        [InlineData(10, "<=1000","10", 200.30)]
        [InlineData(0, ">1000&&<=2000", "20", 200.3)]
        [InlineData(20, ">1000&&<=2000", "20", 1200.5)]
        [InlineData(0, ">1000&&<=2000", "20", 2200.5)]
        [InlineData(11.99, "<=1000", "11,99", 1000)]
        [InlineData(0, ">5000", "147,5", 1000.5)]
        [InlineData(0, ">5000", "147,5", 5000)]
        [InlineData(147.5, ">5000", "147,5", 5000.1)]
        //weight price
        [InlineData(0, ">25", "40+0,417", 20)]
        [InlineData(40, ">25", "40+0,417", 25.3)]
        [InlineData(40.417, ">25", "40+0,417", 26)]
        [InlineData(41.251, ">25", "40+0,417", 28)]
        public void CalculateTheory(double expected, string calculateString, string calculationPrice, double weightOrDimension)
        {

            Assert.Equal(expected, _systemUnderTest.Calculate(calculateString, calculationPrice, weightOrDimension));

        }


    }
}