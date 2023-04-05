using System;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystemForWeb.Models.VO
{
    public class BookTypeChartVO
    {
        public string BtName { get; set; }
        [Key]
        public int Sum { get;set; }
    }
}