using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
{
	public class Invoice
	{
        [Key]
        public int Id { get; set; }
        public string CostumerId { get; set; }
        public int ProductId { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }
        public double Tax { get; set; }
        public double Total { get; set; }
    }
}
