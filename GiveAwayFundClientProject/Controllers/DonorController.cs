using GiveAwayFundClientProject.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GiveAwayFundClientProject.Controllers
{
    public class DonorController : Controller
    {
        public dynamic DropDownList { get; private set; }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Donor donor)
        {
            Donor don = new Donor();
            using (var httpClient = new HttpClient())
            {
                //int id = verify.VerificationId;
                StringContent content = new StringContent(JsonConvert.SerializeObject(donor), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("http://localhost:51177/api/Donors", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //ViewBag.Result = "Success";
                    don = JsonConvert.DeserializeObject<Donor>(apiResponse);
                }
            }
            return RedirectToAction("Login","Login");
        }
        //[HttpPost]
        //public async Task<IActionResult> Register(Donor donor)
        //{

        //    Donor don = new Donor();
        //    using (var httpClient = new HttpClient())
        //    {
        //        //int id = verify.VerificationId;
        //        StringContent content = new StringContent(JsonConvert.SerializeObject(donor), Encoding.UTF8, "application/json");

        //        using (var response = await httpClient.PostAsync("http://localhost:51177/api/Donors", content))
        //        {
        //            string apiResponse = await response.Content.ReadAsStringAsync();
        //            //ViewBag.Result = "Success";
        //            don = JsonConvert.DeserializeObject<Donor>(apiResponse);
        //        }
        //    }
        //    return RedirectToAction("Login","Login");
        //}
        public async Task<IActionResult> Index(string name)
        {
            List<Donor> donorList = new List<Donor>();
            Donation donation = new Donation();
            string donarname;
            int totalamount;
            using (var httpClient = new HttpClient())
            {
                //
                using (var response = await httpClient.GetAsync("http://localhost:51177/api/Donors"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    donorList = JsonConvert.DeserializeObject<List<Donor>>(apiResponse);

                    
                }
            }
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:58271/api/Donation/getDonorNameWithMaxDonations"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    // don = JsonConvert.DeserializeObject<int>(apiResponse);
                    donarname = apiResponse;
                }

            }

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:58271/api/Donation/totalAmount"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    // don = JsonConvert.DeserializeObject<int>(apiResponse);
                    totalamount = Convert.ToInt32(apiResponse);
                }

            }


            Donor donor = donorList.FirstOrDefault(d => d.DonorUserName == name);
            TempData["donarid"] = donor.DonorId;
            TempData["donorname"] = donor.DonorUserName;
            TempData["donarnamemax"] = donarname;
            TempData["totalamount"] = totalamount;
            return View();
        }


        public async Task<IActionResult> Donate()
        {
            //ViewBag.specialization = DropDownList;
            //NGO ngo = new NGO();
            Donation don = new Donation();
            don.DonorId = (int)TempData.Peek("donarid");
            Verification verify = new Verification();
            List<SelectListItem> DropDownList = new List<SelectListItem>();

            //List<NGO> ngoList = new List<NGO>();
            List<Verification> verifyList = new List<Verification>();
            using (var httpClient = new HttpClient())
            {
                //
                using (var response = await httpClient.GetAsync("http://localhost:45908/api/Verification/GetApprovedStatusList"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    verifyList = JsonConvert.DeserializeObject<List<Verification>>(apiResponse);
                }
            }

            // change ngolist to verifyList
            foreach (var item in verifyList)
            {
                var id = Convert.ToString(item.NgoId);
                DropDownList.Add(new SelectListItem() { Text = id, Value = Convert.ToString(item.NgoId) });
            }
            ViewBag.specialization = DropDownList;
            return View(don);
           // return View("SuccessFullDonation","Donor");
        }

        //public async Task<IActionResult> Donate()
        //{
        //    //ViewBag.specialization = DropDownList;
        //    NGO ngo = new NGO();
        //    //Verification verify = new Verification();
        //    List<SelectListItem> DropDownList = new List<SelectListItem>();

        //    List<NGO> ngoList = new List<NGO>();
        //    //List<Verification> verifyList = new List<Verification>();
        //    using (var httpClient = new HttpClient())
        //    {
        //         //http://localhost:45908/api/Verification/GetApprovedStatusList
        //        using (var response = await httpClient.GetAsync("http://localhost:14780/api/NGO"))
        //        {
        //            string apiResponse = await response.Content.ReadAsStringAsync();
        //            ngoList = JsonConvert.DeserializeObject<List<NGO>>(apiResponse);
        //        }
        //    }

        //    // change ngolist to verifyList
        //    foreach (var item in ngoList)
        //    {
        //        var id = Convert.ToString(item.NgoId);
        //        DropDownList.Add(new SelectListItem() { Text = id, Value = Convert.ToString(item.NgoId) });
        //    }
        //    ViewBag.specialization = DropDownList;
        //    return View();
        //}
        [HttpPost]
        public async Task<IActionResult> Donate(Donation donation)
        {
            Donation don = new Donation();
            using (var httpClient = new HttpClient())
            {
                //int id = verify.VerificationId;
                StringContent content = new StringContent(JsonConvert.SerializeObject(donation), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("http://localhost:58271/api/Donation/addDonation", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //ViewBag.Result = "Success";
                    don = JsonConvert.DeserializeObject<Donation>(apiResponse);
                }
            }
            return View("SuccessFullDonation", "Donor");

            //successfull donation
           // return  RedirectToAction("SuccessFullDonation","Donor");
        }
        //public IActionResult DonationLists()
        //{
        //    return View();
        //}
       // 
        public async Task<IActionResult> DonationLists(int id)
        {
            List<Donation> donationList = new List<Donation>();
            //TempData["donorid"] = donation.DonorId;
            using (var httpClient = new HttpClient())
            {
                //
                using (var response = await httpClient.GetAsync("http://localhost:58271/api/Donation/GetDonation/"+id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    donationList = JsonConvert.DeserializeObject<List<Donation>>(apiResponse);
                }
            }
            return View(donationList);
        }

        [HttpGet]
        public async Task<IActionResult> GetDetails(int id)
        {

            //Verification verify = new Verification();
            Donor don = new Donor();
            don.DonorId = (int)TempData.Peek("donarid");
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:51177/api/Donors/GetDonor/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    don = JsonConvert.DeserializeObject<Donor>(apiResponse);
                }

            }
            //var model = new Verification()
            //{
            //    Status = "Pending"
            //};
            //return View(model);
            return View(don);
        }

        //Post Method to verify the NGO's Account
        [HttpPost]
        public async Task<IActionResult> GetDetails(int id, Donor don)
        {
            // Verification verification = new Verification();
            Donor donor = new Donor();
            using (var httpClient = new HttpClient())
            {
                // int id = verify.VerificationId;
                StringContent content = new StringContent(JsonConvert.SerializeObject(don), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("http://localhost:51177/api/Donors/EditDonor/" + id, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //ViewBag.Result = "Success";
                    donor = JsonConvert.DeserializeObject<Donor>(apiResponse);
                }
            }

            return RedirectToAction("SuccessfullyEdited","Donor");
        }

        [HttpGet]
        public async Task<IActionResult> GetTotalAmount(int id)
        {

            //Verification verify = new Verification();
            int don1;

           // don.DonorId = (int)TempData.Peek("donarid");
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:58271/api/Donation/getTotalAmountDonor/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                   // don = JsonConvert.DeserializeObject<int>(apiResponse);
                    don1 = Convert.ToInt32(apiResponse);
                }

            }
            TempData["donorid"] = (int)TempData.Peek("donarid");
            TempData["amount"] = don1;
            return View();
        }


        public async Task<IActionResult> DonateChildFood()
        {
            //ViewBag.specialization = DropDownList;
            //NGO ngo = new NGO();
            Donation don = new Donation();
            don.DonorId = (int)TempData.Peek("donarid");
            Verification verify = new Verification();
            List<SelectListItem> DropDownList = new List<SelectListItem>();

            //List<NGO> ngoList = new List<NGO>();
            List<Verification> verifyList = new List<Verification>();
            using (var httpClient = new HttpClient())
            {
                //
                using (var response = await httpClient.GetAsync("http://localhost:45908/api/Verification/GetApprovedStatusList"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    verifyList = JsonConvert.DeserializeObject<List<Verification>>(apiResponse);
                }
            }

            // change ngolist to verifyList
            foreach (var item in verifyList)
            {
                var id = Convert.ToString(item.NgoId);
                DropDownList.Add(new SelectListItem() { Text = id, Value = Convert.ToString(item.NgoId) });
            }
            ViewBag.specialization = DropDownList;
            return View(don);
            // return View("SuccessFullDonation","Donor");
        }
        [HttpPost]
        public async Task<IActionResult> DonateChildFood(Donation donation)
        {
            Donation don = new Donation();
            using (var httpClient = new HttpClient())
            {
                //int id = verify.VerificationId;
                StringContent content = new StringContent(JsonConvert.SerializeObject(donation), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("http://localhost:58271/api/Donation/addDonation", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //ViewBag.Result = "Success";
                    don = JsonConvert.DeserializeObject<Donation>(apiResponse);
                }
            }
            return View("SuccessFullDonation", "Donor");

            //successfull donation
            // return  RedirectToAction("SuccessFullDonation","Donor");
        }


        public async Task<IActionResult> DonateWomenEdu()
        {
            //ViewBag.specialization = DropDownList;
            //NGO ngo = new NGO();
            Donation don = new Donation();
            don.DonorId = (int)TempData.Peek("donarid");
            Verification verify = new Verification();
            List<SelectListItem> DropDownList = new List<SelectListItem>();

            //List<NGO> ngoList = new List<NGO>();
            List<Verification> verifyList = new List<Verification>();
            using (var httpClient = new HttpClient())
            {
                //
                using (var response = await httpClient.GetAsync("http://localhost:45908/api/Verification/GetApprovedStatusList"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    verifyList = JsonConvert.DeserializeObject<List<Verification>>(apiResponse);
                }
            }

            // change ngolist to verifyList
            foreach (var item in verifyList)
            {
                var id = Convert.ToString(item.NgoId);
                DropDownList.Add(new SelectListItem() { Text = id, Value = Convert.ToString(item.NgoId) });
            }
            ViewBag.specialization = DropDownList;
            return View(don);
            // return View("SuccessFullDonation","Donor");
        }

        [HttpPost]
        public async Task<IActionResult> DonateWomenEdu(Donation donation)
        {
            Donation don = new Donation();
            using (var httpClient = new HttpClient())
            {
                //int id = verify.VerificationId;
                StringContent content = new StringContent(JsonConvert.SerializeObject(donation), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("http://localhost:58271/api/Donation/addDonation", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //ViewBag.Result = "Success";
                    don = JsonConvert.DeserializeObject<Donation>(apiResponse);
                }
            }
            return View("SuccessFullDonation", "Donor");

            //successfull donation
            // return  RedirectToAction("SuccessFullDonation","Donor");
        }

        





    }
}
