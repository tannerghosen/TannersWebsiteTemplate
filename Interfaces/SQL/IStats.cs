namespace TannersWebsiteTemplate.Interfaces.SQL
{
    public interface IStats
    {
        public static abstract Task UpdateStat(string stat);
        public static abstract Task ResetStats();
        public static abstract Task<Models.Stats> GetStats();
    }
}
