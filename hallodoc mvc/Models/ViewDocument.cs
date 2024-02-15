namespace hallodoc_mvc.Models
{
    public class ViewDocument
    {
        public int RequestId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<IFormFile>? File { get; set; }
    }
}
