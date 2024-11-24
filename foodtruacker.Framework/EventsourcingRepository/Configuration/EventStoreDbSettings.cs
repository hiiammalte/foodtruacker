namespace foodtruacker.EventSourcingRepository.Configuration
{
    public class EventStoreDbSettings
    {
        public string Schema { get; set; }
        public string Url { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Uri { get; set; }
        public bool Tls { get; set; }
        public bool TlsVerifyCert { get; set; }
    }
}
