using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveAwayFundClientProject.Models.ViewModels
{
    public class Verification
    {
        public int VerificationId { get; set; }
        public int NgoId { get; set; }
        public string ProofOfVerification { get; set; }


        public string Status { get; set; }
    }
}
