using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Ecommerce.WebApp.FrontEnd.Server.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private IMediator _mediator;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected readonly string _webRootPath;

        [Obsolete]
        public BaseApiController(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _env = hostingEnvironment;
            _webRootPath = hostingEnvironment.WebRootPath;
        }
    }
}
