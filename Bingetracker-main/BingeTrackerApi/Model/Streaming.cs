namespace BingeTrackerApi.Model
{
    public class Streaming
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Platform { get; set; }
        public string Genre { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public int Rating { get; set; }
    }
}
