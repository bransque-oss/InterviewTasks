using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class PicketAreaDal
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int PicketId { get; set; }
        public double Weight { get; set; }
        public string AreaName { get; set; }
        public required DateTime Created { get; set; }
    }
}
