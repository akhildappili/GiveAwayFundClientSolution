using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GiveAwayFundClientProject.Models.ViewModels
{
    public class EventRaiser
    {
        public int FundId { get; set; }
        public int? NgoId { get; set; }
        public string FundDescription { get; set; }
        public int? FundPrice { get; set; }
    }
}
