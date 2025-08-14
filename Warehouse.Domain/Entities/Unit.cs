using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Domain.Entities
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; } = true;

        public ICollection<ReceiptItem> ReceiptItems { get; set; } = new List<ReceiptItem>();
    }
}
