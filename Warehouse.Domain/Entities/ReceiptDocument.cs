using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Domain.Entities
{
    public class ReceiptDocument
    {
        public int Id { get; set; }
        public string Number { get; set; } = null!;
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public ICollection<ReceiptItem> Items { get; set; } = new List<ReceiptItem>();
    }
}
