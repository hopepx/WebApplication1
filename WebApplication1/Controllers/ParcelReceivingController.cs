using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing.Printing;
using WebApplication1.DAL;
using WebApplication1.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

public class ParcelReceivingController : Controller
{
    private ShippingRateDAL shippingRateDAL = new ShippingRateDAL();
    private PaymentTransactionDAL paymentTransactionContext = new PaymentTransactionDAL();
    private ParcelDAL parcelContext = new ParcelDAL();
    private DeliveryHistoryDAL deliveryHistoryContext = new DeliveryHistoryDAL();

    public IActionResult CreateParcel()
    {
        if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Front Office Staff"))
        {
            return RedirectToAction("Index", "Home");
        }

        List<string> cityAndCountryList = shippingRateDAL.GetToCityAndCountryList();
        ViewBag.CityAndCountryList = cityAndCountryList;

        if (TempData.ContainsKey("Parcel"))
        {
            Parcel previousParcel = JsonConvert.DeserializeObject<Parcel>(TempData["Parcel"].ToString());
            return View(previousParcel);
        }

        return View(new Parcel());
    }

    private async Task<decimal> ConvertCurrency(decimal deliveryCharge, string targetCurrency)
    {
        // Make a request to the currency conversion API
        string apiUrl = "https://api.currencyapi.com/v3/latest?apikey=cur_live_QKrfgWBKoqfNLOc1Mh1XaJqKC5av1dd4pcOXSZHS&currencies=EUR%2CUSD%2CJPY%2CMYR%2CCNY%2CGBP%2CAUD&base_currency=SGD";

        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(json);

                decimal exchangeRate;
                switch (targetCurrency)
                {
                    case "EUR":
                        exchangeRate = (decimal)data.data.EUR.value;
                        break;
                    case "USD":
                        exchangeRate = (decimal)data.data.USD.value;
                        break;
                    case "JPY":
                        exchangeRate = (decimal)data.data.JPY.value;
                        break;
                    case "MYR":
                        exchangeRate = (decimal)data.data.MYR.value;
                        break;
                    case "CNY":
                        exchangeRate = (decimal)data.data.CNY.value;
                        break;
                    case "GBP":
                        exchangeRate = (decimal)data.data.GBP.value;
                        break;
                    case "AUD":
                        exchangeRate = (decimal)data.data.AUD.value;
                        break;
                    default:
                        exchangeRate = 1.0m; // Default to 1 if targetCurrency is not found
                        break;
                }

                // Convert the delivery charge to the target currency
                return deliveryCharge * exchangeRate;
            }
            else
            {
                Console.WriteLine("ERROR");
                // If there's an error with the API, log the error or handle it accordingly
                return deliveryCharge;
            }
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateParcelAsync(Parcel parcel)
    {
        if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Front Office Staff"))
        {
            return RedirectToAction("Index", "Home");
        }

        if (ModelState.IsValid)
        {
            // Set TargetDeliveryDate and DeliveryManID to null
            parcel.TargetDeliveryDate = null;
            parcel.DeliveryManID = null;

            // Retrieve the shipping rate and transit time
            ShippingRateModel shippingRate = shippingRateDAL.GetShippingRate(parcel.ToCity, parcel.ToCountry);
            if (shippingRate != null)
            {
                // Store the calculated values in the parcel object
                shippingRate.ShippingRate = await ConvertCurrency(shippingRate.ShippingRate, parcel.Currency);
                parcel.DeliveryCharge = ComputeDeliveryCharge(shippingRate, parcel.ParcelWeight);
                parcel.TargetDeliveryDate = ComputeTargetDeliveryDate(shippingRate.TransitTime);

                // Redirect to the ComputeTargetDeliveryDate action with the parcel info
                string info = GetParcelInfo(parcel, shippingRate);

                string parcelJson = JsonConvert.SerializeObject(parcel);

                TempData["Parcel"] = parcelJson;

                HttpContext.Session.SetInt32("parcelDeliveryCharge", Convert.ToInt32(parcel.DeliveryCharge));

                HttpContext.Session.SetString("parcelDeliveryDate", parcel.TargetDeliveryDate?.ToString("dd/MM/yy"));

                TempData["Parcelinfo"] = info;

                return RedirectToAction("ComputeTargetDeliveryDate", new { info });
            }
        }

        // If the model state is invalid or shipping rate is not found, return to the view with the parcel object to display validation errors
        List<string> cityAndCountryList = shippingRateDAL.GetToCityAndCountryList();
        ViewBag.CityAndCountryList = cityAndCountryList;
        return View(parcel);
    }

    private decimal ComputeDeliveryCharge(ShippingRateModel shippingRate, double parcelWeight)
    {
        decimal rawDeliveryCharge = Decimal.Multiply(shippingRate.ShippingRate, Convert.ToDecimal(parcelWeight));
        decimal roundedDeliveryCharge = Math.Ceiling(rawDeliveryCharge);
        decimal finalDeliveryCharge = roundedDeliveryCharge >= 5 ? roundedDeliveryCharge : 5;
        return finalDeliveryCharge;
    }

