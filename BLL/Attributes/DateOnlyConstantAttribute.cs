using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Attributes
{
    public class DateOnlyConstantAttribute : CustomConstantAttribute
    {
        private string _date;

        public DateOnlyConstantAttribute()
        {
            _date = DateTime.Today.ToShortDateString();
        }

        public override object? Value => _date;
    }
}
