using MySqlConnector;

namespace TannersWebsiteTemplate.SQL
{
    public static class Triggers
    {
        // Creates ban entries for existing accounts
        public static void Accounts()
        {
            using (var con = Main.Connect())
            {
                con.Open();

                string trigger = "CREATE TRIGGER IF NOT EXISTS accountstrigger AFTER INSERT ON accounts FOR EACH ROW BEGIN INSERT INTO accountbans(id, banned, reason) VALUES (NEW.id, false, ''); END;";
                using (var cmd = new MySqlCommand(trigger, con))
                {
                    cmd.ExecuteNonQuery();
                }

                con.Close();
            }
        }
    }
}
