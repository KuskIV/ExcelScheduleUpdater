using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAbsence
{
    class TextFile
    {
        /// <summary>
        /// This method gets the scheme based on url.
        /// This is done either from a txt file (if the exists)
        /// or it is crawled from the website, and then saved as a txt file.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<List<Day>> GetScheme(string url, string file, string lastDay)
        {
            string dayAsText;
            List<Day> days = new List<Day>();

            if (File.Exists(file))
            {
                // If the file exists, it is read.
                dayAsText = File.ReadAllText(file);
                days = JsonConvert.DeserializeObject<List<Day>>(dayAsText);
            }
            else
            {
                // If the file does not exists, it is created.
                Web web = new Web(url, lastDay);
                days.AddRange(await web.GetDays());
                Writefile(days, file);
            }

            return days;
        }


        /// <summary>
        /// Returns a list of Dates of all saturdays and sundays (weekends)
        /// </summary>
        /// <param name="days"></param>
        /// <returns></returns>
        public List<Date> GetWeekends(List<Day> days)
        {
            List<Date> weekend = new List<Date>();

            foreach (Day day in days)
            {
                if (IsWeekend(day))
                {
                    weekend.Add(day.date);
                }
            }

            return weekend;
        }

        #region Private methods
        void Writefile(List<Day> data, string path)
        {
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, data);
            }
        }

        bool IsWeekend(Day day)
        {
            return day.dayname.Equals("Saturday") || day.dayname.Equals("Sunday");
        }
        #endregion
    }
}
