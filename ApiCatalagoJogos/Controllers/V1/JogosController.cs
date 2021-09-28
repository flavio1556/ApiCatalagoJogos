using ApiCatalagoJogos.Exception;
using ApiCatalagoJogos.InputModel;
using ApiCatalagoJogos.Services;
using ApiCatalagoJogos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApiCatalagoJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _service;

        public JogosController(IJogoService jogoService)
        {
            this._service = jogoService;
        }

        [HttpGet]
        public async Task<ActionResult<List<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, int.MaxValue)] int quantidade = 5)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _service.Obter(pagina, quantidade);
                if (result == null)
                    return NotFound();
                else
                    return Ok(result);

            }
            catch (System.Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }

        }

        [HttpGet("{idjogo:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter(Guid idjogo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _service.Obter(idjogo);
                if (result == null)
                    return NotFound();
                else
                    return Ok(result);                
            }
            catch  ( System.Exception e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> Inserir([FromBody] JogoInputModel jogo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _service.Inserir(jogo);
                if (result == null)
                    return NotFound(result);
                else
                    return Ok(result);
                
            }
            catch(JogoJaCadastradoException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut("{idjogo:guid}")]
        public async Task<ActionResult> AtualizarJogo(Guid idjogo, [FromBody] JogoInputModel jogo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                 await _service.Atualizar(idjogo, jogo);
                return Ok();
            }
            catch(JogoNaoCadastradoException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPatch("{idjogo:guid}/preco{preco:double}")]
        public async Task<ActionResult> AtualizarJogo(Guid idjogo, double preco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _service.Atualizar(idjogo, preco);
                return Ok();
            }
            catch (JogoNaoCadastradoException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete("idjogo:guid")]
        public async Task<ActionResult> DeletarJogo(Guid idjogo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _service.Remover(idjogo);
                return Ok();
            }
            catch (JogoNaoCadastradoException e)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
            }
        }


    }
}
