using System.ComponentModel.DataAnnotations;

namespace APBD3_ASYNC.Models
{
    public class Warehouse
    {
        [Required]
        public int IdProduct { get; set; }
        [Required]
        public int IdWarehouse { get; set; }
        [Required]
        public int Amount { get; set; }

        public DateTime CreatedAt { get { return DateTime.Now; } }

    }
}
