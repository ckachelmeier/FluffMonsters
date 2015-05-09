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
        string serviceMenuJson1 = @"
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

        string customerItemsJson1 = @"
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

        private string customerItemsJson2 = @"
[
            'Day of Food'
]";
        private string customerItemsJson3 = @"
[
            'Day of Boarding'
]";
        private string customerItemsJson4 = @"
[
            'Chew Toy'
]";
        private string customerItemsJson5 = @"
[
            'Day of Boarding',
            'Day of Boarding'
]";
        private string customerItemsJson6 = @"
[
            'Unknown'
]";
        #endregion

        #region test variables

        private JArray serviceMenu1;
        private JArray customerItemsMultipleItems;
        private JArray customerItemsSingleDayOfFood;
        private JArray customerItemsSingleDayOfBoarding;
        private JArray customerItemsSingleChewToy;
        private JArray customerItemsTwoDaysOfBoarding;
        private JArray customerItemsUnknownItem;
        #endregion

        [TestFixtureSetUp]
        public void Setup()
        {
            serviceMenu1 = JsonConvert.DeserializeObject<JArray>(serviceMenuJson1);
            customerItemsMultipleItems = JsonConvert.DeserializeObject<JArray>(customerItemsJson1);
            customerItemsSingleDayOfFood = JsonConvert.DeserializeObject<JArray>(customerItemsJson2);
            customerItemsSingleDayOfBoarding = JsonConvert.DeserializeObject<JArray>(customerItemsJson3);
            customerItemsSingleChewToy = JsonConvert.DeserializeObject<JArray>(customerItemsJson4);
            customerItemsTwoDaysOfBoarding = JsonConvert.DeserializeObject<JArray>(customerItemsJson5);
            customerItemsUnknownItem = JsonConvert.DeserializeObject<JArray>(customerItemsJson6);
        }

        [Test]
        public void GetBillTotal_Single_Day_Of_Food_Returns_Price()
        {
            var bill = new FluffBill(serviceMenu1, customerItemsSingleDayOfFood);
            bill.getBillTotal().Should().Be(10);
        }

        [Test]
        public void GetBillTotal_Single_Day_Of_Boarding_Returns_Price()
        {
            var bill = new FluffBill(serviceMenu1, customerItemsSingleDayOfBoarding);
            bill.getBillTotal().Should().Be(49);
        }

        [Test]
        public void GetBillTotal_Single_Chew_Toy_Returns_Price()
        {
            var bill = new FluffBill(serviceMenu1, customerItemsSingleChewToy);
            bill.getBillTotal().Should().Be(5);
        }

        [Test]
        public void GetBillTotal_Two_Days_Of_Boarding_Gets_Discount()
        {
            var bill = new FluffBill(serviceMenu1, customerItemsTwoDaysOfBoarding);
            bill.getBillTotal().Should().Be(88.2M);
        }

        [Test]
        public void GetBillTotal_Unknown_Customer_Item_Returns_Bill_Of_0()
        {
            var bill = new FluffBill(serviceMenu1, customerItemsUnknownItem);
            bill.getBillTotal().Should().Be(0);
        }

        [Test]
        public void GetBillTotal_Multiple_Customer_Items_Out_Of_Order()
        {
            var bill = new FluffBill(serviceMenu1, customerItemsMultipleItems);

            //((5 days of food) * $10 + (2 chew toys) *$5 + (4 days of boarding) * $49) * 90% off total
            //0.9( 50 + 10 + 196) = 230.4
            bill.getBillTotal().Should().Be(230.4M);
        }
    }
}
