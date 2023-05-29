using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public record PicketInputView
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int? Number { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? WarehouseId { get; set; }
    }   
}
