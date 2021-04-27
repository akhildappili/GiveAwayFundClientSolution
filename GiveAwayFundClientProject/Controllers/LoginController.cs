using GiveAwayFundClientProject.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace GiveAwayFundClientProject.Controllers
{
    public class LoginController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }      

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {

            HttpContext.Session.SetString("UserId", login.UserName);

           

            string token = "";
            using (var httpclient = new HttpClient())
            {
                httpclient.BaseAddress = new Uri("http://localhost:9885/");
                var postData = httpclient.PostAsJsonAsync<Login>("api/Login/AuthenicateUser", login);
                var res = postData.Result;
                if (res.IsSuccessStatusCode)
                {
                    token = await res.Content.ReadAsStringAsync();
                    TempData["token"] = token;
                    if (token != null)
                    {
                        return RedirectToAction("CheckRole", "Login", new { username = login.UserName, password = login.Password });
                    }
                }
            }
            return View();

        }

        public async Task<IActionResult> CheckRole(string username, string password)
        {
            ///api/Users/{uname},{pass}
            Loginas login = new Loginas();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("http://localhost:9885/");
                using (var response = await httpClient.GetAsync("/api/Login/" + username + "," + password))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    login = JsonConvert.DeserializeObject<Loginas>(apiResponse);
                }
                if (login.loginas == null)
                {    
                    return RedirectToAction("Login", "Home");
                }
                if (login.loginas.ToLower() == "admin")
                {
                    return RedirectToAction("Index", "Admin", new { name = username });
                }
                if (login.loginas.ToLower() == "donor")
                {
                    //Donor donor = new Donor();
                    //TempData["DUserId"] = donor.DonorId;

                    return RedirectToAction("Index", "Donor", new { name = username });
                }
                if (login.loginas.ToLower() == "fundraiser")
                {
                    //ViewBag.data = HttpContext.Session.GetString("UserId");
                    NGO ngo = new NGO();
                    TempData["NUserName"] = ngo.NgoUserName;
                    return RedirectToAction("Index", "FundRaiser", new { name = username });
                }
            }
            return RedirectToAction("Login", "Home");
        }
    }
}

