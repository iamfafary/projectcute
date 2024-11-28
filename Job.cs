using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gradpath
{
    public class Job
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Skills { get; set; } = new List<string>();
    }
}