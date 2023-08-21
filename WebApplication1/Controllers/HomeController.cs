using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private StaffDAL staffContext = new StaffDAL();

        private MemberDAL memberContext = new MemberDAL();

        private ShippingRateDAL shippingRateDAL = new ShippingRateDAL();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            TempData.Clear();
            return View();
        }

        public ActionResult LogOut()
        {
            // Clear all key-values pairs stored in session state
            HttpContext.Session.Clear();
            // Call the Index action of Home controller
            TempData.Clear();
            return RedirectToAction("Index");
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Receiving()
        {
            return View();
        }

        public IActionResult Delivery()
        {
            return View();
        }

        public IActionResult Tracking()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult MemberLogin()
        {
            return View();
        }

        public IActionResult MemberSignUp()
        {
            List<string> cityAndCountryList = shippingRateDAL.GetToCityAndCountryList();
            ViewBag.CityAndCountryList = cityAndCountryList;
            return View(new Member());
        }

        [HttpPost]
        public IActionResult MemberSignUp(Member member)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("Successful!");
                memberContext.Add(member);
                return RedirectToAction("MemberLogin");
            }
            Console.WriteLine("Error!");
            return View(member);
        }

        public IActionResult StaffLogin()
        {
            return View();
        }

            [HttpPost]
            public ActionResult LoginStaff(IFormCollection formData)
            {
                // Read inputs from textboxes 
                // Email address converted to lowercase
                string loginID = formData["txtLoginID"].ToString();
                string password = formData["txtPassword"].ToString();

            Staff? staff = staffContext.Login(loginID, password);
            if (staff != null)
            {
                HttpContext.Session.SetString("LoginID", loginID);
                HttpContext.Session.SetString("Role", staff.Appointment);
                HttpContext.Session.SetString("Name", staff.StaffName);
                HttpContext.Session.SetInt32("StaffID", staff.StaffID);

                    return RedirectToAction("Dashboard");
                }

                else
                {
                    TempData["Message"] = "Invalid Login Credentials!";
                    return RedirectToAction("StaffLogin");
                }
            }

        [HttpPost]
        public ActionResult LoginMember(IFormCollection formData)
        {
            // Read inputs from textboxes 
            // Email address converted to lowercase
            string loginID = formData["txtLoginID"].ToString();
            string password = formData["txtPassword"].ToString();

            Member? member = memberContext.Login(loginID, password);
            if (member != null)
            {
                HttpContext.Session.SetString("LoginID", loginID);
                HttpContext.Session.SetString("Role", "Member");
                HttpContext.Session.SetString("Name", member.Name);
                HttpContext.Session.SetInt32("MemberID", member.MemberID);

                TempData.Clear();
                return RedirectToAction("Dashboard");
            }

            else
            {
                TempData["Message"] = "Invalid Login Credentials!";
                return RedirectToAction("MemberLogin");
            }
        }

        public IActionResult Dashboard()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Front Office Staff" &&
                HttpContext.Session.GetString("Role") != "Member" &&
                HttpContext.Session.GetString("Role") != "Station Manager" &&
                HttpContext.Session.GetString("Role") != "Delivery Man"))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}