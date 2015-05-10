using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluffMonsters.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FluffMonsters.Services
{
    public class FluffBill
    {
        private Dictionary<string, decimal> _petServiceMenu;
        private List<string> _customerBillItems;
        private List<decimal> _customerCoupons;
        private readonly Regex _couponRegex;

        private const string CouponFormat = @"\$(\d+\.\d\d) coupon";
        private const string DiscountItemString = "Day of Boarding";

        /// <summary>
        /// Constructs the bill from JSON objects representing the menu and bill items.  Throws ArgumentOutOfRange exception for invalid JSON
        /// </summary>
        /// <param name="serviceMenu">JSON string describing the menu.  Example valid JSON is [{"name": "Day of Food","price": "$10.00"}, {"name": "Day of Boarding","price": "$49.00"}]</param>
        /// <param name="customerItems">JSON string with a list of items on the customer's bill.  Valid JSON is an array of strings referring to the names in the serviceMenuJson parameter.
        /// strings of the format "$#.## coupon" are also valid and will apply a discount to the total after all discounts have been applied</param>
        public FluffBill(string serviceMenu, string customerItems)
        {
            _couponRegex = new Regex(CouponFormat, RegexOptions.IgnoreCase);
            try
            {
                PopulateServiceMenuFromJson(serviceMenu);
                PopulateBillItemsAndCouponsFromJson(customerItems);
            }
            catch (Exception ex)
            {
                if (ex is JsonReaderException || ex is InvalidCastException || ex is NullReferenceException || ex is ArgumentNullException)
                {
                    throw new ArgumentOutOfRangeException("Error parsing json", ex);
                }
                throw;
            }
        }

        /// <summary>
        /// Gets the total bill based on the menu and bill items given in the constructor.  Discounts and coupons will be applied in that order.
        /// </summary>
        /// <returns>Total bill</returns>
        public decimal GetBillTotal()
        {
            decimal total = 0;
            foreach (var billItem in _customerBillItems)
            {
                //we already checked to make sure the item is in the menu when we populated the bill items.
                total += _petServiceMenu[billItem];
            }
            ApplyDiscount(ref total);

            //apply coupons after discount.
            total -= _customerCoupons.Sum();
            //The bill is money, so we need to round to 2 decimal places.  
            return Math.Round(total, 2, MidpointRounding.ToEven);
        }

        /// <summary>
        /// Applies 10% off discount if they have more than to days of boarding.
        /// </summary>
        /// <param name="customerBillItems">List of bill items</param>
        /// <param name="total">Current total that may have a discount applied to it.</param>
        private void ApplyDiscount(ref decimal total)
        {
            if (_customerBillItems.Count(bi => bi.Equals(DiscountItemString)) > 2)
            {
                total *= 0.9M;
            }
        }

        private void PopulateServiceMenuFromJson(string serviceMenuJson)
        {
            var serviceMenuArray = JsonConvert.DeserializeObject<JArray>(serviceMenuJson);
            _petServiceMenu = new Dictionary<string, decimal>();
            foreach (var menuItem in serviceMenuArray)
            {
                if (menuItem["name"] != null && menuItem["price"] != null)
                {
                    decimal price;
                    string name = menuItem["name"].ToString().Trim();
                    if (Decimal.TryParse(menuItem["price"].ToString().Trim(" $".ToCharArray()), out price))
                    {
                        _petServiceMenu.Add(name, price);
                    }
                }
            }
        }

        private void PopulateBillItemsAndCouponsFromJson(string customerItems)
        {
            var customerItemsArray = JsonConvert.DeserializeObject<JArray>(customerItems);
            _customerBillItems = new List<string>();
            _customerCoupons = new List<decimal>();
            foreach (var customerItem in customerItemsArray.Where(customerItem => customerItem.Type == JTokenType.String))
            {
                if (_couponRegex.IsMatch(customerItem.ToString()))
                {
                    var matches = _couponRegex.Matches(customerItem.ToString());
                    if (matches.Count > 0 && matches[0].Groups.Count > 1)
                    {
                        _customerCoupons.Add(decimal.Parse(matches[0].Groups[1].Value));
                    }
                }
                else
                {
                    if (!_petServiceMenu.ContainsKey(customerItem.ToString()))
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    _customerBillItems.Add(customerItem.ToString());
                }
            }
        }

    }
}
