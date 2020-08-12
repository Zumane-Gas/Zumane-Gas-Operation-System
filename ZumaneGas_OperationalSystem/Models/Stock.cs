using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZumaneGas_OperationalSystem.Models
{
    public class Stock
    {
        [Key]
        public int Stock_Id { get; set; }
        public virtual Invetory Inventory { get; set; }
        public int Invetory_Id { get; set; }
        public string Report { get; set; }
        public int Quantity { get; set; } 
        public int? Cylinder_Size { get; set; }
        public string C_Size { get; set; }
        public string ProductName { get; set; }
        public int? NewStock { get; set; }
        public Status Status { get; set; }
    }
    public enum Status
    {
        Available, OutOfStock
    }
}