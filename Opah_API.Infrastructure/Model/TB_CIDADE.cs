using CoreEssentials.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Opah_API.Infrastructure.Model
{
    public class TB_CIDADE : BaseEntity
    {
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string NOME { get; set; }
        [Required]
        [Column(TypeName = "char(2)")]
        public string ESTADO { get; set; }
        public ICollection<TB_ENDERECO> ENDERECOS { get; set; }
    }
}
