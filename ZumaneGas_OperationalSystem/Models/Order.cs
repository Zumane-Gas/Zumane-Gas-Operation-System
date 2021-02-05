using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
        public DateTime? Order_Date { get; set; }
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

        //public Order()
        //{

        //}
        //public Order(int OrderID, int Qty1, int price1, DateTime? Date){
        //    this.Order_ID = Order_ID;
        //    this.Order_Qty1 = Qty1;
        //    this.Order_price1 = price1;
        //    this.Order_Date = Date;
        //}

    }
    public enum PaymentStatus
    {
        NotPaid, Paid
    }

    public enum PaymentType
    {
        Cash, EFT
    }
    public enum OrderStatus
    {
        Processed, NotProcessed, Cancelled
    }
}
