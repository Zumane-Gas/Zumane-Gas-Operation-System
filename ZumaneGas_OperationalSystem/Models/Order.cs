using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ZumaneGas_OperationalSystem.Models
{
    public class Order
    {
        [Key]
        public int Order_ID { get; set; }
        public string Order_Number { get; set; }
        public string Order_item1 { get; set; }
        public string Order_item2 { get; set; }
        public int Order_price1 { get; set; }
        public int Order_price2 { get; set; }
        public int Order_Qty1 { get; set; }
        public int Order_Qty2 { get; set; }
        public double UnitPrice { get; set; }
        public double Vat { get; set; }
        public double CombinedPrice { get; set; }
        public double finalPrice { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public PaymentType PaymentType { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
        public double Order_cashReceived { get; set; }
        public double Order_change { get; set; }
        //Gas Appliances
        //public string Product_Item1 { get; set; }
        //public string Product_Item2 { get; set; }
        //public int Product_Price1 { get; set; }
        //public int Product_Price2 { get; set; }
        //public int Product_Qty1 { get; set; }
        //public int Product_Qty2 { get; set; }
        public string SaleDate { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? Order_Date { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Alias_Order_Date { get; set; }
        public string Order_DepositItem { get; set; }
        public int Order_DepositPrice { get; set; }
        public int Order_DepositQuantity { get; set; }
        public DateTime? ProcessingDate { get; set; }
        public bool isToday { get; set; }
        public string ContactNumber { get; set; }
        public string AltrenateContactNum { get; set; }
        public string CustomerFullName { get; set; }
        public string Address_Line1 { get; set; }
        public string StreetName_Number { get; set; }
        public int Order_Counter { get; set; }
        public bool isViewed { get; set; }
        public string[] notes { get; set; }
        //[ForeignKey("Sale_Id")]
        //public virtual Sale sale { get; set; }
        public int? Sale_Id { get; set; }
        public string Order_InvoiceNumber { get; set; }

    }
    public enum PaymentStatus
    {
        NotPaid, Paid
    }
    public enum PaymentType
    {
       Cash, EFT, NotPaid
    }
    public enum OrderStatus
    {
        Processed, Processing, Cancelled
    }
    public enum DeliveryStatus
    {

        NotDelivered, Delivered 
    }
}
