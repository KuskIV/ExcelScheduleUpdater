using System.Collections.Generic;

namespace ProjectAbsence
{
    class PythonVar
    {
        public List<string> date = new List<string>();
        public List<string> value = new List<string>();
        public List<string> person = new List<string>();
        public string amount_person { get; set; }
        public string amount_day { get; set; }

        public PythonVar(string a_per, string a_day)
        {
            amount_person = a_per;
            amount_day = a_day;
        }

        public void AddInstance(Date d, string v, string p)
        {
            date.Add(SetupDate(d));
            value.Add(v);
            person.Add(p);
        }

        /// <summary>
        /// The date is converted to how the python script reads it
        /// and returned as a string.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        string SetupDate(Date d)
        {
            return d.year + "-" + d.month.ToString().PadLeft(2, '0') + "-" + d.day.ToString().PadLeft(2, '0');
        }
    }
}
