using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cansat.Setebos.Web.Models
{
    public class FlexigridResponseRow<T, TKey>
    {
        public TKey id { get; set; }
        public T cell { get; set; }
    }
}