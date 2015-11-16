using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFSToolset.Logic.Models
{
    class TfsModel
    {
        string TfsServerName { get; set; }
        string TfsProjectName { get; set; }
        string OriginalQueryFolder { get; set; }
        string NewQueryFolder { get; set; }
        string OriginalQueryText { get; set; }
        string NewQueryText { get; set; }
    }
}
