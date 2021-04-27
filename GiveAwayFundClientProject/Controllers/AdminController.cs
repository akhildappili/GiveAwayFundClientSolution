using GiveAwayFundClientProject.Models.ViewModels;
using Microsoft.AspNetCore.Http;
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
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       //Get List of All Ngo's
        public async Task<IActionResult> NGODetails()
        {
           
            List<NGO> ngo = new List<NGO>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:14780/api/NGO"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ngo = JsonConvert.DeserializeObject<List<NGO>>(apiResponse);
                }
            }
            return View(ngo);
        }

        //Get List of All Donor's
        public async Task<IActionResult> DonorsDetails()
        {
            
            List<Donor> donor = new List<Donor>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:51177/api/donors"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    donor = JsonConvert.DeserializeObject<List<Donor>>(apiResponse);
                }
            }
            return View(donor);
        }


        //List view of All NGO's that are in pending state and neeed to be verified
        public async Task<IActionResult> listverify()
        {
            List<Verification> verify = new List<Verification>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:45908/api/Verification/GetPendingStatusList"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    verify = JsonConvert.DeserializeObject<List<Verification>>(apiResponse);
                }
            }
            return View(verify);
        }

        //List view of All NGO's that are Accepted 
        public async Task<IActionResult> acceptverify()
        {
            List<Verification> verify = new List<Verification>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:45908/api/Verification/GetApprovedStatusList"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    verify = JsonConvert.DeserializeObject<List<Verification>>(apiResponse);
                }
            }
            return View(verify);
        }

        //List view of All NGO's that are Rejected 
        public async Task<IActionResult> rejectverify()
        {
            List<Verification> verify = new List<Verification>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:45908/api/Verification/GetRejectedStatusList"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    verify = JsonConvert.DeserializeObject<List<Verification>>(apiResponse);
                }
            }
            return View(verify);
        }


        //Get Method to verify the NGO's Account
        [HttpGet]
        public async Task<IActionResult> VerifyNgo(int id)
        {

            Verification verify = new Verification();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:45908/api/Verification/GetbyId/" + id))
                  {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    verify = JsonConvert.DeserializeObject<Verification>(apiResponse);
                }

            }
            return View(verify);
        }

        //Post Method to verify the NGO's Account
        [HttpPost]
        public async Task<IActionResult> VerifyNgo(int id,Verification verify)
        {
            Verification verification = new Verification();
            using (var httpClient = new HttpClient())
            {
               // int id = verify.VerificationId;
                StringContent content = new StringContent(JsonConvert.SerializeObject(verify), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync("http://localhost:45908/api/Verification/AdminEditStatus/" + id, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //ViewBag.Result = "Success";
                    verification = JsonConvert.DeserializeObject<Verification>(apiResponse);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteNgo(int id)
        {

            NGO ngo = new NGO();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:14780/api/NGO/GetbyId/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ngo = JsonConvert.DeserializeObject<NGO>(apiResponse);
                }

            }
            return View(ngo);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNgo(int id, NGO n)
        {
            NGO ngo = new NGO();
            using (var httpClient = new HttpClient())
            {
                // int id = verify.VerificationId;
                StringContent content = new StringContent(JsonConvert.SerializeObject(ngo), Encoding.UTF8, "application/json");

                using (var response = await httpClient.DeleteAsync("http://localhost:14780/api/NGO/DeletebyId/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //ViewBag.Result = "Success";
                    ngo = JsonConvert.DeserializeObject<NGO>(apiResponse);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
