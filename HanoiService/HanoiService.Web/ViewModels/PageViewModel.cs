using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HanoiService.Web.ViewModels
{
    public class PageViewModel
    {
        public int Index { get; set; } = 0;
        public int Size { get; set; } = 10;
    }
}