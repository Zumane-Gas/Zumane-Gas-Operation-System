using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZumaneGas_OperationalSystem.Models
{
    public class Empty_Cylinder
    {
        [Key]
        public int EmptiesID { get; set; }
        public string Empty_Size { get; set; }
        public int Empty_Size_Quantity { get; set; }
        public int Total_Empties_Large { get; set; }
        public int Total_Empties_Small { get; set; }
        public Sale SaleDetails { get; set; }
        public virtual Invetory GoodDetails { get; set; }
        public int? Invetory_Id { get; set; }
        public string[] getSize { get; set; }
        public int[] getQuantity { get; set; }
    }
}