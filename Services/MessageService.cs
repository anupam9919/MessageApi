using MessageApi.Models;
using Microsoft.Data.SqlClient;

namespace MessageApi.Services
{
    public class MessageService
    {
        private readonly string _connectionString;

        public MessageService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public int InsertMessage(Message message)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    INSERT INTO WhatsappMessage 
                    (MessageText, UserMob, DemandNumber,Project,Location) 
                    VALUES 
                    (@MessageText, @UserMob,@DemandNumber, @Project, @Location );
                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MessageText", message.MessageText);
                    command.Parameters.AddWithValue("@UserMob", message.UserMob);
                    command.Parameters.AddWithValue("@Project", message.Project);
                    command.Parameters.AddWithValue("@Location", message.Location);
                    command.Parameters.AddWithValue("@DemandNumber", message.DemandNumber);

                    connection.Open();
                    int newMsgId = Convert.ToInt32(command.ExecuteScalar());
                    return newMsgId;
                }
            }
        }
        public List<MessageResponse> GetMessages(string userMob, string project, string location)
        {
            List<MessageResponse> messages = new List<MessageResponse>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT msgid, MessageText, MsgDate, MsgStatus, DemandNumber
            FROM WhatsappMessage 
            WHERE UserMob = @UserMob 
            AND Project = @Project 
            AND Location = @Location  order by msgid desc";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserMob", userMob);
                    command.Parameters.AddWithValue("@Project", project);
                    command.Parameters.AddWithValue("@Location", location);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            messages.Add(new MessageResponse
                            {
                                MessageText = reader["MessageText"].ToString(),
                                MsgDate = reader["MsgDate"] != DBNull.Value ? DateTime.Parse(reader["MsgDate"].ToString()) : null,
                                MsgStatus = reader["MsgStatus"] != DBNull.Value ? reader["MsgStatus"].ToString():null
                            });
                        }
                    }
                }
            }

            return messages;
        }
    }
}
