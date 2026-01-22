namespace Freaks.Common.Extensions;

public static class DateTimeExtensions
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