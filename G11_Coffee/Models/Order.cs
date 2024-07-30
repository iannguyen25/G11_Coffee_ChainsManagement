using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace G11_Coffee.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public int CafeId { get; set; }
        public Cafe Cafe { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

    }
}
