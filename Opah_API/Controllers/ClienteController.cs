using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opah_API.Domain.Service.Interface;
using Opah_API.Domain.VO;

namespace Opah_API.Controllers
{
    [ApiController]
    [Route("cliente")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _service;
        public ClienteController(IClienteService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]ClienteFilterVO cliente)
        {
            var clientes = await _service.Get(cliente);
            if (clientes == null || clientes.Count == 0)
            {
                return NotFound();
            }
            return Ok(clientes);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ClienteVO>> Get(int id)
        {
            var item = await _service.Get(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        [HttpPost("")]
        public async Task<IActionResult> Insert([FromBody]ClienteVO cliente)
        {            
            var item = await _service.Insert(cliente);
            if (item.error)
            {
                return BadRequest(item);
            }
            return Ok(item);
        }
        [HttpPut("")]
        public async Task<IActionResult> Update([FromBody]ClienteVO cliente)
        {
            var item = await _service.Update(cliente);
            if (item.error)
            {
                return BadRequest(item);
            }
            return Ok(item);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _service.Delete(id);
            if (item.error)
            {
                return BadRequest(item);
            }
            return Ok(item);
        }
    }
}