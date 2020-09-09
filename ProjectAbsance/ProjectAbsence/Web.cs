using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAbsence
{
    class Web
    {
        public string url;

        public Web(string _url, string _lastDay)
        {
            url = _url;
            lastDay = _lastDay;
        }

        // .contains if it is the last day of the semester
        string lastDay;

        public List<Lecture> GetLectures(HtmlNode node)
        {
            List<HtmlNode> temp = node.Descendants("div").Where(x => x.GetAttributeValue("class", "").Equals("event")).ToList();
            List<Lecture> result = new List<Lecture>();
            List<string> time = new List<string>();

            foreach (HtmlNode n in temp)
            {
                time = n.Descendants("div").Where(x => x.GetAttributeValue("class", "").Equals("time")).FirstOrDefault().InnerText.Split('-').ToList();
                Lecture e = new Lecture()
                {
                    name = n.Descendants("a").FirstOrDefault().InnerText,
                    teacher = n.Descendants("div").Where(x => x.GetAttributeValue("class", "").Equals("teacher")).FirstOrDefault().InnerText,
                    start = new Time(time[0]),
                    end = new Time(time[1]),
                    location = n.Descendants("div").Where(x => x.GetAttributeValue("class", "").Equals("location")).FirstOrDefault().InnerText,
                    note = n.Descendants("div").Where(x => x.GetAttributeValue("class", "").Equals("note")).FirstOrDefault().InnerText
                };
                result.Add(e);
            }
            return result;
        }

        public async Task<List<Day>> GetDays()
        {
            List<Day> days = new List<Day>();
            List<Lecture> temp_Lecture = new List<Lecture>();

            List<int> weeks = await GetWeekList();
            List<HtmlNode> divs = await GetDivs();

            int week = 0;
            double weekCount = 0;

            foreach (var div in divs)
            {
                week = (int)Math.Floor(weekCount / 7);
                days.Add(MakeDay(div, weeks[week]));

                if (LastDay(days.Last()))
                {
                    break;
                }
                weekCount += 1;
            }
            return days;
        }

        async Task<List<int>> GetWeekList()
        {
            List<HtmlNode> hTwo = await GetHtwo();
            List<int> weekNum = new List<int>();
            string day;
            string remove = "Week ";

            foreach (HtmlNode w in hTwo)
            {
                day = w.Descendants("a").FirstOrDefault().InnerText.Remove(0, remove.Length);
                weekNum.Add(Int32.Parse(day));
            }
            return weekNum;
        }

        async Task<List<HtmlNode>> GetHtwo()
        {
            HttpClient httpClient = new HttpClient();
            string html = await httpClient.GetStringAsync(url);
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            return htmlDocument.DocumentNode.Descendants("h2")
                .Where(node => node.GetAttributeValue("class", "").Equals("week")).ToList();
        }

        public bool LastDay(Day day)
        {
            if (day.lectures.Count == 0)
            {
                return false;
            }
            return day.lectures.Any(x => x.note.ToLower().Contains(lastDay));
        }

        public async Task<List<HtmlNode>> GetDivs()
        {
            HttpClient httpClient = new HttpClient();
            string html = await httpClient.GetStringAsync(url);
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            return htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "").Equals("day")).ToList();
        }

        public Day MakeDay(HtmlNode node, int weekNum)
        {
            List<Lecture> temp_Lecture = GetLectures(node);

            Day tempDay = new Day()
            {
                weekNumber = weekNum,
                dayname = node.Descendants("h3").FirstOrDefault().InnerText,
                date = new Date(node.Descendants("div").FirstOrDefault().InnerText),
                lectures = temp_Lecture.Count > 0 ? temp_Lecture : new List<Lecture>()
            };

            return tempDay;
        }

    }
}
