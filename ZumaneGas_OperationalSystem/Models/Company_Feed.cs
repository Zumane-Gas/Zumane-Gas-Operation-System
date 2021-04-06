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
        public DateTime? DateTime { get; set; }
        public string Details { get; set; }
        public string Updated_Item { get; set; }
        //for Payout 
        public string updated_Cost { get; set; }
        public string Old_Quantity { get; set; }
        public string New_Quantiy { get; set; }
        //for Order
        public string[] Items { get; set; }
        public string OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public string OrderInfomation_OrderNumber { get;set; }
        public string OrderInformation_CustomerName { get; set; }
        public string OrderInformation_Contact { get; set; }
        public string OrderInformation_Address { get; set; }
        //<h5>Customer Name:</h5>
        //<h5>Contact Number:</h5>
        //<h5>Alternate Number:</h5>
        //<h5>Delivery Address:</h5>


    }
}