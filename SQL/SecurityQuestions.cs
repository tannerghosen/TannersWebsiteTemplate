using MySqlConnector;

namespace TannersWebsiteTemplate.SQL
{
    public class SecurityQuestions
    {
        // Gets Security Question by UserID
        public static async Task<(string, string)> GetSecurityQuestion(int? userid)
        {
            try
            {
                using (var con = Main.Connect())
                {
                    con.Open();
                    string query = "SELECT question, answer FROM securityquestion WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", userid);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                string question = reader.GetString(0);
                                string answer = reader.GetString(1);
                                return ( question, answer );
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                await Logger.Write("SQL.Accounts: An error occured in GetSecurityQuestion: " + e.Message + "\nSQL.Accounts: Error Code: " + e.ErrorCode, "ERROR");
                return (null, null);
            }
            return (null, null);
        }

        // Creates a Security Question entry under a specified UserID.
        public static async Task<(bool, bool)> CreateSecurityQuestion(int? userid, string? question, string? answer)
        {
            try
            {
                using (var con = Main.Connect())
                {
                    con.Open();
                    string prequery = "SELECT COUNT(*) FROM securityquestion WHERE id = @id";
                    using (var cmd = new MySqlCommand(prequery, con))
                    {
                        cmd.Parameters.AddWithValue("@id", userid);
                        int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                        if (count > 0)
                        {
                            return (false, false);
                        }
                    }
                    string query = "INSERT INTO securityquestion (id, question, answer) VALUES (@id, @q, @a)";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", userid);
                        cmd.Parameters.AddWithValue("@q", question);
                        cmd.Parameters.AddWithValue("@a", answer);
                        await cmd.ExecuteNonQueryAsync();
                        return (true, false);
                    }
                }
            }
            catch (MySqlException e)
            {
                Logger.Write("SQL.Accounts: An error occured in CreateSecurityQuestion: " + e.Message + "\nSQL.Accounts: Error Code: " + e.ErrorCode, "ERROR");
                return (false, true);
            }
        }

        // Updates a Security Question entry under a specified UserID.
        public static async Task<(bool, bool)> UpdateSecurityQuestion(int? userid, string? question = null, string? answer = null)
        {
            try
            {
                using (var con = Main.Connect())
                {
                    con.Open();
                    string prequery = "SELECT COUNT(*) FROM securityquestion WHERE id = @id";
                    using (var cmd = new MySqlCommand(prequery, con))
                    {
                        cmd.Parameters.AddWithValue("@id", userid);
                        int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                        if (count == 0)
                        {
                            return (false, false);
                        }
                    }
                    (string? q, string? a) = await GetSecurityQuestion(userid); 
                    question = question == null ? q : question;
                    answer = answer == null ? a : answer;
                    string query = "UPDATE securityquestion SET question = @q, answer = @a WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", userid);
                        cmd.Parameters.AddWithValue("@q", question);
                        cmd.Parameters.AddWithValue("@a", answer);
                        await cmd.ExecuteNonQueryAsync();
                        return (true, false);
                    }
                }
            }
            catch (MySqlException e)
            {
                Logger.Write("SQL.Accounts: An error occured in UpdateSecurityQuestion: " + e.Message + "\nSQL.Accounts: Error Code: " + e.ErrorCode, "ERROR");
                return (false, true);
            }
        }
    }
}
