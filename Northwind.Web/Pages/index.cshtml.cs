namespace Northwind.Web.Pages;
public class Index
{
    public string DayName { get; set; }
    public void OnGet()
    {
        DayName = DateTime.Now.ToString("dddd");
    }
}
