using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveAwayFundClientProject.Models.ViewModels
{
    public class NGO
    {
        public int NgoId { get; set; }
        public string NgoName { get; set; }
        public string NgoPhoneNumber { get; set; }
        public string NgoMailId { get; set; }
        public string NgoAddress { get; set; }
        public string NgoUserName { get; set; }
        public string NgoPassword { get; set; }
        public string NgoConfirmPassword { get; set; }
    }
}
