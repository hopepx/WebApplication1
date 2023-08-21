using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using WebApplication1.DAL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ParcelTrackingController : Controller
    {
        private ParcelDAL parcelContext = new ParcelDAL();
        private StaffDAL staffContext = new StaffDAL();
        private FeedBackDAL feedBackDAL = new FeedBackDAL();
        private DeliveryHistoryDAL deliveryHistoryContext = new DeliveryHistoryDAL();
        public IActionResult CustomerDashboardNavbar()
        {
            return View();
        }

        public IActionResult CustomerTracking()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Member"))
            {
                return RedirectToAction("Index", "Home");
            }

            string CustomerName = HttpContext.Session.GetString("Name");
            List<Parcel> parcelList = parcelContext.GetAllParcelsBySenderName(CustomerName);
            List<Staff> deliveryMenList = staffContext.GetDeliveryMen();
            List<DeliveryHistory> deliveryHistoryList = deliveryHistoryContext.GetDeliveryHistoryList();
            ViewData["deliveryMenList"] = deliveryMenList;
            ViewData["deliveryHistoryList"] = deliveryHistoryList;
            return View(parcelList);
        }

        public IActionResult MakeFeedback()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Member"))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new FeedbackEnquiry());
        }

        [HttpPost]
        public IActionResult MakeFeedback(FeedbackEnquiry feedback)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Member"))
            {
                return RedirectToAction("Index", "Home");
            }

            int memberId = HttpContext.Session.GetInt32("MemberID") ?? 0;

            if (ModelState.IsValid)
            {
                feedback.StaffID = null;
                feedback.Response = null;

                // Get the logged-in member's email (assuming you have the email stored in the user claims)
                //string email = User.FindFirstValue("email");
                // Get the MemberID based on the email

                // Call the Add method in FeedBackDAL to save the feedback
                feedBackDAL.Add(feedback, memberId);

                ViewData["ShowResult"] = true;
                Console.WriteLine("SUCCESS");
                return RedirectToAction("ViewFeedbackList");
            }
            else
            {
                //show the error message
                ViewData["ShowResult"] = false;
                Console.WriteLine("ERROR");
                return RedirectToAction("ViewFeedbackList");
            }

        }

        public IActionResult RespondToFeedback(int id)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Station Manager"))
            {
                return RedirectToAction("Index", "Home");
            }

            FeedbackEnquiry feedback = feedBackDAL.GetFeedbackById(id);

            if (feedback == null)
            {
                // Feedback not found, handle the error (e.g., redirect to an error page)
                return NotFound();
            }

            return View(feedback);
        }

        [HttpPost]
        public IActionResult SubmitResponse(FeedbackEnquiry feedback)
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Station Manager"))
            {
                return RedirectToAction("Index", "Home");
            }

            int staffid = HttpContext.Session.GetInt32("StaffID") ?? 0;
            // Save the response to the database (update the feedback record)
            feedBackDAL.Update(feedback, staffid);
            // After saving, redirect the station manager to a confirmation page or back to the list of feedbacks
            return RedirectToAction("ViewFeedbackManagement");

        }

        public IActionResult StationManagerTracking()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Station Manager"))
            {
                return RedirectToAction("Index", "Home");
            }

            List<Parcel> parcelList = parcelContext.GetAllParcels();
            List<Staff> deliveryMenList = staffContext.GetDeliveryMen();
            List<DeliveryHistory> deliveryHistoryList = deliveryHistoryContext.GetDeliveryHistoryList();
            ViewData["deliveryMenList"] = deliveryMenList;
            ViewData["deliveryHistoryList"] = deliveryHistoryList;
            return View(parcelList);
        }

        public IActionResult ViewFeedbackList()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Member"))
            {
                return RedirectToAction("Index", "Home");
            }

            List<FeedbackEnquiry> feedbackList = feedBackDAL.GetAllFeedBack();
            int memberId = HttpContext.Session.GetInt32("MemberID") ?? 0;
            ViewBag.MemberId = memberId;
            return View(feedbackList);
        }

        public IActionResult ViewFeedbackManagement()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Station Manager"))
            {
                return RedirectToAction("Index", "Home");
            }

            List<FeedbackEnquiry> feedbackList = feedBackDAL.GetAllFeedBack();
            return View(feedbackList);
        }

        public IActionResult Contact()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Member"))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
