using MySqlConnector;

namespace TannersWebsiteTemplate.SQL
{
    public class Automation
    {
        public static async Task UnbanAccounts()
        {
            try
            {
                using (var con = Main.Connect())
                {
                    con.Open();
                    string e = "UPDATE accountbans SET expire = NOW(), banned = 0 WHERE expire <= NOW()";
                    using (var cmd = new MySqlCommand(e, con))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                    con.Close();
                }
            }
            catch (MySqlException e)
            {
                await Logger.Write("SQL.Automation: An error occured in UnbanAccounts: " + e.Message + "\nSQL.Automation: Error Code: " + e.ErrorCode, "ERROR");
            }
        }
        public static async Task UnbanIPs()
        {
            try
            {
                using (var con = Main.Connect())
                {
                    con.Open();
                    string e = "DELETE FROM bans WHERE expire <= NOW()";
                    using (var cmd = new MySqlCommand(e, con))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                    con.Close();
                }
            }
            catch (MySqlException e)
            {
                await Logger.Write("SQL.Automation: An error occured in UnbanIPs: " + e.Message + "\nSQL.Automation: Error Code: " + e.ErrorCode, "ERROR");
            }
        }
    }
}
