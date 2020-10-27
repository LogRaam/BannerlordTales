// Code written by Gabriel Mailhot, 14/09/2020.

#region

using TalesContract;
using TaleWorlds.CampaignSystem;

#endregion

namespace TalesBase.TW
{
    public class BaseCampaignTime : ICampaignTime
    {
        public BaseCampaignTime(CampaignTime time)
        {
            if (time == null) return;

            CurrentHourInDay = time.CurrentHourInDay;
            ElapsedDaysUntilNow = time.ElapsedDaysUntilNow;
            ElapsedHoursUntilNow = time.ElapsedHoursUntilNow;
            ElapsedMillisecondsUntilNow = time.ElapsedMillisecondsUntilNow;
            ElapsedSeasonsUntilNow = time.ElapsedSeasonsUntilNow;
            ElapsedSecondsUntilNow = time.ElapsedSecondsUntilNow;
            ElapsedWeeksUntilNow = time.ElapsedWeeksUntilNow;
            ElapsedYearsUntilNow = time.ElapsedYearsUntilNow;
            GetDayOfSeason = time.GetDayOfSeason;
            GetDayOfSeasonf = time.GetDayOfSeasonf;
            GetDayOfWeek = time.GetDayOfWeek;
            GetDayOfYear = time.GetDayOfYear;
            GetHourOfDay = time.GetHourOfDay;
            GetSeasonOfYear = time.GetSeasonOfYear;
            GetSeasonOfYearf = time.GetSeasonOfYearf;
            GetWeekOfSeason = time.GetWeekOfSeason;
            GetYear = time.GetYear;
            IsDayTime = time.IsDayTime;
            IsFuture = time.IsFuture;
            IsNightTime = time.IsNightTime;
            IsNow = time.IsNow;
            IsPast = time.IsPast;
            RemainingDaysFromNow = time.RemainingDaysFromNow;
            RemainingHoursFromNow = time.RemainingHoursFromNow;
            RemainingMillisecondsFromNow = time.RemainingMillisecondsFromNow;
            RemainingSeasonsFromNow = time.RemainingSeasonsFromNow;
            RemainingSecondsFromNow = time.RemainingSecondsFromNow;
            RemainingWeeksFromNow = time.RemainingWeeksFromNow;
            RemainingYearsFromNow = time.RemainingYearsFromNow;
            ToDays = time.ToDays;
            ToHours = time.ToHours;
            ToMilliseconds = time.ToMilliseconds;
            ToMinutes = time.ToMinutes;
            ToSeasons = time.ToSeasons;
            ToSeconds = time.ToSeconds;
            ToWeeks = time.ToWeeks;
            ToYears = time.ToYears;
        }

        public BaseCampaignTime()
        {
        }

        public float CurrentHourInDay { get; set; }

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

        public bool IsDayTime { get; set; }
        public bool IsFuture { get; set; }
        public bool IsNightTime { get; set; }
        public bool IsNow { get; set; }
        public bool IsPast { get; set; }

        public float RemainingDaysFromNow { get; set; }
        public float RemainingHoursFromNow { get; set; }
        public float RemainingMillisecondsFromNow { get; set; }
        public float RemainingSeasonsFromNow { get; set; }
        public float RemainingSecondsFromNow { get; set; }
        public float RemainingWeeksFromNow { get; set; }
        public float RemainingYearsFromNow { get; set; }

        public double ToDays { get; set; }
        public double ToHours { get; set; }
        public double ToMilliseconds { get; set; }
        public double ToMinutes { get; set; }
        public double ToSeasons { get; set; }
        public double ToSeconds { get; set; }
        public double ToWeeks { get; set; }
        public double ToYears { get; set; }
    }
}