using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class CargoDal
    {
        public int Id { get; set; }
        public double Weight { get; set; }

        public int PicketId { get; set; }
        public PicketDal Picket { get; set; }
    }
}
