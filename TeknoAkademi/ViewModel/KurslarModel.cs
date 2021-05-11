using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeknoAkademi.ViewModel
{
    public class KurslarModel
    {
        public int kursId { get; set; }
        public string kursAd { get; set; }
        public decimal kursFiyat { get; set; }
        public string kursIcerik { get; set; }
        public int kursKategoriId { get; set; }
        public int kursEgitmenId { get; set; }
    }
}