using MySql.Data.MySqlClient;
using TannersWebsiteTemplate.Models;
using System.Globalization;
using System.Reflection.PortableExecutable;

namespace TannersWebsiteTemplate.SQL
{
    public static class Comments
    {
        // Adds a comment to a specified comment section
        public static async Task AddComment(string? comment, string username = "Anonymous", int commentsection = 0)
        {
            int userid, anonymousid = -1;
            if (comment == null)
            {
                comment = "";
            }
            if (!Accounts.DoesUserExist(username) || (username == "" || username == null || username == "Anonymous"))
            {
                userid = anonymousid;
            }
            else
            {
                userid = Accounts.GetUserID(username);
            }
            try
            {
                using (var con = Main.Connect())
                {
                    con.Open();
                    // "INSERT INTO comments (userid, commentsid, comment, date) VALUES (@userid, @commentsection, @comment, DATETIME('now', 'utc', '-8 hours'))";
                    string q = "INSERT INTO comments (userid, commentsid, comment, date) VALUES (@userid, @commentsection, @comment, NOW())";
                    using (var cmd = new MySqlCommand(q, con))
                    {
                        cmd.Parameters.AddWithValue("@userid", userid);
                        cmd.Parameters.AddWithValue("@comment", comment);
                        cmd.Parameters.AddWithValue("@commentsection", commentsection);
                        await cmd.ExecuteNonQueryAsync();
                    }
                    Logger.Write("Comment added by " + username + " to comment section id " + commentsection);
                }
            }
            catch (MySqlException e)
            {
                Logger.Write("SQL.Comments: An error occured in AddComment: " + e.Message + "\nSQL.Comments: Error Code: " + e.ErrorCode, "ERROR");
            }
        }

        // Gets the comments for a specified section and puts them into a CommentSection model
        public static CommentSection GetCommentSection(int section = 0)
        {
            CommentSection cs = new CommentSection();
            try
            {
                using (var con = Main.Connect())
                {
                    con.Open();
                    // SELECT account id, account username, comments comment, comments date, comments id FROM comments JOIN accounts on comments userid = accounts id WHERE commentsid = section ORDER BY comments date DESC
                    string query = @"SELECT a.id, a.username, c.comment, c.date, c.id FROM comments c JOIN accounts a ON a.id = c.userid WHERE c.commentsid = @section ORDER BY c.date DESC";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@section", section);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    //Comment c = new Comment { UserId = reader.GetInt32(0), Username = reader.GetString(1), Content = reader.GetString(2), Date = Convert.ToString(reader.GetDateTime(3)), CommentSId = reader.GetInt32(4) };
                                    cs.Comments.Add(new Comment { UserId = reader.GetInt32(0), Username = reader.GetString(1), Content = reader.GetString(2), Date = Convert.ToString(reader.GetDateTime(3)), CommentSId = reader.GetInt32(4) });
                                }
                            }
                        }
                    }
                    return cs;
                }
            }
            catch (MySqlException e)
            {
                Logger.Write("SQL.Comments: An error occured in GetCommentSection: " + e.Message + "\nSQL.Comments: Error Code: " + e.ErrorCode, "ERROR");
                return null;
            }
        }

        // Deletes comment by commentid
        public static async Task DeleteComment(int? commentid)
        {
            try
            {
                using (var con = Main.Connect())
                {
                    con.Open();
                    string query = "DELETE FROM comments WHERE id = @id";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@id", commentid);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
                Logger.Write("Deleted comment with id " + commentid);
            }
            catch (MySqlException e)
            {
                Logger.Write("SQL.Comments: An error occured in DeleteComment: " + e.Message + "\nSQL.Comments: Error Code: " + e.ErrorCode, "ERROR");
            }
        }

        public static int CountCommentsByUserId(int userid)
        {
            try
            {
                using (var con = Main.Connect())
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM comments WHERE userid = @userid";
                    using (var cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@userid", userid);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    return reader.GetInt32(0);
                                }
                            }
                        }
                        return 0;
                    }
                }
            }
            catch (MySqlException e)
            {
                Logger.Write("SQL.Comments: An error occured in CountCommentsByUserId: " + e.Message + "\nSQL.Comments: Error Code: " + e.ErrorCode, "ERROR");
                return 0;
            }
        }
    }
}
