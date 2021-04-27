using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GiveAwayFundClientProject.Models.ViewModels
{
    public class verificationwithoutid
    {
        public int? NgoId { get; set; }
        public string ProofOfVerification { get; set; }        
        public string Status { get; set; }
    }
}
