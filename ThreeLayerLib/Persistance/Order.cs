namespace Persistence;

public class Order
{
    public int OrderID { get; set; }
    public int StaffID { get;set; }
    public string Status {get;set; }
    public int order_status { get; set; }
    public int Order_Quantity { get; set; }
    public int Order_Create_at {get; set; }
    public string Order_Create_by{get; set; }
    public Staff CreateBy { get; set; } = default!;
    public List<Manga> Manga { get; set; }
    public string Order_PaymentMethod { get; set; }
    //public Staff CreateBy { get; set; }
    public DateTime CreateAt { get; set; }
    public int OrderTypeID { get; set; } 

    public Type Typea { get; set; }
    public Type Type{ get; set;}
}
