using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;

namespace Services.Models
{
    public record Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Picket> Pickets { get; set; }
    }
}
