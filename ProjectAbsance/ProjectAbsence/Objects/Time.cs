using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAbsence
{
    class Time
    {
        public int hour { get; set; }
        public int minute { get; set; }

        public Time(int _hour, int _minute)
        {
            hour = _hour;
            minute = _minute;
        }

        public Time(string date)
        {
            if (date.Contains("Time:"))
            {
                date = date.Remove(0, 5);
            }
            string[] timeArr = date.Split(':');
            hour = Int32.Parse(timeArr[0]);
            minute = Int32.Parse(timeArr[1]);
        }

        public Time() { }

        public string GetTime()
        {
            return hour + ":" + minute;
        }
    }
}
