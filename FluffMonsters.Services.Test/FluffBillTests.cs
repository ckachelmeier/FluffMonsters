using System;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using Newtonsoft.Json.Linq;

namespace FluffMonsters.Services.Test
{
    [TestFixture]
    public class FluffBillTests
    {
        #region jsonStrings
        string serviceMenu1 = @"
[
    {
        'name': 'Day of Food',
        'price': '$10.00'
    },
    {
        'name': 'Day of Boarding',
        'price': '$49.00'
    },
    {
        'name': 'Chew Toy',
        'price': '$5.00'
    }
]";

        string serviceMenu2 = @"
[
    {
        'name': 'Day of Food',
        'price': '$10.00'
    },
    {
        'name': 'Chew Toy',
        'price': '$5.00'
    }
]";
        string serviceMenuSigFigPrices = @"
[
    {
        'name': 'Day of Food',
        'price': '$10.99'
    },
    {
        'name': 'Day of Boarding',
        'price': '$49.99'
    },
    {
        'name': 'Chew Toy',
        'price': '$5.99'
    }
]";

        string customerItemsMultipleItems = @"
[
    'Day of Food',
    'Day of Boarding',
    'Chew Toy',
    'Day of Food',
    'Chew Toy',
    'Day of Boarding',
    'Day of Boarding',
    'Day of Boarding',
    'Day of Food',
    'Day of Food',
    'Day of Food'
]";

        private string customerItemsSingleDayOfFood = @"
[
            'Day of Food'
]";
        private string customerItemsSingleDayOfBoarding = @"
[
            'Day of Boarding'
]";
        private string customerItemsSingleChewToy = @"
[
            'Chew Toy'
]";
        private string customerItemsThreeDaysOfBoarding = @"
[
            'Day of Boarding',
            'Day of Boarding',
            'Day of Boarding'
]";
        private string customerItemsUnknownItem = @"
[
            'Unknown'
]";

        private string customerItemsSingleDayWithCoupon = @"
[
            'Day of Boarding',
            '$5.00 Coupon'
]";
        private string customerItemsTwoDaysWithCoupon = @"
[
            'Day of Boarding',
            'Day of Boarding',
            'Day of Boarding',
            '$5.00 Coupon'
]";
        #endregion

        [Test]
        public void GetBillTotal_Single_Day_Of_Food_Returns_Price()
        {
            var bill = new FluffBill(serviceMenu1, customerItemsSingleDayOfFood);
            bill.GetBillTotal().Should().Be(10);
        }

        [Test]
        public void GetBillTotal_Single_Day_Of_Boarding_Returns_Price()
        {
            var bill = new FluffBill(serviceMenu1, customerItemsSingleDayOfBoarding);
            bill.GetBillTotal().Should().Be(49);
        }

        [Test]
        public void GetBillTotal_Single_Chew_Toy_Returns_Price()
        {
            var bill = new FluffBill(serviceMenu1, customerItemsSingleChewToy);
            bill.GetBillTotal().Should().Be(5);
        }

        [Test]
        public void GetBillTotal_Three_Days_Of_Boarding_Gets_Discount()
        {
            var bill = new FluffBill(serviceMenu1, customerItemsThreeDaysOfBoarding);
            bill.GetBillTotal().Should().Be(132.3M, "49.00 * 3 * 0.9 = 132.3");
        }

        [Test]
        public void GetBillTotal_Unknown_Customer_Item_Throws_Exception()
        {
            Action action = () => new FluffBill(serviceMenu1, customerItemsUnknownItem);
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        public void GetBillTotal_Single_Day_With_Coupon()
        {
            var bill = new FluffBill(serviceMenu1, customerItemsSingleDayWithCoupon);
            bill.GetBillTotal().Should().Be(44);
        }

        [Test]
        public void GetBillTotal_Three_Days_Of_Boarding_Gets_Discound_And_Coupon()
        {
            var bill = new FluffBill(serviceMenu1, customerItemsTwoDaysWithCoupon);
            bill.GetBillTotal().Should().Be(127.3M, "(3 days*49)* 0.9 discount - 5 for coupon = 127.3");
        }

        [Test]
        public void GetBillTotal_Multiple_Customer_Items_Out_Of_Order()
        {
            var bill = new FluffBill(serviceMenu1, customerItemsMultipleItems);

            //0.9( 50 + 10 + 196) = 230.4
            bill.GetBillTotal().Should().Be(230.4M, "((5 days of food) * $10 + (2 chew toys) *$5 + (4 days of boarding) * $49) * 90% off total");
        }

        [Test]
        public void GetBillTotal_Rounds_To_Two_Decimals()
        {
            var bill = new FluffBill(serviceMenuSigFigPrices, customerItemsThreeDaysOfBoarding);
            bill.GetBillTotal().Should().Be(134.97M, "3*49.99 * 0.9 = 134.973 rounded to 134.97");
        }

        [Test]
        [TestCase("test")]
        [TestCase("{}")]
        [TestCase("")]
        public void GetBillTotal_Invalid_Menu_Json_Throws_Exception(string badJson)
        {
            Action action = () => new FluffBill(badJson, customerItemsMultipleItems);
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }

        [Test]
        [TestCase("test")]
        [TestCase("{}")]
        [TestCase("")]
        public void GetBillTotal_Invalid_Customer_Items_Json_Throws_Exception(string badJson)
        {
            Action action = () => new FluffBill(serviceMenu1, badJson);
            action.ShouldThrow<ArgumentOutOfRangeException>();
        }
    }
}
