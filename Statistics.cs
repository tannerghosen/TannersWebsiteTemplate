using TannersWebsiteTemplate.Models;
namespace TannersWebsiteTemplate
{
    public static class Statistics
    {
        public async static void IncrementLogins()
        {
            Logger.Write("Incrementing logins", "STATS");
            await SQL.Stats.UpdateStat("logins");
        }

        public async static void IncrementRegistrations()
        {
            Logger.Write("Incrementing registrations", "STATS");
            await SQL.Stats.UpdateStat("registrations");
        }

        public async static void IncrementErrors()
        {
            Logger.Write("Incrementing error total", "STATS");
            await SQL.Stats.UpdateStat("errors");
        }

        public async static void ResetStats()
        {
            Logger.Write("Resetting stats", "STATS");
            await SQL.Stats.ResetStats();
        }

        public static Stats GetStats()
        {
            return SQL.Stats.GetStats();
        }
    }
}
