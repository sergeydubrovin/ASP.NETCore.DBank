using DBank.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace DBank.Web.Controllers
{
    [ApiController]
    [TypeFilter<ApiExceptionFilter>]
    public class ApiBaseController : ControllerBase
    {
        
    }
}
