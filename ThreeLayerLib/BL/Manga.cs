using System.Collections.Generic;
using Persistence;
using DAL;

namespace BL
{
    public class MangaBL
    {
        private MangaDAL cDAL = new MangaDAL();
        public List<Manga> GetAllManga() {
            return cDAL.GetAllManga();
        }
        public Manga GetMangaByID(int MangaID) {
            return cDAL.GetMangaByID(MangaID);
        }
    }
} 
