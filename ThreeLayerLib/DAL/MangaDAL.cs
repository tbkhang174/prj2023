using System.Net.Quic;
using MySqlConnector;
using Persistence;

namespace DAL
{
    public class MangaDAL
    {
        private string query = "";
        private MySqlConnection connection = DbConfig.GetConnection();

        internal Manga GetManga(MySqlDataReader reader)
        {
            Manga manga = new Manga();
            manga.MangaID = reader.GetInt32("Manga_ID");
            manga.MangaName = reader.GetString("Manga_Name");
            manga.Quantity = reader.GetInt32("quantity");
            manga.Price = reader.GetDecimal("price");
            manga.TypeID = reader.GetInt32("type_id");
            manga.Type = new Persistence.Type();
            return manga;
        }
        internal Persistence.Type GetType(MySqlDataReader reader)
        {
            Persistence.Type type = new Persistence.Type();
            type.TypeID = reader.GetInt32("type_id");
            type.MangaType = reader.GetString("type_name");
            return type;
        }
        public Persistence.Type GetTypeByID(int typeID)
        {
            Persistence.Type type = new Persistence.Type();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                query = "SELECT * FROM Comics_types WHERE type_id = @typeid;";
                command.CommandText = query;
                command.Parameters.AddWithValue("@typeid", typeID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    type = GetType(reader);
                }
                reader.Close();
            }
            catch { }
            return type;
        }
        public Manga GetMangaByID(int MangaID)
        {
            Manga manga = new Manga();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                query = "SELECT * FROM Manga m inner join manga_types mt on m.type_id = mt.type_id WHERE m.manga_ID = @mangaID;";
                command.CommandText = query;
                command.Parameters.AddWithValue("@mangaID", MangaID);
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    manga = GetManga(reader);
                }
                reader.Close();
            }
            catch { }
            manga.Type = MangaType(manga.TypeID);
            return manga;
        }
        public List<Manga> GetAllManga()
        {
            List<Manga> Mangas = new List<Manga>();
            try
            {
                MySqlCommand command = new MySqlCommand("", connection);
                query = "SELECT * FROM Manga;";
                command.CommandText = query;
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Mangas.Add(GetManga(reader));
                }
                reader.Close();
            }
            catch { }
            List<Manga> output = new List<Manga>();
            foreach(var manga in Mangas){
                output.Add(GetMangaByID(manga.MangaID));
            }
            return output;
        }

        internal Persistence.Type GetOrderType(MySqlDataReader reader)
        {
            Persistence.Type type = new Persistence.Type();
            type.orderTypeID = reader.GetString("type_id");
            return type;
        }
        public Persistence.Type MangaType(int typeid){
            Persistence.Type output = new Persistence.Type();
            try{
                query = @"select * from manga_types where type_id = @typeid;";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@typeid", typeid);
                MySqlDataReader reader = command.ExecuteReader();
                if(reader.Read()){
                    output = GetType(reader);
                }
                reader.Close();
            }catch{

            }
            return output;
        }

    }
}
