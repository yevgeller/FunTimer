using System;

namespace FunTimer.Lib.Models
{
    public class TimeRecord
    {
        public int Id { get; set; }
        public TimeRecordTypeEnum TimeRecordType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string ConvertToString()
        {
            return string.Format("{0}^{1}^{2}", TimeRecordType, StartTime, EndTime);
        }
    }
}
