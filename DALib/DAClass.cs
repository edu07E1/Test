using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DALib.BaseCode;


namespace DALib
{
    /// <summary>
    /// Abstract class for Data Access Layer
    /// </summary>
    public abstract class DAClass
    {
        protected const char FIELD_SEPARATOR_CHAR = ' ';

        public abstract int ProcessFile(IEnumerable<string> dataStr);
        public abstract IEnumerable<DriversReport> GenerateReport();
    }
}
