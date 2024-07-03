using Microsoft.AspNetCore.Mvc.RazorPages; // To use PageModel;
using Microsoft.AspNetCore.Mvc; // To us [BindProperty], IActionResult;
using Northwind.Context; // To use NorthwindDataContext;
using Northwind.EntityModels; // To use NorthwindDataContext;

namespace Northwind.Web.Pages;

public class SuppliersModel : PageModel
{
    private NorthwindDataContext _db;
    public IEnumerable<Supplier>? Suppliers { get; set; }

    [BindProperty]
    public Supplier? Supplier { get; set; }

    public SuppliersModel(NorthwindDataContext db)
    {
        _db = db;
    }

    public void OnGet()
    {
        ViewData["Title"] = "Northwind B2B - Suppliers";
        //Suppliers = _db.Suppliers
        //    .OrderBy(c => c.Country)
        //    .ThenBy(s => s.CompanyName);

        Suppliers = from s in _db.Suppliers
                    orderby s.Country, s.CompanyName
                    select s;
    }

    public IActionResult OnPost()
    {
        if (Supplier is not null && ModelState.IsValid)
        {
            _db.Suppliers.Add(Supplier);
            _db.SaveChanges();
            return RedirectToPage("/suppliers");
        }
        return Page();
    }
}


