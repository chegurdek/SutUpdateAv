using System.Data;
using System.Data.Common;

namespace Elsy.UoCommon.Models
{
    public class Parameter 
    {
        public string ParameterName { get; set; }
        public ParameterDirection Direction { get; set; }
        public DbType DbType { get; set; }
        public object Value { get; set; }
        public string SourceColumn { get; set; }
        public int Size { get; set; }

    }
}
