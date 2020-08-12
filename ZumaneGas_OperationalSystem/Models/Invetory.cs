using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZumaneGas_OperationalSystem.Models
{
    public class Invetory
    {
        [Key]
        public int Invetory_Id { get; set; }
        public int? Cylinder_Size { get; set; }
        public int? Cylinder_Price { get; set; }
        public string Size_Code { get; set; }
        public string Gas_Size { get; set; }
        public string ProductName { get; set; }
        public int? Product_Price { get; set; }
        public string Deposit { get; set; }
        public int DepositFee { get; set; }
        public string Inv_Description { get; set; }
    }
}