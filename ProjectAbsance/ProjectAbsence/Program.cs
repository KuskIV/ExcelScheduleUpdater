using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ProjectAbsence
{
    class Program
    {
        static async Task Main(string[] args)
        {
            #region Setup
            string schemeId = "7566";
            string startDate = "2020-02-09";
            string lastDay = "2020-21-12"; // Note on last day of semester
            int pplAmount = 6; // Group size
            Time Limit = new Time(9, 0); // Aftaltte mødetid
            #endregion

            #region Paths (unique)
            string url = $"https://www.moodle.aau.dk/calmoodle/public/?sid={schemeId}&startdate={startDate}";
            string file; //= @"C:\Users\madsh\OneDrive\Universitetet\Github Projects\AbsenceFile\scheme.txt";
            file = Path.GetFullPath("semester5schedule.txt");
            string python = @"C:\Users\madsh\OneDrive\Universitetet\Github Projects\pyExcel\Main.py";
            string exePath = @"C:\Users\madsh\AppData\Local\Programs\Python\Python38-32\python.exe";
            #endregion

            #region Dummy data

            List<Person> persons = new List<Person>();

            Person p = new Person("Mads");
            Person r = new Person("Roni");

            Date da = new Date("10/02/2020"); // møde kl 09:00 ingen forelæsning
            Time a = new Time(9, 1);

            Date db = new Date("11/02/2020"); // møde kl 8:15 forelæsning
            Time b = new Time(8, 16);

            //Date dc = new Date("28/05/2020"); // møde kl 12:00 før
            //Time c = new Time(9, 1);

            Date dd = new Date("13/02/2020"); // møde kl 14:00 efter forelæsning
            Time d = new Time(9, 1);

            Date de = new Date("17/02/2020"); // møde kl 09:00 før forelæsning
            Time e = new Time(9, 1);

            Date df = new Date("15/02/2020"); // weekend
            Time f = new Time(11, 0);

            Date test = new Date("17/02/2020");
            Time t = new Time(15,29);
            
            p.AddTime(t, test);
            p.AddTime(a, da);
            p.AddTime(b, db);
            //p.AddTime(c, dc);
            p.AddTime(d, dd);
            p.AddTime(e, de);
            p.AddTime(f, df);

            r.AddTime(a, da);
            r.AddTime(b, db);
            //r.AddTime(c, dc);
            r.AddTime(d, dd);
            r.AddTime(e, de);
            r.AddTime(f, df);

            persons.Add(p);
            persons.Add(r);

            #endregion

            CallPython py = new CallPython(url, file, python, exePath, lastDay, Limit, pplAmount);

            // Update excel for a list of persons or one person
            string response = await py.UpdateExcel(persons);

            Console.WriteLine(string.IsNullOrEmpty(response) ? "Success!" : "Error:\n" + response);
            Console.ReadKey();
        }
    }
}
