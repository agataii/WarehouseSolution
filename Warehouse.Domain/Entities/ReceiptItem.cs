using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Domain.Entities
{
    public class ReceiptItem
    {
        public int Id { get; set; }

        public int ReceiptDocumentId { get; set; }
        public ReceiptDocument ReceiptDocument { get; set; } = null!;

        public int ResourceId { get; set; }
        public Resource Resource { get; set; } = null!;

        public int UnitId { get; set; }
        public Unit Unit { get; set; } = null!;

        public decimal Quantity { get; set; }
    }
}
