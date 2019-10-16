using BigPicture.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigPicture.Resolver.Oracle.Nodes
{
    public class Column : Node
    {

        public String DatabaseName { get; set; }
        public String Schema { get; set; }
        public String TableName { get; set; }

        public Int32 ColumnId { get; set; }
        public String DataType { get; set; }
        public Int32 DataLength { get; set; }
        public Int32 DataPrecision { get; set; }
        public Int32 DataScale { get; set; }
        public String Nullable { get; set; }
    }
}
