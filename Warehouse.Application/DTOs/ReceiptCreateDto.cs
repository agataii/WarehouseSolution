using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Application.DTOs
{
    public class ReceiptCreateDto
    {
        public string Number { get; set; } = null!;
        public DateTime Date { get; set; }
        public List<ReceiptItemCreateDto> Items { get; set; } = new();
    }

    public class ReceiptItemCreateDto
    {
        public int ResourceId { get; set; }
        public int UnitId { get; set; }
        public decimal Quantity { get; set; }
    }
}
