using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveAwayFundClientProject.Models.ViewModels
{
    public class Donor
    {
        public int DonorId { get; set; }
        public string DonorName { get; set; }
        public string DonorPhoneNumber { get; set; }
        public string DonorMailId { get; set; }
        public string DonorUserName { get; set; }
        public string DonorPassword { get; set; }
        public string DonorConfirmPassword { get; set; }
    }
}
