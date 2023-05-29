using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public record PicketAreaView
    {
        public string WarehouseName { get; set; }
        public IEnumerable<AreaView> Areas { get; set; }
    }

    public record AreaView
    {
        public IEnumerable<int> PicketNumbers { get; set; }
        public double Weight { get; set; }
        public string AreaName { get; set; }
        public DateTime Created { get; set; }
    }
}
