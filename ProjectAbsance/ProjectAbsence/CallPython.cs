using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAbsence
{
    class CallPython
    {
        public CallPython(string _url, string _txtPath, string _pyPath, string _exePath, string _lastDay, Time _limit, int _pplAmount)
        {
            url = _url;
            txtPath = _txtPath;
            pyPath = _pyPath;
            limit = _limit;
            exePath = _exePath;
            pplAmount = _pplAmount;
            lastDay = _lastDay;
        }

        string url;
        string txtPath;
        string pyPath;
        string exePath;
        string lastDay;

        int pplAmount;
        
        List<Day> days = new List<Day>();
        List<Date> weekend = new List<Date>();
        
        TextFile text = new TextFile();
        PyClass Python = new PyClass();

        Time limit;
        
        Get get;
        
        PythonVar pyVar;

        /// <summary>
        /// This method updates the excel for one person, based
        /// on the data saved on the person.
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public async Task<string> UpdateExcel(Person person)
        {
            await SetupLists();

            pyVar = new PythonVar(pplAmount.ToString(), days.Count.ToString());

            foreach (MetTimes mt in person.time)
            {
                CheckOneTime(person, mt.date, mt.time);
            }

            return ApplyChanges();
        }

        /// <summary>
        /// THis method updates the excel for a list of persons
        /// based on data saved on the person.
        /// </summary>
        /// <param name="persons"></param>
        /// <returns></returns>
        public async Task<string> UpdateExcel(List<Person> persons)
        {
            await SetupLists();

            pyVar = new PythonVar(pplAmount.ToString(), days.Count.ToString());

            foreach (Person person in persons)
            {
                foreach (MetTimes mt in person.time)
                {
                    CheckOneTime(person, mt.date, mt.time);
                }
            }

            return ApplyChanges();
        }

        /// <summary>
        /// Based on the Person a Date and when he met in on that Date
        /// the amount of minutes he was late is added to the object
        /// sent to the python script.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="date"></param>
        /// <param name="time"></param>
        void CheckOneTime(Person p, Date date, Time time)
        {
            Time meet = new Time();
            int minLate;
            meet = get.GetMeetingTime(date);

            if (!get.IsWeekend(date))
            {
                minLate = get.MinLate(time, meet);
                pyVar.AddInstance(date, minLate.ToString(), p.name);
            }
        }

        #region private methods

        string ApplyChanges()
        {
            if (pyVar.person.Count > 0)
            {
                string test = Python.Call(pyPath, exePath, pyVar);
                return test;
            }
            return "";
        }

        private async Task SetupLists()
        {
            if (!IsSetup())
            {
                days = await text.GetScheme(url, txtPath, lastDay);
                weekend = text.GetWeekends(days);
                get = new Get(days, weekend, limit);
            }
        }

        private bool IsSetup()
        {
            return get != null;
        }

        #endregion
    }
}
