using System;
using System.Collections.Generic;
using System.Text;

namespace AppdReporter
{
    class DateParser
    {
        
        public String[] TimeTable;
       

        public DateParser(int daysCount)
        {
            this.TimeTable = timeMachine(daysCount);
        }

        public DateParser()
        {
            // For parsing purpose only
        }

        public DateParser(DateTime start, DateTime end)
        {
            this.TimeTable = timeMachine(start,end);
        }


        // Prepare timetable for each daytime span
        public string[] timeMachine(int days)
        {
            DateTime currentDay = DateTime.Today;
            DateTime previousDay = DateTime.Today.AddDays(-days);   
            return new [] { unixEpoch(previousDay), unixEpoch(currentDay) };
        }

        public string[] timeMachine(DateTime start , DateTime end)
        {
            return new[] { unixEpoch(start).Split('.')[0], unixEpoch(end).Split('.')[0] };
        }

        // Return epoch value to provide timestamps
        private String unixEpoch(DateTime dateTime)
        {
            return (dateTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds.ToString();
        }

        public string epochToReadable(string epoch)
        {
            DateTimeOffset dateTimeOffSet = DateTimeOffset.FromUnixTimeMilliseconds(long.Parse(epoch));
            DateTime dateTime = dateTimeOffSet.DateTime;
            return dateTime.ToString();
        }

        public string durationInMinutes(string start , string end)
        {
            return ((long.Parse(end) - long.Parse(start)) / 60000).ToString();
        }


    }
}
