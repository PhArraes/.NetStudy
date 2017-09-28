using HanoiService.Core;
using HanoiService.Core.Interfaces;
using HanoiService.Core.Interfaces.Services;
using HanoiService.Data;
using HanoiService.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HanoiService.Web.Controllers
{
    public class HanoiController : ApiController
    {
        private readonly IHanoiManager _manager;
        private readonly IHanoiHistoryService _historyService;

        public HanoiController(IHanoiManager manager, IHanoiHistoryService histService)
        {
            _manager = manager;
            _historyService = histService;
        }

        // GET api/values/5
        public IHttpActionResult Get(int id)
        {
            try
            {
                int newID = -1;
                if (id < 1)
                {
                    return BadRequest("O número de discos deve ser maior que 0");
                }

                if (!_manager.TryStartHanoiThread(id, out newID))
                {
                    return StatusCode(HttpStatusCode.ServiceUnavailable);
                    //return InternalServerError(new Exception("Serviço sobrecarregado, tente novamente mais tarde."));
                }
                return Ok(newID);
            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }

        // GET api/values/5
        [Route("api/hanoi/stateText/{id}")]
        public IHttpActionResult GetState(int id)
        {
            List<List<int>> state = null;
            try
            {
                if (_manager.TryGetCurrentState(id, out state))
                {
                    return Ok(state);
                }
                return BadRequest("Id não encontrado ou não foi possível completar a sua execução.");
            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }

        [Route("api/hanoi/state/{id}")]
        public HttpResponseMessage GetImageState(int id)
        {
            List<List<int>> state = null;
            HttpResponseMessage r = null;
            try
            {
                if (_manager.TryGetCurrentState(id, out state))
                {
                    HanoiImageViewModel imageVM = new HanoiImageViewModel(state);
                    var ms = imageVM.ToMemoryStream();
                    r = Request.CreateResponse();
                    r.Content = new StreamContent(ms);
                    r.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                    r.Content.Headers.ContentLength = ms.Length;
                    return r;
                }
                r = Request.CreateResponse();
                r.StatusCode = HttpStatusCode.BadRequest;
                r.ReasonPhrase = "Id não encontrado ou não foi possível completar a sua execução.";
                return r;
            }
            catch (Exception)
            {
                r = Request.CreateResponse();
                r.StatusCode = HttpStatusCode.InternalServerError;
                return r;
            }
        }

        // GET api/values/5
        [Route("api/hanoi/history")]
        public IHttpActionResult GetHistory([FromUri]PageViewModel page)
        {
            try
            {
                if (page != null)
                {
                    if (page.Index < 0 || page.Size < 1)
                    {
                        return BadRequest("O índice da página deve ser maior ou igual a 0 e o tamanho maior que 0.");
                    }
                }
                if (page == null)
                    page = new PageViewModel();
                return Ok(_historyService.GetHistoryPaged(page.Index, page.Size));

            }
            catch (Exception)
            {
                return InternalServerError();
            }

        }
    }
}