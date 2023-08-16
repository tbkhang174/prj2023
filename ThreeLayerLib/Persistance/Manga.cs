namespace Persistence;
public class Manga
{
    public int MangaID { get; set; }
    public string MangaName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int TypeID { get; set; }
    public Type Type { get; set; }
}
