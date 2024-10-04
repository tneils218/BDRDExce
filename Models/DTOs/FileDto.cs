namespace BDRDExce.Models.DTOs
{
    public class FileDto
    {
        public string FileName { get; set; }
        public string FileUrl { get; set; }

        public FileDto()
        {
            
        }
        public FileDto(string fileName, string fileUrl)
        {
            FileName = fileName;
            FileUrl = fileUrl; 
        }
    }
}