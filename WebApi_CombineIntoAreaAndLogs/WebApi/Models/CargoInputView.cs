using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public record CargoInputView
    {
        [Required]
        [Range(1, double.MaxValue)]
        public double? Weight { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? PicketId { get; set; }
    }
}
