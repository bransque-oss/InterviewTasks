﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models
{
    public record Cargo(int Id, double Weight, int PicketId, int PicketNumber);
}
