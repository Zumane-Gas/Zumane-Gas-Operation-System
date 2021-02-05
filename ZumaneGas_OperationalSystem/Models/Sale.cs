using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZumaneGas_OperationalSystem.Models
{
    public class Sale
    {
        [Key]
        public int Sale_Id { get; set; }
        public string InvoiceId { get; set; }
        //public virtual Stock Stock { get; set; }
        public int? Stock_Id { get; set; }
        public int? Invetory_Id { get; set; }
        public virtual Invetory Inventory { get; set; }
        public string InvId { get; set; }
        public int Quantity { get; set; }
        public string item1 { get; set; }
        public string item2 { get; set; }
        public string item3 { get; set; }
        public int price1 { get; set; }
        public int price2 { get; set; }
        public int price3 { get; set; }
        public int Qty1 { get; set; }
        public int Qty2 { get; set; }
        public int Qty3 { get; set; }
        public double UnitPrice { get; set; }
        public double Vat { get; set; }
        public double CombinedPrice { get; set; }
        public double finalPrice { get; set; }
        public string Description { get; set; }
        [DisplayName("Payment Method")]
        public SaleType SaleType { get; set; }
        public ServiceType ServiceType { get; set; }
        public SaleStatus SaleStatus { get; set; }
        public DepositStatus DepositStatus { get; set; }
        public double cashReceived { get; set; }
        public double change { get; set; }
        //Gas Appliances
        public string Product_Item1 { get; set; }
        public string Product_Item2 { get; set; }
        public int Product_Price1 { get; set; }
        public int Product_Price2 { get; set; }
        public int Product_Qty1 { get; set; }
        public int Product_Qty2 { get; set; }
        public string SaleDate { get; set; }
        public DateTime SaleD { get; set; }
        public string DepositItem { get; set; }
        public int DepositPrice { get; set; }
        public int DepositQuantity { get; set; }
        public DateTime? DepositReturnDate { get; set; }
        [DisplayName("New 48kg")]
        public bool New { get; set; }
        public bool Delivery { get; set; }
        public int TotalItems { get; set; }

    }
    public enum SaleType
    {
        Cash, EFT
    }

    public enum ServiceType
    {
        Exchange, Refill
    }
    public enum SaleStatus
    {
        Sold, Cancelled
    }
    public enum DepositStatus
    {
        NotReturned, Returned
    }

}