namespace VezetaApi.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] ImageData { get; set; }

        //public Doctor Doctor { get; set; }
        public ApplicationUser user { get; set; }
    }
}
