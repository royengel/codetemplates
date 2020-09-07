using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TailorTools.Props;

namespace TailorTools.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class IndexModel : PageModel
    {
        private readonly IConverter _converter;

        public IndexModel(IConverter converter)
        {
            _converter = converter;
        }

        public void OnGet()
        {

        }

        public JsonResult OnPostClass(string script)
        {
            return new JsonResult(_converter.ClassFromScript(script));
        }
    }
}
