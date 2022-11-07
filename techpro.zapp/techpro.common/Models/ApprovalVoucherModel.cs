using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace techpro.common.Models
{
    public class ApprovalVoucher
    {
        public dynamic voucher_data { get; set; }
        public string voucher_detail { get; set; }
        public string menu_name { get; set; }
    }
}
