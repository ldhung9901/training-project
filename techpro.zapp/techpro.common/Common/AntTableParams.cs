using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace techpro.common.common
{
    public class AntTableParams
    {
        public int pageSize { get; set; }
        public int pageIndex { get; set; }

        public string sortField { get; set; }

        public string sortOrder { get; set; }
        public List<ListFilter> filter { get; set; }
    }
    public class ListFilter
    {

        public string key { get; set; }

        public string value { get; set; }

    }
}