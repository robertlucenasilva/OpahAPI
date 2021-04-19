using Opah_API.Domain.Service.Interface;
using Opah_API.Domain.VO;
using Opah_API.Domain.VO.Enum;
using Opah_API.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opah_API.Test.Fake
{
    public class ClienteServiceFake : IClienteService
    {
        private readonly List<TB_CLIENTE> _cliente;
        private readonly List<TB_CIDADE> _cidade;
        private readonly List<TB_ENDERECO> _endereco;
        public ClienteServiceFake()
        {
            _cliente = new List<TB_CLIENTE>()
            {
                new TB_CLIENTE() { Id = 1, NOME = "Silvio Santos", COD_EMPRESA = 1, CPF = "12563266539", },
                new TB_CLIENTE() { Id = 2, NOME = "Fausto Silva", COD_EMPRESA = 2, CPF = "62598566587"},
                new TB_CLIENTE() { Id = 3, NOME = "Marcos Mion", COD_EMPRESA = 1, CPF = "25632566987"},
            };

            _cidade = new List<TB_CIDADE>() 
            {
                
                new TB_CIDADE() { Id = 1, NOME = "Embu das Artes", ESTADO = "SP" }, 
                new TB_CIDADE() { Id = 2, NOME = "Taboão da Serra", ESTADO = "SP" } 
            };

            _endereco = new List<TB_ENDERECO>()
            {

                new TB_ENDERECO() { Id = 1, RUA = "Rua Salvação",  BAIRRO="Bairro Alto", NUMERO="52", COMPLEMENTO="", CEP="06845632", TIPO_ENDERECO= 1, TB_CIDADE = new TB_CIDADE(){ Id = 2}, TB_CLIENTE_ID = 1 },
                new TB_ENDERECO() { Id = 2, RUA = "Rua da Gloria",BAIRRO = "Bairro da Graça",NUMERO = "93,",COMPLEMENTO = "",CEP = "06542365",TIPO_ENDERECO = 2, TB_CIDADE = new TB_CIDADE() { Id = 1 }, TB_CLIENTE_ID = 2 }
            };
        }
        public async Task<ResultVO> Delete(int id)
        {
            var result = new ResultVO();
            var cliente = _cliente.Where(p => p.Id == id).FirstOrDefault();
            if (cliente != null)
            {
                _cliente.Remove(cliente);
                result.error = false;
                result.message = "Cliente deletado com sucesso";
                return result;
            }
            else
            {
                result.error = true;
                result.message = "Cliente não encontrado";
                return result;
            }
        }

        public async Task<IReadOnlyList<ClienteVO>> Get(ClienteFilterVO filter)
        {
            var cliente = _cliente.Where(p => p.COD_EMPRESA == filter.COD_EMPRESA);
            if (!string.IsNullOrEmpty(filter.CIDADE))
            {
                cliente = cliente.Where(p => p.ENDERECOS.Where(p => p.TB_CIDADE.NOME == filter.CIDADE).Any());
            }
            if (!string.IsNullOrEmpty(filter.NOME))
            {
                cliente = cliente.Where(p => p.NOME == filter.NOME);
            }
            if (!string.IsNullOrEmpty(filter.CPF))
            {
                cliente = cliente.Where(p => p.CPF == filter.CPF);
            }
            if (!string.IsNullOrEmpty(filter.ESTADO))
            {
                cliente = cliente.Where(p => p.ENDERECOS.Where(p => p.TB_CIDADE.ESTADO == filter.ESTADO).Any());
            }           
                       
            return ModelToVO(cliente.ToList());
        }

        public async Task<ClienteVO> Get(int id)
        {
            var cliente = _cliente.Where(p => p.Id == id).FirstOrDefault();
            if (cliente != null)
            {                
                return ModelToVO(new List<TB_CLIENTE>() { cliente }).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public async Task<ResultVO> Insert(ClienteVO cliente)
        {
            var result = new ResultVO();
            cliente.ID = new Random().Next();
            foreach (var item in cliente.ENDERECOS)
            {
                var tipoEndereco = cliente.ENDERECOS.Where(p => p.TIPO_ENDERECO == item.TIPO_ENDERECO).Count();
                if (tipoEndereco > 1)
                {
                    result.message = "Não é possível inserir mais de um endereço com o mesmo TIPO_ENDERECO ";
                    result.error = true;
                    return result;
                }
            }
            var jaExisteCod = _cliente.Where(p => p.COD_EMPRESA == (int)cliente.COD_EMPRESA && p.CPF == cliente.CPF.Replace(".", "").Replace("-", "")).Any();
            if (!jaExisteCod)
            {
                _cliente.Add(VOToModel(new List<ClienteVO>() { cliente }).FirstOrDefault());
            }
            else
            {

                result.error = true;
                result.message = "CPF já cadastrado para o COD_EMPRESA " + cliente.COD_EMPRESA;
                return result;
            }
            return new ResultVO() { error = false, message = "Cliente inserido com sucesso", objectResult = cliente };
        }

        public async Task<ResultVO> Update(ClienteVO cliente)
        {
            var result = new ResultVO();
            foreach (var item in cliente.ENDERECOS)
            {
                var tipoEndereco = cliente.ENDERECOS.Where(p => p.TIPO_ENDERECO == item.TIPO_ENDERECO).Count();
                if (tipoEndereco > 1)
                {
                    result.message = "Não é possível inserir mais de um endereço com o mesmo TIPO_ENDERECO ";
                    result.error = true;
                    return result;
                }
            }
            var dbCliente = _cliente.Where(p => p.Id == cliente.ID).FirstOrDefault();
            if (dbCliente != null)
            {
                var existeCod = _cliente.Where(p => p.COD_EMPRESA == (int)cliente.COD_EMPRESA && p.CPF == cliente.CPF.Replace(".", "").Replace("-", "") && p.Id != cliente.ID).Any();
                if (!existeCod)
                {

                    dbCliente.COD_EMPRESA = (int)cliente.COD_EMPRESA;
                    dbCliente.CPF = cliente.CPF.Replace(".", "").Replace("-", "");
                    dbCliente.DATA_NASCIMENTO = cliente.DATA_NASCIMENTO;
                    dbCliente.EMAIL_ = cliente.EMAIL_;
                    dbCliente.NOME = cliente.NOME;
                    dbCliente.RG = cliente.RG;
                    dbCliente.TELEFONE = cliente.TELEFONE;

                    foreach (var endereco in dbCliente.ENDERECOS)
                    {
                        if (!cliente.ENDERECOS.Any(c => c.Id == endereco.Id))
                            _endereco.Remove(endereco);
                    }
                    foreach (var endereco in cliente.ENDERECOS)
                    {
                        TB_ENDERECO dbEndereco = new TB_ENDERECO()
                        {
                            BAIRRO = endereco.BAIRRO,
                            CEP = endereco.CEP,
                            COMPLEMENTO = endereco.COMPLEMENTO,
                            NUMERO = endereco.NUMERO,
                            RUA = endereco.RUA,
                            TIPO_ENDERECO = endereco.TIPO_ENDERECO,
                            TB_CIDADE_ID = endereco.TB_CIDADE.Id,
                            TB_CLIENTE_ID = dbCliente.Id
                        };
                        dbCliente.ENDERECOS.Add(dbEndereco);
                    }
                    result.error = false;
                    result.message = "Cliente atualizado com sucesso";
                    return result;
                }
                else
                {
                    result.error = true;
                    result.message = "CPF já cadastrado para o COD_EMPRESA " + cliente.COD_EMPRESA;
                    return result;
                }
            }
            else
            {
                result.error = true;
                result.message = "Cliente não identificado";
                return result;
            }
        }


        public List<ClienteVO> ModelToVO(List<TB_CLIENTE> model)
        {
            var resultVO = new List<ClienteVO>();
            foreach (var item in model.ToList())
            {
                var cliVO = new ClienteVO();

                cliVO.ID = item.Id;
                cliVO.NOME = item.NOME;
                cliVO.RG = item.RG;
                cliVO.CPF = item.CPF;
                cliVO.DATA_NASCIMENTO = item.DATA_NASCIMENTO;
                cliVO.TELEFONE = item.TELEFONE;
                cliVO.EMAIL_ = item.EMAIL_;
                cliVO.COD_EMPRESA = (CodEmpresaEnum)item.COD_EMPRESA;
                cliVO.ENDERECOS = new List<EnderecoVO>();
                foreach (var endereco in item.ENDERECOS)
                {
                    cliVO.ENDERECOS.Add(new EnderecoVO()
                    {
                        RUA = endereco.RUA,
                        BAIRRO = endereco.BAIRRO,
                        NUMERO = endereco.NUMERO,
                        COMPLEMENTO = endereco.COMPLEMENTO,
                        CEP = endereco.CEP,
                        TIPO_ENDERECO = endereco.TIPO_ENDERECO,
                        TB_CIDADE = new CidadeVO() { Id = endereco.TB_CIDADE_ID }
                    });
                }
                resultVO.Add(cliVO);
            };
            return resultVO;
        }
        public List<TB_CLIENTE> VOToModel(List<ClienteVO> vo)
        {
            var resultVO = new List<TB_CLIENTE>();
            foreach (var item in vo.ToList())
            {
                var cliModel = new TB_CLIENTE();

                cliModel.Id = item.ID;
                cliModel.NOME = item.NOME;
                cliModel.RG = item.RG;
                cliModel.CPF = item.CPF;
                cliModel.DATA_NASCIMENTO = item.DATA_NASCIMENTO;
                cliModel.TELEFONE = item.TELEFONE;
                cliModel.EMAIL_ = item.EMAIL_;
                cliModel.COD_EMPRESA = (int)item.COD_EMPRESA;
                cliModel.ENDERECOS = new List<TB_ENDERECO>();
                foreach (var endereco in item.ENDERECOS)
                {
                    cliModel.ENDERECOS.Add(new TB_ENDERECO()
                    {
                        RUA = endereco.RUA,
                        BAIRRO = endereco.BAIRRO,
                        NUMERO = endereco.NUMERO,
                        COMPLEMENTO = endereco.COMPLEMENTO,
                        CEP = endereco.CEP,
                        TIPO_ENDERECO = endereco.TIPO_ENDERECO,
                        TB_CIDADE = new TB_CIDADE() { Id = endereco.TB_CIDADE.Id }
                    });
                }
                resultVO.Add(cliModel);
            };
            return resultVO;
        }
    }
}
