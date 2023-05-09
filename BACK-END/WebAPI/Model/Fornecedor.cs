using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Model
{
    public enum TipoFornecedor
    {
        PessoaFisica,
        PessoaJuridica
    }
    public class Fornecedor
    {
        [Key]
        public int Id { get; internal set; }

        [Required]
        [MaxLength(14)]
        public string CnpjCpf { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(8)]
        public string CEP { get; set; } = string.Empty;

        [Required]
        [MaxLength(9)]
        public string RG { get; set; } = string.Empty;

        [Required]
        public DateTime? DataNascimento { get; set; }

        [Required]
        [EnumDataType(typeof(TipoFornecedor))]
        public TipoFornecedor TipoFornecedor { get; set; }
        public virtual List<FornecedorEmpresa>? FornecedorEmpresa { get; set; }
    }

}

