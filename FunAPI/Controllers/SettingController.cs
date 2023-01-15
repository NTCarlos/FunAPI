using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Services;
using Services.DTO;

namespace FunApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _defaultService;
        public SettingController(ISettingService defaultService)
        {
            _defaultService = defaultService ?? throw new ArgumentNullException(nameof(defaultService));
        }

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
            var result = await _defaultService.Add(setting);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] SettingDto setting, int id)
        {
            var result = await _defaultService.Update(setting, id);
            return Ok(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _defaultService.Delete(id);
            return Ok(result);
        }
    }
}
