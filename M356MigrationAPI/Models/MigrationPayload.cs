namespace M356MigrationAPI.Models
{
    public class MigrationPayload
    {
        public string sourceSiteId { get; set; }
        public string targetSiteId { get; set; }
        public List<ListGuid> listGuids { get; set; }
    }

    public class ListGuid
    {
        public string url { get; set; }
        public string name { get; set; }
    }
}
