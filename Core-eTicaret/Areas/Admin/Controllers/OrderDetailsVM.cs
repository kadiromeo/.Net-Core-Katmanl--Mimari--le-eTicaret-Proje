using Models;
using System.Collections.Generic;

namespace Core_eTicaret.Areas.Admin.Controllers
{
    public class OrderDetailsVM
    {
        public OrderHeader OrderHeader { get; internal set; }
        public IEnumerable<OrderDetails> OrderDetails { get; internal set; }
    }
}