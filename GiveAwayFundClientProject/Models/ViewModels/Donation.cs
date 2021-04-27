using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GiveAwayFundClientProject.Models.ViewModels
{
    public class Donation
    {

        public int TransactionId { get; set; }
        public int? DonorId { get; set; }
        public int? NgoId { get; set; }

        public int? Amount { get; set; }
        [Required]
        public string Description { get; set; }

        public virtual Donor Donor { get; set; }
        public virtual NGO NGO { get; set; }
    }
}
