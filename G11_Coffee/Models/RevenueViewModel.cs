using G11_Coffee.Models;

namespace G11_Coffee.Models
{
    public class RevenueViewModel
    {
        public decimal TotalRevenue { get; set; }
        public List<Order> Orders { get; set; }
        public DateTime? StartDate { get; set; } // Thêm thuộc tính này
        public DateTime? EndDate { get; set; } // Thêm thuộc tính này
    }

}