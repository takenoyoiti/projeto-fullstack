using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Model
{
    public class Empresa
    {
        [Key]
        public int Id { get; protected set; }

        [Required]
        [MaxLength(14)]
        public string CNPJ { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string NomeFantasia { get; set; } =  string.Empty;

        [Required]
        [MaxLength(8)]
        public string CEP { get; set; } = string.Empty;

        public virtual List<FornecedorEmpresa>? FornecedorEmpresa { get; set; }

    }
}
