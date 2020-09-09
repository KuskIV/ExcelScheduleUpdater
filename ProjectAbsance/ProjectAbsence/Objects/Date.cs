using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAbsence
{
    class Date
    {
        public int day { get; set; }
        public int month { get; set; }
        public int year { get; set; }

        public Date() { }

        public Date(string date)
        {
            string[] dateArr = date.Split('/');

            day = Int32.Parse(dateArr[0]);
            month = Int32.Parse(dateArr[1]);
            year = Int32.Parse(dateArr[2]);
        }

        public string GetDate()
        {
            return day + "/" + month + "/" + year;
        }
    }
}
