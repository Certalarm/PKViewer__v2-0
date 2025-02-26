using System;

namespace PKInfo.Domain.Entity
{
    public sealed class TimeInterval: IEquatable<TimeInterval>, IComparable<TimeInterval>, IComparable
    {
        private readonly DateTime? _startTime;
        private readonly DateTime? _endTime;
        private (int, int, int, int, int)? _interval;

        private (int, int, int, int, int)? _Interval => _interval ??= IsEmpty
            ? null
            : TimeIntervalHelper.CalcTimeInterval((DateTime)_startTime, (DateTime)_endTime); 

        public int? Years => _Interval?.Item1;
        public int? Months => _Interval?.Item2;
        public int? Days => _Interval?.Item3;
        public int? Hours => _Interval?.Item4;
        public int? Minutes => _Interval?.Item5;

        public bool IsEmpty => _startTime is null || _endTime is null;

        #region .ctors
        private TimeInterval() { }

        internal TimeInterval(int years, int months, int days, int hours, int minutes)
        {
            _interval = (years, months, days, hours, minutes);
        }

        internal TimeInterval(DateTime? startTime, DateTime? endTime)
        {
            _startTime = startTime;
            _endTime = endTime;
        }
        #endregion

        public static TimeInterval Empty() => new ();

        public override bool Equals(object obj) =>
            this.Equals(obj as TimeInterval);

        public bool Equals(TimeInterval other)
        {
            if (other is null)
                return false;
            return Years == other.Years
                && Months == other.Months
                && Days == other.Days
                && Hours == other.Hours
                && Minutes == other.Minutes;
        }

        public override int GetHashCode() => base.GetHashCode();

        public int CompareTo(object obj) =>
            obj is null
                ? 1
                : CompareTo(obj as TimeInterval);

        public int CompareTo(TimeInterval other)
        {
            if (other == null)
                return 1;

            var minutesThis = this.ToMinutes();
            var minutesOther = other.ToMinutes();

            return minutesThis.CompareTo(minutesOther);
        }

#if DEBUG
        public override string ToString() =>
            $"{nameof(Years)}={Years}, {nameof(Months)}={Months}, {nameof(Days)}={Days}, "
           +$"{nameof(Hours)}={Hours}, {nameof(Minutes)}={Minutes}";
#endif

        private long ToMinutes() =>
            IsEmpty || IsNullAnyProperty()
                ? -1
                : DaysToMinutes((int)Years * 365)
                    + DaysToMinutes((int)Months * 30)
                    + DaysToMinutes((int)Days)
                    + (int)Hours * 60
                    + (int)Minutes;

        private bool IsNullAnyProperty() =>
            Years == null || Months == null || Days == null || Hours == null || Minutes == null;

        private static long DaysToMinutes(int days) =>
            days * 24 * 60;
    }
}
