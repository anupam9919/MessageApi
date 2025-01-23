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
                    INSERT INTO [CENTRALSTORE_DEMO].[dbo].[TelegramMessage] 
                    (MessageText, UserMob, MsgStatus, MsgDate, DemandNumber) 
                    VALUES 
                    (@MessageText, @UserMob, @MsgStatus, @MsgDate, @DemandNumber);
                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MessageText", message.MessageText);
                    command.Parameters.AddWithValue("@UserMob", message.UserMob);
                    command.Parameters.AddWithValue("@MsgStatus", message.MsgStatus);
                    command.Parameters.AddWithValue("@MsgDate", message.MsgDate);
                    command.Parameters.AddWithValue("@DemandNumber", message.DemandNumber);

                    connection.Open();
                    int newMsgId = Convert.ToInt32(command.ExecuteScalar());
                    return newMsgId;
                }
            }
        }
    }
}
