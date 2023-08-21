using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using System.Reflection.Metadata.Ecma335;
using WebApplication1.DAL;
using WebApplication1.Models;
using Microsoft.AspNetCore.Http;


namespace WebApplication1.Controllers
{

    public class ParcelDeliveryController : Controller
    {
        private StaffDAL staffContext = new StaffDAL();
        private ParcelDAL parcelContext = new ParcelDAL();
        private DeliveryHistoryDAL deliveryHistoryContext = new DeliveryHistoryDAL();

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateDeliveryFailureReport()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Delivery Man"))
            {
                return RedirectToAction("Index", "Home");
            }
            DeliveryFailure deliveryFailure = new DeliveryFailure();
            return View(DeliveryFailureReportConfirmation);
        }
        public IActionResult DeliveryFailureReportConfirmation()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Delivery Man"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public IActionResult ReceivingDeliveryRecords()
        {
            return View();
        }
        public IActionResult SelectDeliveryMan(Parcel parcel)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Station Manager"))
            {
                return RedirectToAction("Index", "Home");
            }
            // Retrieve delivery man data from the database
            List<Staff> deliveryManList = staffContext.GetDeliveryManList();
            List<SelectListItem> selectListItemList = new List<SelectListItem>();
            foreach (Staff record in deliveryManList)
            {
                selectListItemList.Add(new SelectListItem { Text = Convert.ToString(record.StaffID), Value = Convert.ToString(record.StaffID) });
            }
            ViewData["deliveryManList"] = selectListItemList;
            return View(parcel);
        }
        public IActionResult ViewUnassignedParcelList()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Station Manager"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public ActionResult Assign(IFormCollection formData)
        {
            int parcelId = Convert.ToInt32(formData["parcelId"]);

            Parcel? parcel = parcelContext.GetParcelById(parcelId);

            string parcelJson = JsonConvert.SerializeObject(parcel);

            TempData["Parcel"] = parcelJson;

            if (parcel != null)
            {
                return RedirectToAction("SelectDeliveryMan", parcel);
            }

            else
            {
                TempData["Message"] = "Invalid Login Credentials!";
                return RedirectToAction("_SMMenu");
            }
        }
        
        [HttpPost]
        public ActionResult UpdateDevStatus(IFormCollection formData)
        {
            int parcelId = Convert.ToInt32(formData["parcelId"]);

            Parcel? parcel = parcelContext.GetParcelById(parcelId);

            string parcelJson = TempData["Parcel"] as string;

            string loginID = HttpContext.Session.GetString("LoginID");

            string description = $"Received parcel by {loginID} on {DateTime.Now.ToString("dd MMM yyyy h:mm tt")}.";

            DeliveryHistory deliveryHistory = new DeliveryHistory
            {
                
                ParcelID = parcelId,
                Description = description
            };
            
            deliveryHistoryContext.UpdateDevStatus(deliveryHistory, loginID);
            deliveryHistoryContext.AddDeliveryHistorySM(deliveryHistory);
            return View("ViewUnassignedParcelList");
        }

        [HttpPost]
        public ActionResult AddDeliveryHistoryDM()
        {
            string parcelJson = TempData["Parcel"] as string;

            string loginID = HttpContext.Session.GetString("LoginID");

            string description = $"Received parcel by {loginID} on {DateTime.Now.ToString("dd MMM yyyy h:mm tt")}.";

            Parcel parcel = JsonConvert.DeserializeObject<Parcel>(parcelJson);
            
            DeliveryHistory deliveryHistory = new DeliveryHistory
            {
                ParcelID = parcel.ParcelID,
                Description = description
            };

            deliveryHistoryContext.UpdateDevStatus(deliveryHistory, loginID);
            deliveryHistoryContext.AddDeliveryHistorySM(deliveryHistory);
            return View("ViewUnassignedParcelList");
        }

        
        public IActionResult ViewDMParcelList()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Delivery Man"))
            {
                return RedirectToAction("Index", "Home");
            }
            List<Parcel> parcels = parcelContext.GetAllParcels();
            return View(parcels);

        }

        [HttpPost]
        public IActionResult ListDeliveryMan(Staff staff)
        {
            List<Staff> deliveryManList = staffContext.GetDeliveryManList();
            ViewBag.DeliveryManList = deliveryManList;
            // Pass the delivery man data to the view
            return View();
        }

        public IActionResult DeliveryManParcels(int deliveryManID)
        {
            ParcelDAL parcelDAL = new ParcelDAL();
            List<Parcel> parcels = parcelDAL.GetAllParcelsByDMID(deliveryManID);

            return View(parcels);
        }
        public IActionResult CreateDevFailRep(CreateFailReport FailureReportID)
        {

            return View();
        }
        

    }
    }
