using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyLibrary.Models
{
    public class BookViewModel
    {
        public String Title;

        public String Author;

        public String ISBN;

        public int Year;

        public int availableCount;

        public int copyCount;
    }
}