namespace EX.Common
{
    public class AppConfig
    {

        public string Title { get; set; }

        public string AuthUrl { get; set; }
        
        public string TokenUrl { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Realm { get; set; } 

        public string HandbookApi { get; set; }

        public string RedisConnection{ get; set; }

        public string NotificationApi { get; set; }

        public string IdentityApi { get; set; }

        public string ExamApi { get; set; }

        public string CloudEndpoint{ get; set; }

        public string MailboxUrl { get; set; }
    }
}
