namespace TannersWebsiteTemplate.Interfaces.SQL
{
    public interface ISecurityQuestions
    {
        public static abstract Task<(string, string)> GetSecurityQuestion(int? userid);
        public static abstract Task<(bool, bool)> CreateSecurityQuestion(int? userid, string? question, string? answer)
        public static abstract Task<(bool, bool)> UpdateSecurityQuestion(int? userid, string? question = null, string? answer = null);
    }
}
