// Code written by Gabriel Mailhot, 02/08/2020.

namespace TalesContract
{
   public interface ICampaignTime
   {
      public float CurrentHourInDay { get; set; }

      public int DaysInSeason { get; set; }

      public int DaysInWeek { get; set; }

      public int DaysInYear { get; set; }

      public float ElapsedDaysUntilNow { get; set; }

      public float ElapsedHoursUntilNow { get; set; }

      public float ElapsedMillisecondsUntilNow { get; set; }

      public float ElapsedSeasonsUntilNow { get; set; }

      public float ElapsedSecondsUntilNow { get; set; }

      public float ElapsedWeeksUntilNow { get; set; }

      public float ElapsedYearsUntilNow { get; set; }

      public int GetDayOfSeason { get; set; }

      public float GetDayOfSeasonf { get; set; }

      public int GetDayOfWeek { get; set; }

      public int GetDayOfYear { get; set; }

      public int GetHourOfDay { get; set; }

      public int GetSeasonOfYear { get; set; }

      public float GetSeasonOfYearf { get; set; }

      public int GetWeekOfSeason { get; set; }

      public int GetYear { get; set; }

      public int HoursInDay { get; set; }

      public bool IsDayTime { get; set; }

      public bool IsFuture { get; set; }

      public bool IsNightTime { get; set; }

      public bool IsNow { get; set; }

      public bool IsPast { get; set; }

      public int MinutesInHour { get; set; }

      public float RemainingDaysFromNow { get; set; }

      public float RemainingHoursFromNow { get; set; }

      public float RemainingMillisecondsFromNow { get; set; }

      public float RemainingSeasonsFromNow { get; set; }

      public float RemainingSecondsFromNow { get; set; }

      public float RemainingWeeksFromNow { get; set; }

      public float RemainingYearsFromNow { get; set; }

      public int SeasonsInYear { get; set; }

      public int SunRise { get; set; }

      public int SunSet { get; set; }

      public double ToDays { get; set; }

      public double ToHours { get; set; }

      public double ToMilliseconds { get; set; }

      public double ToMinutes { get; set; }

      public double ToSeasons { get; set; }

      public double ToSeconds { get; set; }

      public double ToWeeks { get; set; }

      public double ToYears { get; set; }

      public int WeeksInSeason { get; set; }
   }
}