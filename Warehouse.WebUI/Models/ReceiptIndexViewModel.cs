using Microsoft.AspNetCore.Mvc.Rendering;
using Warehouse.Application.DTOs;

namespace Warehouse.WebUI.Models
{
    public class ReceiptIndexViewModel
    {
        public IEnumerable<ReceiptDto> Receipts { get; set; } = [];
        public ReceiptFilterDto Filter { get; set; } = new();
        public List<string> Numbers { get; set; } = [];
        public SelectList Resources { get; set; }
        public SelectList Units { get; set; }
    }
}