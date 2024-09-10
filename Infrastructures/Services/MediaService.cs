using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Infrastuctures;
using BDRDExce.Models;

namespace BDRDExce.Infrastructures.Services
{
    public class MediaService : BaseDbService<Media>, IMediaService
    {
        public MediaService(AppDbContext context) : base(context)
        {
        }
        
    }
}