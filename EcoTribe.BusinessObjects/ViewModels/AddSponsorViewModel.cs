using System.Collections.Generic;
using System.Web.Mvc;

public class AddSponsorViewModel
{
    public int EventId { get; set; }
    public List<SelectListItem> Organizations { get; set; }
}
