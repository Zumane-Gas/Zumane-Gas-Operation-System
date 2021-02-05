using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZumaneGas_OperationalSystem.Models
{
    public class Company_Feed
    {
        [Key]
        public int Feed_Id { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public DateTime DateTime { get; set; }
        public string Details { get; set; }
        public string Updated_Item { get; set; }
        //for Payout 
        public string updated_Cost { get; set; }
        public string Old_Quantity { get; set; }
        public string New_Quantiy { get; set; }


    }
}