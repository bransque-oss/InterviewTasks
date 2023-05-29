using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public record PicketArea
    {
        public string WarehouseName { get; set; }
        public int PicketId { get; set; }
        public int PicketNumber { get; set; }
        public double Weight { get; set; }
        public string AreaName { get; set; }
        public DateTime Created { get; set; }
    }
}
