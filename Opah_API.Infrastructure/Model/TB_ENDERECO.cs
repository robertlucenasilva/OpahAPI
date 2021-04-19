using CoreEssentials.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Opah_API.Infrastructure.Model
{
    public class TB_ENDERECO : BaseEntity
    {
        [Required]
        [Column(TypeName = "varchar(255)")]
        public string RUA { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string BAIRRO { get; set; }
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string NUMERO { get; set; }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string COMPLEMENTO { get; set; }
        [Required]
        [Column(TypeName = "varchar(10)")]
        public string CEP { get; set; }
        [Required]
        [Column(TypeName = "int")]
        public int TIPO_ENDERECO { get; set; }
        [Required]
        public int TB_CIDADE_ID { get; set; }
        [ForeignKey("TB_CIDADE_ID")]
        public TB_CIDADE TB_CIDADE { get; set; }
        public int TB_CLIENTE_ID { get; set; }
        [ForeignKey("TB_CLIENTE_ID")]
        public TB_CLIENTE TB_CLIENTE { get; set; }
    }
}
