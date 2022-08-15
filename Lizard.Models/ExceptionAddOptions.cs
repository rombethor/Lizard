using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lizard.Models
{
    public class ExceptionAddOptions
    {
        public ExceptionAddOptions(Exception ex)
        {
            if (ex.StackTrace != null || ex.TargetSite != null)
            {
                StackTrace = new StackTrace()
                {
                    Content = ex.StackTrace ?? String.Empty,
                    MethodName = ex.TargetSite?.Name ?? string.Empty
                };
            }
            if (ex.InnerException != null)
                InnerException = new ExceptionAddOptions(ex.InnerException);
        }

        public StackTrace? StackTrace { get; set; }

        public ExceptionAddOptions? InnerException { get; set; }
    }
}
