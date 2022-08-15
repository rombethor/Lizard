using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lizard.Models
{
    public class Source
    {
        public long SourceID { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Version { get; set; } = string.Empty;
    }
}
