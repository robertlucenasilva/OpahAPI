using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Converters;
using Opah_API.Domain.VO.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Opah_API.Domain.VO
{
    public class ClienteVO
    {
        [Required]
        public int ID { get; set; }
        [Required]
        public string NOME { get; set; }
        [Required]
        public string RG { get; set; }
        [Required]
        public string CPF { get; set; }
        [Required]
        public DateTime DATA_NASCIMENTO { get; set; }
        [Required]
        public string TELEFONE { get; set; }
        [Required]
        public string EMAIL_ { get; set; }
        [Required]        
        public CodEmpresaEnum COD_EMPRESA { get; set; }
        
        public string CIDADE { get; set; }
        public string ESTADO { get; set; }
        public List<EnderecoVO> ENDERECOS { get; set; }
    }
    public class ClienteFilterVO
    {        
        public string NOME { get; set; }             
        public string CPF { get; set; }                
        [BindRequired]
        public int COD_EMPRESA { get; set; }
        public string CIDADE { get; set; }
        public string ESTADO { get; set; }        
    }
}
