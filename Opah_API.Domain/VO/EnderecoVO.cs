using System;
using System.Collections.Generic;
using System.Text;

namespace Opah_API.Domain.VO
{
    public class EnderecoVO
    {
        public int Id { get; set; }        
        public string RUA { get; set; }        
        public string BAIRRO { get; set; }        
        public string NUMERO { get; set; }        
        public string COMPLEMENTO { get; set; }        
        public string CEP { get; set; }        
        public int TIPO_ENDERECO { get; set; }        
        public CidadeVO TB_CIDADE { get; set; }
        public ClienteVO TB_CLIENTE { get; set; }
    }
}
