using TannersWebsiteTemplate.Models;
namespace TannersWebsiteTemplate
{
    public static class Statistics
    {
        public async static void IncrementLogins()
        {
            await Logger.Write("Incrementing logins", "STATS");
            await SQL.Stats.UpdateStat("logins");
        }

        public async static void IncrementRegistrations()
        {
            await Logger.Write("Incrementing registrations", "STATS");
            await SQL.Stats.UpdateStat("registrations");
        }

        public async static void IncrementErrors()
        {
            await Logger.Write("Incrementing error total", "STATS");
            await SQL.Stats.UpdateStat("errors");
        }

        public async static void ResetStats()
        {
            await Logger.Write("Resetting stats", "STATS");
            await SQL.Stats.ResetStats();
        }

        public static async Task<Stats> GetStats()
        {
            Stats stats = await SQL.Stats.GetStats();
            return stats;
        }
    }
}
