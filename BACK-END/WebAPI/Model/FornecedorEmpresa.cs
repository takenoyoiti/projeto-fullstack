using System.ComponentModel.DataAnnotations;

namespace WebAPI.Model
{

    public class FornecedorEmpresa
    {
        [Key]
        public int Id { get; internal set; }
        public int FornecedorId { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }

        public int EmpresaId { get; set; }
        public virtual Empresa Empresa { get; set; }
    }
}
