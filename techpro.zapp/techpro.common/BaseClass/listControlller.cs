using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using techpro.common.Models;

namespace techpro.common.BaseClass
{
    public  class ListControlller
    {
        public static List<ControllerAppModel> list { get; set; }
        public static List<string> listpublicactioncontroller { get; set; }
    }
}
