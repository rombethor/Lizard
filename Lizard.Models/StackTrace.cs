using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lizard.Models
{
    public class StackTrace
    {
        [Required, MaxLength(36)]
        public string MethodName { get; set; } = string.Empty;

        [Required, MaxLength(5000)]
        public string Content { get; set; } = string.Empty;
    }
}
