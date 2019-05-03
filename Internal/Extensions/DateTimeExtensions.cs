using System;

namespace CourtCase.Internal.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsBetween(this DateTime date, DateTime startDate, DateTime endDate)
        {
            var result = true;
            result = result && date >= startDate;
            result = result && date <= endDate;
            return result;
        }
    }
}