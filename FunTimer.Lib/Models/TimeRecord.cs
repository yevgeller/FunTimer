using System;
using System.Diagnostics;

namespace FunTimer.Lib.Models
{
    [DebuggerDisplay("{StartTime}-{EndTime}:{TimeRecordType}")]
    public class TimeRecord
    {
        public int Id { get; set; }
        public TimeRecordTypeEnum TimeRecordType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string ConvertToString()
        {
            return string.Format("{0}{3}{1}{3}{2}{4}", TimeRecordType, StartTime, EndTime, ElementSeparator, RecordsSeparator);
        }

        public string ForDisplay()
        {
            string result = StartTime.ToShortTimeString() + " - " + EndTime.ToShortTimeString();

            TimeSpan diff = EndTime.Subtract(StartTime);

            result = TimeRecordType == TimeRecordTypeEnum.FunTimePeriod ? "Fun: " : "Work: ";
            result += " at " + StartTime.ToShortTimeString();
            result += " for "; // + diff.
            if (diff.Hours > 0) result += diff.Hours + " hh";
            if (diff.Minutes > 0) result += diff.Minutes + " min";
            if (diff.Seconds > 0) result += diff.Seconds + " sec";
            return result; 
        }

        public static char ElementSeparator
        {
            get
            {
                return '^';
            }
        }

        public static char RecordsSeparator
        {
            get
            {
                return '|';
            }
        }
    }
}
