using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZumaneGas_OperationalSystem.Models
{
    public class Daily_Report
    {
        [Key]
        public int Report_ID { get; set; }
        public double Total_Revenue { get; set; }
        public double CashSales { get; set; }
        public double EFTSales { get; set; }
        public double Payout_Amount { get; set; }
        public double Closing_Amount { get; set; }
        public int Refills { get; set; }
        public int Deposits { get; set; }
        public int Cooker_Tops { get; set; }
        public int CylinderKeys { get; set; }
        public int Exchanges_9 { get; set; }
        public int Exchanges_14 { get; set; }
        public int Exchanges_19 { get; set; }
        public int Exchanges_48 { get; set; }
        public int Regulators { get; set; }
        public Shift Shift { get; set; }
        public string Search_Shift { get; set; }
        public DateTime Report_Date { get; set; }
        public string Search_Date { get; set; }
        public List<Stock> ClosingStock { get; set; }
        public List<DailyPayout> RecordedPayouts { get; set; }
        public int Current_9kg { get; set; }
        public int Current_14kg { get; set; }
        public int Current_19kg { get; set; }
        public int Current_48kg { get; set; }
        public int Current_CT { get; set; }
        public int Current_Key { get; set; }
        public int Current_Reg { get; set; }
        public List<string> PayoutsDesc { get; set; }
        public int[] PayoutAmt { get; set; }

        //Opening Stock 
        public int Opening_9kg { get; set; }
        public int Opening_14kg { get; set; }
        public int Opening_19kg { get; set; }
        public int Opening_48kg { get; set; }
        public int Opening_CT { get; set; }
        public int Opening_Reg { get; set; }
        public int Opening_Key { get; set; }
    }

    public enum Shift
    {
        Day, Night
    }
}