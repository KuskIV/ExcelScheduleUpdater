using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectAbsence
{
    class Get
    {
        public Get(List<Day> _days, List<Date> _weekend, Time _limit)
        {
            days = _days;
            weekend = _weekend;
            limit = _limit;
        }

        List<Day> days;
        List<Date> weekend;
        Time limit;

        // .equals if a event on a day is a lecture
        string lecture = "lecture";
        
        // .contains if a event is exercise
        string execise = "exercises";

        /// <summary>
        /// Based on a date, this method find the day
        /// and what lectures (if any) there are.
        /// Based on this a Time is returned to show when to meet
        /// on the following date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public Time GetMeetingTime(Date date)
        {
            if (IsWeekend(date))
            {
                string test = date.day.ToString();
                return new Time();
            }

            foreach (Day day in days)
            {
                if (RightDate(date, day))
                {
                    return day.lectures.Count > 0 ? FindExerciseTime(day) : limit;
                }
            }
            return limit;
        }


        /// <summary>
        /// Based on a date, a Boolean value is returned
        /// to show if it is weekend or not.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool IsWeekend(Date date)
        {
            return weekend.Exists(t => SameTime(t, date));
        }

        /// <summary>
        /// Based on when a person met compared to when they should have met,
        /// the amount of minutes late is decided and returned, with a limit
        /// of 60 minutes (as decided by the group)
        /// </summary>
        /// <param name="metTime"></param>
        /// <param name="meetingTime"></param>
        /// <returns></returns>
        public int MinLate(Time metTime, Time meetingTime)
        {
            if (Late(metTime, meetingTime))
            {
                int minLate = CalcMinLate(metTime, meetingTime);
                return minLate;
                
            }
            else
            {
                return 0;
            }
        }

        #region private methods

        Time FindExerciseTime(Day d)
        {
            Lecture l = d.lectures.Where(x => x.note.ToLower().Contains(execise)).FirstOrDefault();
            return l != null ? ApplyAcademicQuater(d, l.start) : CheckFirstLecture(d.lectures[0]);
        }

        Time CheckFirstLecture(Lecture l)
        {
            // Hvad skal der ske hvis der var forelæsning kl 10:15? Ingen implementation lavet.
            return IsLecture(l) && l.start.hour <= limit.hour ? PlusMin(l.end, 15) : limit;
        }

        bool IsLecture(Lecture l)
        {
            return l.note.ToLower().Contains(lecture) && !l.note.ToLower().Contains(execise);
        }

        Time ApplyAcademicQuater(Day d, Time t)
        {
            return d.lectures[0].start.hour > limit.hour ? limit : d.lectures[0].note.ToLower().Equals(lecture) ? PlusMin(t, 15) : t;
        }

        Time PlusMin(Time t, int extra)
        {
            int min = t.minute;
            int hour = t.hour;
            return min + extra < 60 ? new Time(hour, min + extra) : new Time(hour + (min + extra % 60), min + extra % 60);
        }

        bool RightDate(Date time, Day day)
        {
            return day.date.GetDate().Equals(time.GetDate());
        }

        int CalcMinLate(Time metTime, Time meetingTime)
        {
            return LimitReached(metTime, meetingTime) ? 60 : !SameHour(metTime, meetingTime) ? (60 - meetingTime.minute) + metTime.minute : metTime.minute - meetingTime.minute;
        }

        static bool Late(Time metTime, Time meetingTime)
        {
            return metTime.hour > meetingTime.hour || metTime.hour == meetingTime.hour && metTime.minute > meetingTime.minute;
        }

        bool SameTime(Date x, Date y)
        {
            return x.year == y.year && x.month == y.month && x.day == y.day ? true : false;
        }

        bool LimitReached(Time metTime, Time meetingTime)
        {
            return metTime.hour > meetingTime.hour + 1 || metTime.hour > meetingTime.hour && metTime.minute > meetingTime.minute;
        }

        bool SameHour(Time metTime, Time meetingTime)
        {
            return metTime.hour == meetingTime.hour;
        }

        #endregion
    }
}
