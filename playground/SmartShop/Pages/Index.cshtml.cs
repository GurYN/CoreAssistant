using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using CoreAssistant;

namespace SmartShop.Pages;

public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;

    public IndexModel(IConfiguration configuration)
    {
       this._configuration = configuration;
    }

    public void OnGet()
    {
        ViewData["DefaultContext"] = _configuration["CoreAssistantOptions:DefaultContext"];
    }
}
