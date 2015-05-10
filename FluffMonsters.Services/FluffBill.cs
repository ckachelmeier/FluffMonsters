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
        private string _couponFormat = @"\$(\d+\.\d\d) coupon";
        private Regex _couponRegex;

        public FluffBill(string serviceMenu, string customerItems)
        {
            _couponRegex = new Regex(_couponFormat, RegexOptions.IgnoreCase);
            _petServiceMenu = GetMenuFromJson(serviceMenu);
            _customerBillItems = GetBillItemsFromJson(customerItems);
            _customerCoupons = GetCouponsFromJson(customerItems);
        }

        public Decimal getBillTotal()
        {
            decimal total = 0;
            foreach (var billItem in _customerBillItems)
            {
                if (_petServiceMenu.ContainsKey(billItem))
                {
                    total += _petServiceMenu[billItem];
                }
            }
            ApplyDiscount(_customerBillItems, ref total);

            //apply coupons after discount.
            total -= _customerCoupons.Sum();
            return total;
        }

        private void ApplyDiscount(List<string> customerBillItems, ref decimal total)
        {
            const string discountItemString = "day of boarding";
            //10% off discount if they have more than to days of boarding.
            if (customerBillItems.Count(bi => bi.ToLower().Equals(discountItemString)) > 2)
            {
                total *= 0.9M;
            }
        }

        private Dictionary<string, decimal> GetMenuFromJson(string serviceMenu)
        {
            var serviceMenuArray = JsonConvert.DeserializeObject<JArray>(serviceMenu);
            var menu = new Dictionary<string, decimal>();
            foreach (var menuItem in serviceMenuArray)
            {
                if (menuItem["name"] != null && menuItem["price"] != null)
                {
                    decimal price;
                    string name = menuItem["name"].ToString().Trim();
                    if (Decimal.TryParse(menuItem["price"].ToString().Trim().Trim('$'), out price))
                    {
                        menu.Add(name, price);
                    }
                }
            }
            return menu;
        }

        private List<string> GetBillItemsFromJson(string customerItems)
        {
            var customerItemsArray = JsonConvert.DeserializeObject<JArray>(customerItems);
            List<string> items = new List<string>();
            foreach (var customerItem in customerItemsArray)
            {
                if (customerItem.Type == JTokenType.String && !customerItem.ToString().ToLower().Contains("coupon"))
                {
                    items.Add(customerItem.ToString());
                }
            }
            return items;
        }

        private List<decimal> GetCouponsFromJson(string customerItems)
        {
            var customerItemsArray = JsonConvert.DeserializeObject<JArray>(customerItems);
            List<decimal> items = new List<decimal>();
            foreach (var customerItem in customerItemsArray)
            {
                if (customerItem.Type == JTokenType.String && _couponRegex.IsMatch(customerItem.ToString()))
                {
                    var matches = _couponRegex.Matches(customerItem.ToString());
                    if (matches.Count > 0 && matches[0].Groups.Count > 1)
                    {
                        items.Add(decimal.Parse(matches[0].Groups[1].Value));
                    }
                }
            }
            return items;
        }

    }
}
