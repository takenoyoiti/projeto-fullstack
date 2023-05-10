using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Model;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedoresController : ControllerBase
    {
        private readonly AppDBContext _context;

        public FornecedoresController(AppDBContext context)
        {
            _context = context;
        }

        // GET: api/Fornecedores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fornecedor>>> GetFornecedor()
        {
          if (_context.Fornecedor == null)
          {
              return NotFound();
          }
            return await _context.Fornecedor.Include(fe => fe.FornecedorEmpresa)
                   .ThenInclude(e => e.Empresa).ToListAsync();
        }

        // GET: api/Fornecedores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fornecedor>> GetFornecedor(int id)
        {
          if (_context.Fornecedor == null)
          {
              return NotFound();
          }
            var fornecedor = await _context.Fornecedor.Include(fe => fe.FornecedorEmpresa)
                   .ThenInclude(e => e.Empresa).FirstOrDefaultAsync(f => f.Id == id);

            if (fornecedor == null)
            {
                return NotFound();
            }

            return fornecedor;
        }

        // PUT: api/Fornecedores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFornecedor(int id, Fornecedor fornecedor)
        {
            if (id != fornecedor.Id)
            {
                return BadRequest();
            }

            // Verifica se o CPF/CNPJ já está cadastrado
            if (await _context.Fornecedor.AnyAsync(x => x.CnpjCpf == fornecedor.CnpjCpf))
            {
                return BadRequest("O CPF/CNPJ informado já está cadastrado.");
            }

            // Verifica se é uma empresa do Paraná e o fornecedor é pessoa física menor de idade
            if (fornecedor.TipoFornecedor == TipoFornecedor.PessoaFisica && _context.Empresa.Any(x => x.CEP.StartsWith("8")) && fornecedor.DataNascimento > DateTime.Today.AddYears(-18))
            {
                return BadRequest("Não é permitido cadastrar um fornecedor pessoa física menor de idade para uma empresa do Paraná.");
            }

            _context.Entry(fornecedor).State = EntityState.Modified;

            fornecedor.FornecedorEmpresa = await _context.FornecedorEmpresa
            .Where(fe => fe.FornecedorId == fornecedor.Id)
            .ToListAsync();

            fornecedor.FornecedorEmpresa.Clear();

            foreach (var empresaId in fornecedor.FornecedorEmpresa)
            {
                var empresa = await _context.Empresa.FindAsync(empresaId);

                if (empresa == null)
                {
                    return BadRequest();
                }

                var fornecedorEmpresa = new FornecedorEmpresa
                {
                    Empresa = empresa,
                    Fornecedor = fornecedor
                };

                fornecedor.FornecedorEmpresa.Add(fornecedorEmpresa);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FornecedorExists(id))
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

        // POST: api/Fornecedores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Fornecedor>> PostFornecedor(Fornecedor fornecedor)
        {
            if (_context.Fornecedor == null)
            {
                return Problem("Entity set 'AppDBContext.Fornecedor'  is null.");
            }

            // Verifica se o CPF/CNPJ já está cadastrado
            if (await _context.Fornecedor.AnyAsync(x => x.CnpjCpf == fornecedor.CnpjCpf))
            {
                return BadRequest("O CPF/CNPJ informado já está cadastrado.");
            }

            // Verifica se é uma empresa do Paraná e o fornecedor é pessoa física menor de idade
            if (fornecedor.TipoFornecedor == TipoFornecedor.PessoaFisica && _context.Empresa.Any(x => x.CEP.StartsWith("8")) && fornecedor.DataNascimento > DateTime.Today.AddYears(-18))
            {
                return BadRequest("Não é permitido cadastrar um fornecedor pessoa física menor de idade para uma empresa do Paraná.");
            }

            if (fornecedor.FornecedorEmpresa != null)
            {
                if (fornecedor.FornecedorEmpresa.Any())
                {
                    foreach (var item in fornecedor.FornecedorEmpresa)
                    {
                        var empresa = await _context.Empresa.FindAsync(item.EmpresaId);
                        if (empresa == null)
                        {
                            return BadRequest($"Empresa com ID {item.EmpresaId} não encontrada.");
                        }

                        var fornecedorEmpresa = new FornecedorEmpresa
                        {
                            FornecedorId = fornecedor.Id,
                            EmpresaId = item.EmpresaId
                        };
                    }
                }
            }
            _context.Fornecedor.Add(fornecedor);
                
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFornecedor", new { id = fornecedor.Id }, fornecedor);
        }

        // DELETE: api/Fornecedores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFornecedor(int id)
        {
            if (_context.Fornecedor == null)
            {
                return NotFound();
            }
            var fornecedor = await _context.Fornecedor.FindAsync(id);
            if (fornecedor == null)
            {
                return NotFound();
            }

            // Busca a associação do fornecedor com a empresa na tabela FornecedorEmpresa
            var fornecedorEmpresa = await _context.FornecedorEmpresa.FirstOrDefaultAsync(fe => fe.FornecedorId == id);

            // Remove a associação do fornecedor com a empresa
            if (fornecedorEmpresa != null)
            {
                _context.FornecedorEmpresa.Remove(fornecedorEmpresa);
                await _context.SaveChangesAsync();
            }

            _context.Fornecedor.Remove(fornecedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FornecedorExists(int id)
        {
            return (_context.Fornecedor?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
