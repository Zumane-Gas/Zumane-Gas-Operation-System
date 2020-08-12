using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZumaneGas_OperationalSystem.Models
{
    public class DailyPayout
    {
        [Key]
        public int Payout_ID { get; set; }
        public string Payout_Descriptiom { get; set; }
        public int Payout_Amount { get; set; }
        public DateTime Payout_Date { get; set; }
    }
}