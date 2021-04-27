using GiveAwayFundClientProject.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GiveAwayFundClientProject.Controllers
{
    public class FundRaiserController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}
        public async Task<IActionResult> Index(string name)
        {
            List<NGO> ngoList = new List<NGO>();
            using (var httpClient = new HttpClient())
            {
                //
                using (var response = await httpClient.GetAsync("http://localhost:14780/api/NGO"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ngoList = JsonConvert.DeserializeObject<List<NGO>>(apiResponse);
                }
            }
            NGO ngo = ngoList.FirstOrDefault(d => d.NgoUserName == name);
            TempData["ngoname"] = ngo.NgoName;
            TempData["ngoid"] = ngo.NgoId;
            return View();
        }






        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(NGO ngo)
        {
            NGO nGO = new NGO();
            using (var httpClient = new HttpClient())
            {
                //int id = verify.VerificationId;
                StringContent content = new StringContent(JsonConvert.SerializeObject(ngo), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("http://localhost:14780/api/NGO/NgoRegister", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //ViewBag.Result = "Success";
                    nGO = JsonConvert.DeserializeObject<NGO>(apiResponse);
                    ViewBag.Result = "Successfully Registered, Please Login.....THANKYOU";
                }
            }
            return View();            
        }







        [HttpGet]
        public IActionResult ApplyforVerification()
        {
            Verification verify = new Verification();           
            verify.Status = "Pending";
            return View(verify);
        }

        [HttpPost]
        public async Task<IActionResult> ApplyforVerification(verificationwithoutid verify)
        {
            verificationwithoutid ver = new verificationwithoutid();
            using (var httpClient = new HttpClient())
            {
                
                StringContent content = new StringContent(JsonConvert.SerializeObject(verify), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("http://localhost:45908/api/Verification/NgoPostVerification", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //ViewBag.Result = "Success";
                    ver = JsonConvert.DeserializeObject<verificationwithoutid>(apiResponse);                    
                }                                
            }
            return RedirectToAction("SuccessVerification","FundRaiser");
            //return View();
        }






        [HttpGet]
        public IActionResult RaiseFunds()
        {            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RaiseFunds(EventRaiser eventRaiser)
        {
            EventRaiserwithoutid ver = new EventRaiserwithoutid();
            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(eventRaiser), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("http://localhost:7877/api/EventFund", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //ViewBag.Result = "Success";
                    ver = JsonConvert.DeserializeObject<EventRaiserwithoutid>(apiResponse);
                }
                return RedirectToAction("Index");
            }
        }







        public async Task<IActionResult> RaisedEvents(int id)
        {
            //List<NGO> ngoList = new List<NGO>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:14780/api/NGO/GetbyId/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //ViewBag.Result = "Success";
                    NGO ngoList = JsonConvert.DeserializeObject<NGO>(apiResponse);
                }
            }
            return View();
        }





        [HttpGet]
        public async Task<IActionResult> CheckforStatus()
        {
            List<Verification> verifyList = new List<Verification>();
            using (var httpClient = new HttpClient())
            {
                //
                using (var response = await httpClient.GetAsync("http://localhost:45908/api/Verification"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    verifyList = JsonConvert.DeserializeObject<List<Verification>>(apiResponse);
                }
            }
            int ngoid = (int)TempData.Peek("ngoid");
            Verification verify = verifyList.FirstOrDefault(v => v.NgoId == ngoid);

            if (verify.Status == "Accept")
            {
                return RedirectToAction("RaiseFunds", "FundRaiser");
            }
            if (verify.Status == "Reject")
            {
                return RedirectToAction("Rejected", "FundRaiser");
            }
            if (verify.Status == "pending")
            {
                return RedirectToAction("Pending", "FundRaiser");
            }
            return RedirectToAction("Index");
        }


        public IActionResult Rejected()
        {
            return View();
        }

        public IActionResult Pending()
        {
            return View();
        }


        public IActionResult SuccessVerification()
        {
            return View(); 
        }
        //*********akhil's code ***********//
        [HttpGet]
        public async Task<IActionResult> GetStat(int id)
        {

            //Verification verify = new Verification();
            int remainingFund;
            string fundPercentage;
            string fundStatus;

            // don.DonorId = (int)TempData.Peek("donarid");
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:7877/api/EventFund/RemainingFundAmount/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    // don = JsonConvert.DeserializeObject<int>(apiResponse);
                    remainingFund = Convert.ToInt32(apiResponse);
                }

            }
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:7877/api/EventFund/RemainingFundPercentage/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    // don = JsonConvert.DeserializeObject<int>(apiResponse);
                    fundPercentage = apiResponse;
                }

            }

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:7877/api/EventFund/FundStatus/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    // don = JsonConvert.DeserializeObject<int>(apiResponse);
                    fundStatus = apiResponse;
                }

            }




            // TempData["donorid"] = (int)TempData.Peek("donarid");
            TempData["fundStatus"] = fundStatus;
            TempData["percentProgress"] = fundPercentage;
            TempData["amountRemaining"] = remainingFund;
            return View();
        }


    }
}