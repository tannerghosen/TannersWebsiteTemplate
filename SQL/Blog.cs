using MySqlConnector;
using TannersWebsiteTemplate.Models;
using TannersWebsiteTemplate.Interfaces.SQL;

namespace TannersWebsiteTemplate.SQL
{
    public class Blog : IBlog
    {
        // Adds a blog post
        public static async Task AddBlogPost(string title, string message)
        {
            try
            {
                using (var con = Main.Connect())
                {
                    con.Open();
                    string query = "INSERT INTO blog (title, message) VALUES (@title, @message)";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@message", message);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                await Logger.Write("Added blog post with title " + title);
            }
            catch (MySqlException e)
            {
                await Logger.Write("SQL.Blog: An error occured in AddBlogPost: " + e.Message + "\nSQL.Blog: Error Code: " + e.ErrorCode, "ERROR");
            }
        }

        // Updates an existing blog post by blogid.
        public static async Task UpdateBlogPost(int? blogid, string title, string message)
        {
            try
            {
                using (var con = Main.Connect())
                {
                    con.Open();
                    string query = "UPDATE blog SET title = @title, message = @message WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@title", title);
                        cmd.Parameters.AddWithValue("@message", message);
                        cmd.Parameters.AddWithValue("@id", blogid);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                await Logger.Write("Updated blog post " + blogid);
            }
            catch (MySqlException e)
            {
                await Logger.Write("SQL.Blog: An error occured in UpdateBlogPost: " + e.Message + "\nSQL.Blog: Error Code: " + e.ErrorCode, "ERROR");
            }
        }

        // Deletes a blog post by blogid
        public static async Task DeleteBlogPost(int? blogid)
        {
            try
            {
                using (var con = Main.Connect())
                {
                    con.Open();
                    string query = "DELETE FROM blog WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", blogid);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                await Logger.Write("Deleted blog post " + blogid);
            }
            catch (MySqlException e)
            {
                await Logger.Write("SQL.Blog: An error occured in DeleteBlogPost: " + e.Message + "\nSQL.Blog: Error Code: " + e.ErrorCode, "ERROR");
            }
        }

        // Gets a blog post by blogid
        public static async Task<BlogPost> GetBlogPost(int? blogid)
        {
            string? title = null, message = null, date = null;
            try
            {
                using (var con = Main.Connect())
                {
                    con.Open();
                    string query = "SELECT title, message, date FROM blog WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", blogid);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                title = reader.GetString(0);
                                message = reader.GetString(1);
                                date = reader.GetDateTime(2).ToLongDateString();
                            }
                            else
                            {
                                return new BlogPost { Title = null, Message = null, Date = null };
                            }
                        }
                    }
                    return new BlogPost { Title = title , Message = message, Date = date };
                }
            }
            catch (MySqlException e)
            {
                await Logger.Write("SQL.Blog: An error occured in GetBlogPost: " + e.Message + "\nSQL.Blog: Error Code: " + e.ErrorCode, "ERROR");
                return new BlogPost { Title = null, Message = null, Date = null };
            }
        }

        // Get the total amount of blog posts.
        public static async Task<int> GetBlogPostCount()
        {
            int count = 0;
            try
            {
                using (var con = Main.Connect())
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM blog";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    count = reader.GetInt32(0);
                                }
                            }
                        }
                    }
                }
                return count;
            }
            catch (MySqlException e)
            {
                await Logger.Write("SQL.Blog: An error occured in GetBlogPostCount: " + e.Message + "\nSQL.Blog: Error Code: " + e.ErrorCode, "ERROR");
                return 0;
            }
        }

        // Checks if a blog post exist by blogid
        public static async Task<bool> DoesBlogPostExist(int? blogid)
        {
            try
            {
                using (var con = Main.Connect())
                {
                    con.Open();
                    string query = "SELECT * FROM blog WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", blogid);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (MySqlException e)
            {
                await Logger.Write("SQL.Blog: An error occured in DoesBlogPostExist: " + e.Message + "\nSQL.Blog: Error Code: " + e.ErrorCode, "ERROR");
                return false;
            }
        }
    }
}
