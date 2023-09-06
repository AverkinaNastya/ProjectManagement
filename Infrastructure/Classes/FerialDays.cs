using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Infrastructure.Classes
{
    internal class FerialDays
    {
        private static readonly DateOnly?[] holidays = { new DateOnly(2000,2,23),
                                                        new DateOnly(2000,1,1),
                                                        new DateOnly(2000,1,2),
                                                        new DateOnly(2000,1,3),
                                                        new DateOnly(2000,1,4),
                                                        new DateOnly(2000,1,5),
                                                        new DateOnly(2000,1,6),
                                                        new DateOnly(2000,1,7),
                                                        new DateOnly(2000,1,8),
                                                        new DateOnly(2000,3,3),
                                                        new DateOnly(2000,5,1),
                                                        new DateOnly(2000,5,9),
                                                        new DateOnly(2000,6,12),
                                                        new DateOnly(2000,11,4)};
        public static DateOnly AddDay(DateOnly date, int days)
        {
            for (int i = 0; i < days; i++)
            {
                date = date.AddDays(1);

                while (holidays.FirstOrDefault(e => e.Value.Day == date.Day && e.Value.Month == date.Month) != null ||
                        date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                        date = date.AddDays(1);
            }
            return date;
        }
    }
}
