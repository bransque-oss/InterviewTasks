using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public record Picket(int Id, int Number, int? AreaId, string? AreaName);
}
