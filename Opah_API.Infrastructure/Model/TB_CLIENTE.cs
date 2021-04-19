using CoreEssentials.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Opah_API.Infrastructure.Model
{
    public class TB_CLIENTE : BaseEntity
    {
        [Required]
        [Column(TypeName = "varchar(150)")]
        public string NOME { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string RG { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string CPF { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime DATA_NASCIMENTO { get; set; }
        [Required]  
        [Column(TypeName = "varchar(20)")]
        public string TELEFONE { get; set; }
        [Required]
        [Column(TypeName = "varchar(150)")]
        public string EMAIL_ { get; set; }
        [Required]
        [Column(TypeName = "int")]
        public int COD_EMPRESA { get; set; }
        public ICollection<TB_ENDERECO> ENDERECOS { get; set; }
        public TB_CLIENTE()
        {
            ENDERECOS = new Collection<TB_ENDERECO>();
        }
    }
}
