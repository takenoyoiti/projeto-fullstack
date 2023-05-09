using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresasController : ControllerBase
    {
        private readonly AppDBContext _context;

        public EmpresasController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Empresas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empresa>>> GetEmpresa()
        {
          if (_context.Empresa == null)
          {
              return NotFound();
          }
            return await _context.Empresa.Include(e => e.FornecedorEmpresa)
            .ThenInclude(fe => fe.Fornecedor).ToListAsync();
        }

        // GET: api/Empresas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Empresa>> GetEmpresa(int id)
        {
          if (_context.Empresa == null)
          {
              return NotFound();
          }
            var empresa = await _context.Empresa.Include(e => e.FornecedorEmpresa)
            .ThenInclude(fe => fe.Fornecedor).FirstOrDefaultAsync(e => e.Id == id);

            if (empresa == null)
            {
                return NotFound();
            }

            return empresa;
        }

        // PUT: api/Empresas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpresa(int id, Empresa empresa)
        {
            if (id != empresa.Id)
            {
                return BadRequest();
            }

            _context.Entry(empresa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpresaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Empresas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Empresa>> PostEmpresa(Empresa empresa)
        {
            if (_context.Empresa == null)
            {
                return Problem("Entity set 'AppDBContext.Empresa'  is null.");
            }
            
            _context.Empresa.Add(empresa);
            if (empresa.FornecedorEmpresa != null)
            {
                foreach (var fornecedor in empresa.FornecedorEmpresa)
                {
                    _context.FornecedorEmpresa.Add(new FornecedorEmpresa { FornecedorId = fornecedor.Id, EmpresaId = empresa.Id });
                }
            }
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmpresa", new { id = empresa.Id }, empresa);
        }

        // DELETE: api/Empresas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpresa(int id)
        {
            if (_context.Empresa == null)
            {
                return NotFound();
            }
            var empresa = await _context.Empresa.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            // Verifica se a empresa tem fornecedores associados a ela
            var fornecedoresAssociados = _context.FornecedorEmpresa.Where(fe => fe.EmpresaId == id).ToList();

            // Remove o relacionamento entre a empresa e cada fornecedor associado a ela
            if (fornecedoresAssociados.Any())
            {
                _context.FornecedorEmpresa.RemoveRange(fornecedoresAssociados);
            }
            _context.Empresa.Remove(empresa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmpresaExists(int id)
        {
            return (_context.Empresa?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
