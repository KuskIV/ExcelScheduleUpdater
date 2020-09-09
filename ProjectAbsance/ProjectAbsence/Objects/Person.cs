using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spire.Xls;

namespace ProjectAbsence
{
    class Person
    {
        public string name { get; set; }
        public List<MetTimes> time = new List<MetTimes>();

        public Person(string _name, Time _metTime, Date _date)
        {
            name = _name;
            time.Add(new MetTimes(_date, _metTime));
        }

        public Person(string _name)
        {
            name = _name;
        }

        /// <summary>
        /// A Date and a Time met that day is added to person.
        /// </summary>
        /// <param name="_metTime"></param>
        /// <param name="_date"></param>
        public void AddTime(Time _metTime, Date _date)
        {
            time.Add(new MetTimes(_date, _metTime));
        }

        public void TimerList(List<MetTimes> metTimes)
        {
            time.AddRange(metTimes);
        }
    }
}
