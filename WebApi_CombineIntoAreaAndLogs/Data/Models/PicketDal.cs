using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class PicketDal
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public bool Deleted { set; get; }

        public int WarehouseId { get; set; }
        public WarehouseDal Warehouse { get; set; }

        public int? AreaId { get; set; }
        public AreaDal? Area { get; set; }

        public ICollection<CargoDal> Cargoes { get; set; }
    }
}
