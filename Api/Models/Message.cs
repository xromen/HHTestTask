using System.Data;

namespace Api.Models
{
    public class Message
    {
        public long Id { get; set; }
        public string Text { get; set; }
        public long SerialNumber { get; set; }
        public DateTime CreatedAt { get; set; }

        public static Message FromDataRow(DataRow dr)
        {
            return new Message()
            {
                Id = Convert.ToInt64(dr["id"]),
                Text = dr["text"].ToString(),
                SerialNumber = Convert.ToInt64(dr["serial_number"]),
                CreatedAt = Convert.ToDateTime(dr["created_at"])
            };
        }
    }
}
