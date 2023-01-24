namespace SGStask.Models
{
    public class Root
    {
        public DateTime Date { get; set; }
        public DateTime PreviousDate { get; set; }
        public string PreviousURL { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Valute> Valute { get; set; }
    }
}
