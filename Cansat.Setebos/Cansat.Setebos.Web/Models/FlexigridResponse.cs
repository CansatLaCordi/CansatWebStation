using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cansat.Setebos.Web.Models
{
    public class FlexigridResponse<T,TKey>
    {
        public int page { get; set; }
        public int total { get; set; }
        public List<FlexigridResponseRow<T,TKey>> rows { get; set; }

        public void GenerateResponseRows(List<T> list, System.Linq.Expressions.Expression<Func<T, TKey>> Id) { 
            rows = list.Select( o => new FlexigridResponseRow<T,TKey>(){ id = Id.Compile()(o), cell = o}).ToList();

        }
    }
}