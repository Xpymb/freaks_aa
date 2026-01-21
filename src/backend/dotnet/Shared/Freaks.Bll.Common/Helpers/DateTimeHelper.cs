namespace Freaks.Bll.Common.Helpers;

public static class DateTimeHelper
{
    extension(DateOnly date)
    {
        public DateTimeOffset ToDateTimeOffset()
        {
            return new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, TimeSpan.Zero);
        }

        public DateTimeOffset ToDateTimeOffset(TimeSpan timeOffset)
        {
            return new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, timeOffset);
        }
    }
}