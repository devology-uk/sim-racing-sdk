namespace SimRacingSdk.Core
{
    public static class Extensions
    {
        public static string ToGapString(this int timingMs)
        {
            var timeSpan = TimeSpan.FromMilliseconds(timingMs);

            return timingMs switch
            {
                >= Constants.MillisecondsPerHour => timeSpan.ToString(Constants.HoursGapFormat),
                >= Constants.MilliSecondsPerMinute => timeSpan.ToString(Constants.MinutesGapFormat),
                _ => timeSpan.ToString(Constants.SecondsGapFormat)
            };
        }

        public static string ToGapString(this long timingMs)
        {
            var timeSpan = TimeSpan.FromMilliseconds(timingMs);

            return timingMs switch
            {
                >= Constants.MillisecondsPerHour => timeSpan.ToString(Constants.HoursGapFormat),
                >= Constants.MilliSecondsPerMinute => timeSpan.ToString(Constants.MinutesGapFormat),
                _ => timeSpan.ToString(Constants.SecondsGapFormat)
            };
        }

        public static string ToGapString(this float timingMs)
        {
            var timeSpan = TimeSpan.FromMilliseconds(timingMs);

            return timingMs switch
            {
                >= Constants.MillisecondsPerHour => timeSpan.ToString(Constants.HoursGapFormat),
                >= Constants.MilliSecondsPerMinute => timeSpan.ToString(Constants.MinutesGapFormat),
                _ => timeSpan.ToString(Constants.SecondsGapFormat)
            };
        }

        public static double ToMilliseconds(this double seconds)
        {
            return TimeSpan.FromSeconds(seconds)
                           .TotalMilliseconds;
        }

        public static string ToTimeString(this double timeMs)
        {
            return TimeSpan.FromMilliseconds(timeMs)
                           .ToString(Constants.TimeFormat);
        }

        public static string ToTimeString(this int timeMs)
        {
            return TimeSpan.FromMilliseconds(timeMs)
                           .ToString(Constants.TimeFormat);
        }

        public static string ToTimeString(this long timeMs)
        {
            return TimeSpan.FromMilliseconds(timeMs)
                           .ToString(Constants.TimeFormat);
        }

        public static string ToTimingStringFromMilliseconds(this double timingMs)
        {
            return TimeSpan.FromMilliseconds(timingMs)
                           .ToString(Constants.TimingFormat);
        }

        public static string ToTimingStringFromMilliseconds(this float timingMs)
        {
            return TimeSpan.FromMilliseconds(timingMs)
                           .ToString(Constants.TimingFormat);
        }

        public static string ToTimingStringFromMilliseconds(this int timingMs)
        {
            return TimeSpan.FromMilliseconds(timingMs)
                           .ToString(Constants.TimingFormat);
        }

        public static string ToTimingStringFromMilliseconds(this long timingMs)
        {
            return TimeSpan.FromMilliseconds(timingMs)
                           .ToString(Constants.TimingFormat);
        }

        public static string ToTimingStringFromSeconds(this double timingSeconds)
        {
            return TimeSpan.FromSeconds(timingSeconds)
                           .ToString(Constants.TimingFormat);
        }

        public static string ToTimingStringFromSeconds(this float timingSeconds)
        {
            return TimeSpan.FromSeconds(timingSeconds)
                           .ToString(Constants.TimingFormat);
        }

        public static string ToTimingStringFromSeconds(this int timingSeconds)
        {
            return TimeSpan.FromSeconds(timingSeconds)
                           .ToString(Constants.TimingFormat);
        }

        public static string ToTimingStringFromSeconds(this long timingSeconds)
        {
            return TimeSpan.FromSeconds(timingSeconds)
                           .ToString(Constants.TimingFormat);
        }

        public static double ValidatedValue(this double doubleValue)
        {
            return doubleValue is >= double.MaxValue or <= double.MinValue? 0: doubleValue;
        }

        public static long ValidatedValue(this long longValue)
        {
            return longValue is >= int.MaxValue or <= int.MinValue? 0: longValue;
        }

        public static int ValidatedValue(this int intValue)
        {
            return intValue is >= int.MaxValue or <= int.MinValue? 0: intValue;
        }
    }
}