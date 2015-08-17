using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAD.Azure
{
    public interface INoSQLDocument
    {
        string DocumentLink { get; set; }
        string Id { get; set; }
        
    }
}
