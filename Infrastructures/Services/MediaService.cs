using BDRDExce.Infrastructures.Services.Interface;
using BDRDExce.Infrastuctures;
using BDRDExce.Models;

namespace BDRDExce.Infrastructures.Services
{
    public class MediaService(AppDbContext context) : BaseDbService<Media>(context), IMediaService;
}