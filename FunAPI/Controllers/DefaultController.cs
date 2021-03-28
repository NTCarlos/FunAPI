using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Services;
using Services.DTO;

namespace FunApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DefaultController : ControllerBase
    {
        private readonly IDefaultService _defaultService;
        public DefaultController(IDefaultService defaultService)
        {
            _defaultService = defaultService ?? throw new ArgumentNullException(nameof(defaultService));
        }   
        // The Web API will only accept tokens 1) for users, and 2) having the "access_as_user" scope for this API
        //static readonly string[] scopeRequiredByApi = new string[] { "access_as_user" };

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _defaultService.GetAll());
        }
        [HttpGet("Get")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _defaultService.Get(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SettingDto setting)
        {
            await _defaultService.Add(setting);
            return Created("GetAll", true);
        }
    }
}