    private DateTime? ComputeTargetDeliveryDate(int transitTime)
    {
        return DateTime.Now.AddDays(transitTime);
    }

    private string GetParcelInfo(Parcel parcel, ShippingRateModel shippingRate)
    {
        decimal rawDeliveryCharge = Decimal.Multiply(shippingRate.ShippingRate, Convert.ToDecimal(parcel.ParcelWeight));
        decimal roundedDeliveryCharge = Math.Ceiling(rawDeliveryCharge);

        HttpContext.Session.SetInt32("roundedDeliveryCharge", Convert.ToInt32(roundedDeliveryCharge));

        string info = $"Parcel Weight: {parcel.ParcelWeight} kg\n";
        info += $"From City and Country: {parcel.FromCity}, {parcel.FromCountry}\n";
        info += $"To City and Country: {parcel.ToCity}, {parcel.ToCountry}\n";
        info += $"Shipping Rate: {shippingRate.ShippingRate.ToString("F2")}/kg\n";
        info += $"Delivery Charge (Raw): ({shippingRate.ShippingRate.ToString("F2")} x {parcel.ParcelWeight}) = {rawDeliveryCharge.ToString("F2")}\n";
        info += $"Delivery Charge (Rounded): {roundedDeliveryCharge.ToString("F2")}\n";
        info += $"Delivery Charge (Final): {parcel.DeliveryCharge.ToString("F2")} (Note: Minimum delivery charge is 5.00)\n";
        info += $"Estimated Delivery Date: {parcel.TargetDeliveryDate?.ToString("dd/MM/yy")}";
        return info;
    }

    public IActionResult ComputeTargetDeliveryDate(string info)
    {
        if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Front Office Staff"))
        {
            return RedirectToAction("Index", "Home");
        }

        ViewData["ParcelDeliveryCharge"] = Convert.ToString(HttpContext.Session.GetInt32("parcelDeliveryCharge"));
        ViewData["ParcelInfo"] = TempData["Parcelinfo"];
        return View();
    }

    public IActionResult CreatePaymentTransaction()
    {
        if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Front Office Staff"))
        {
            return RedirectToAction("Index", "Home");
        }

        ViewData["ParcelDeliveryCharge"] = Convert.ToString(HttpContext.Session.GetInt32("parcelDeliveryCharge"));
        ViewData["parcelDeliveryDate"] = HttpContext.Session.GetString("parcelDeliveryDate");
        ViewData["roundedDeliveryCharge"] = Convert.ToString(HttpContext.Session.GetInt32("roundedDeliveryCharge"));
        return View();
    }

    [HttpPost]
    public IActionResult CreatePaymentTransaction(decimal cashAmount, decimal voucherAmount)
    {
        if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Front Office Staff"))
        {
            return RedirectToAction("Index", "Home");
        }

        string parcelJson = TempData["Parcel"] as string;

        Parcel parcel = JsonConvert.DeserializeObject<Parcel>(parcelJson);

        string loginID = HttpContext.Session.GetString("LoginID");

        string description = $"Received parcel by {loginID} on {DateTime.Now.ToString("dd MMM yyyy h:mm tt")}.";

        if (cashAmount + voucherAmount == parcel.DeliveryCharge)
        {
            int parcelId = parcelContext.AddParcel(parcel);

            Console.WriteLine(parcelId);

            DeliveryHistory deliveryHistory = new DeliveryHistory
            {
                ParcelID = parcelId, 
                Description = description
            };

            deliveryHistoryContext.AddDeliveryHistory(deliveryHistory);

            if (cashAmount > 0)
            {
                PaymentTransaction cashTransaction = new PaymentTransaction
                {
                    ParcelID = parcelId, // Update with the appropriate ParcelID value
                    AmtTran = cashAmount,
                    Currency = parcel.Currency,
                    TranType = "CASH",
                };

                paymentTransactionContext.AddPaymentTransaction(cashTransaction);
            }

            if (voucherAmount > 0)
            {
                PaymentTransaction voucherTransaction = new PaymentTransaction
                {
                    ParcelID = parcelId, // Update with the appropriate ParcelID value
                    AmtTran = voucherAmount,
                    Currency = parcel.Currency,
                    TranType = "VOUC",
                };

                paymentTransactionContext.AddPaymentTransaction(voucherTransaction);
            }

            TempData.Clear();
            return RedirectToAction("CreateParcel");
        }


        else
        {
            TempData["ErrorMessage"] = "Invalid payment amount. Please make sure the payment amount matches the delivery charge.";
            return RedirectToAction("CreatePaymentTransaction");
        }
    }

    public IActionResult ViewPaymentTransactions()
    {
        if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Front Office Staff"))
        {
            return RedirectToAction("Index", "Home");
        }

        List<PaymentTransaction> staffList = paymentTransactionContext.GetAllTransactions();
        return View(staffList);
    }
}

