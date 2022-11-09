namespace MusicBrainz.SqlServer.Migrations
{
    public class ConnectionSetting
    {
        public string ConnectionString { get; set; }
        public int ConnectRetryCount { get; set; } = 10;
        public int ConnectRetryInterval { get; set; } = 2;
    }
}
