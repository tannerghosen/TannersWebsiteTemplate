using TannersWebsiteTemplate.Models;
namespace TannersWebsiteTemplate
{
    public static class Statistics
    {
        public async static Task IncrementLogins()
        {
            await Logger.Write("Incrementing logins", "STATS");
            await SQL.Stats.UpdateStat("logins");
        }

        public async static Task IncrementRegistrations()
        {
            await Logger.Write("Incrementing registrations", "STATS");
            await SQL.Stats.UpdateStat("registrations");
        }

        public async static Task IncrementErrors()
        {
            await Logger.Write("Incrementing error total", "STATS");
            await SQL.Stats.UpdateStat("errors");
        }

        public async static Task ResetStats()
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
