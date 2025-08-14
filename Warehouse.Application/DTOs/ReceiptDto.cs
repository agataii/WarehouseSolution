using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Application.DTOs
{
    public class ReceiptDto
    {
        public int Id { get; set; }
        public string Number { get; set; } = null!;
        public DateTime Date { get; set; }
        public List<ReceiptItemDto> Items { get; set; } = new();
    }

    public class ReceiptItemDto
    {
        public int ResourceId { get; set; }
        public string ResourceName { get; set; } = null!;
        public int UnitId { get; set; }
        public string UnitName { get; set; } = null!;
        public decimal Quantity { get; set; }
    }
}
