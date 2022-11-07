using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace techpro.common.common
{
    public class ApexData
    {
        public ApexData()
        {
            labels = new List<string>();
            series = new List<double>();
            LineSeries = new List<double>();
            ColumnSeries = new List<double>();
        }
        public List<string> labels { get; set; }
        public List<double> series { get; set; }
        public List<double> LineSeries { get; set; }
        public List<double> ColumnSeries { get; set; }
    }
}