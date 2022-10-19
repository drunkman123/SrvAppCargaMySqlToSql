using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using SrvAppCargasSisbol.Data.Repositories;
using System.Net;
using System.Text;


namespace SrvAppCargasSisbol.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ExclusiveActionAttribute : ActionFilterAttribute
    {
        private static int _isExecuting = 0;
        private static int _isDuplicated = 0;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Interlocked.CompareExchange(ref _isExecuting, 1, 0) == 0)
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            Interlocked.Exchange(ref _isDuplicated, 1);
            filterContext.Result = new StatusCodeResult((int)HttpStatusCode.ServiceUnavailable);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            if (_isDuplicated == 1)
            {
                Interlocked.Exchange(ref _isDuplicated, 0);
                return;
            }
            Interlocked.Exchange(ref _isExecuting, 0);
        }
    }

    [ExclusiveAction]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CargasSisbolController : Controller
    {
        private IHostEnvironment _hostingEnvironment;
        private int re;
        private readonly CargasSisbolRepository _cargasSisbolRepo;
        public CargasSisbolController(IHostEnvironment hostingEnvironment, IConfiguration Configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _cargasSisbolRepo = new CargasSisbolRepository(Configuration, hostingEnvironment);
        }
        // GET: CargasSisbolController
        [HttpGet]
        public void Carga()
        {
            while (true)
            {
                try
                {
                    _cargasSisbolRepo.CargaFolha1();
                    new ManualResetEvent(false).WaitOne(TimeSpan.FromMinutes(1));
                    _cargasSisbolRepo.CargaFolha2();
                    new ManualResetEvent(false).WaitOne(TimeSpan.FromMinutes(1));
                    _cargasSisbolRepo.CargaFolha3();
                    new ManualResetEvent(false).WaitOne(TimeSpan.FromMinutes(1));
                    _cargasSisbolRepo.CargaFolha4();
                    _cargasSisbolRepo.SaveUltimaAtu();
                    new ManualResetEvent(false).WaitOne(TimeSpan.FromMinutes(17));
                }
                catch (Exception ex)
                {
                    _cargasSisbolRepo.GetErro(ex.ToString());
                    new ManualResetEvent(false).WaitOne(TimeSpan.FromMinutes(5));

                }
            }
            
        }
    }
}
