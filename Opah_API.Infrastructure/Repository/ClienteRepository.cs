using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Opah_API.Domain.Repository.Interface;
using Opah_API.Domain.VO;
using Opah_API.Domain.VO.Enum;
using Opah_API.Infrastructure.Interfaces;
using Opah_API.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opah_API.Infrastructure.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly IApplicationReadDbConnection _readDbConnection;
        private readonly IApplicationDbContext _context;
        private readonly ILogger _logger;
        public ClienteRepository(IApplicationReadDbConnection readDbConnection, IApplicationDbContext context)
        {
            _readDbConnection = readDbConnection;
            _context = context;
        }

        public async Task<IReadOnlyList<ClienteVO>> Get(ClienteFilterVO filter)
        {
            try
            {
                var parameters = new { COD_EMPRESA = filter.COD_EMPRESA, NOME = filter.NOME, CPF = filter.CPF, CIDADE = filter.CIDADE, ESTADO = filter.ESTADO };
                var query = @"SELECT DISTINCT C.ID, C.COD_EMPRESA, C.NOME, C.CPF, C.RG, C.DATA_NASCIMENTO, C.TELEFONE, C.EMAIL_, CI.NOME as CIDADE, CI.ESTADO as ESTADO FROM TB_CLIENTE C                                
	                            LEFT JOIN TB_ENDERECO E ON(E.TB_CLIENTE_ID = C.ID)
                                LEFT JOIN TB_CIDADE CI ON(E.TB_CIDADE_ID = CI.ID)
                            where 
                                C.COD_EMPRESA = @COD_EMPRESA and (@NOME IS NULL OR C.NOME LIKE CONCAT('%',@NOME,'%')) and (@CPF  IS NULL OR C.CPF = @CPF) and (@CIDADE IS NULL OR CI.NOME LIKE CONCAT('%',@CIDADE,'%')) and (@ESTADO  IS NULL OR CI.ESTADO = @ESTADO) ";
                return await _readDbConnection.QueryAsync<ClienteVO>(query, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(string.Format("Erro ao tentar buscar clientes por filtro.  Mensagem de Erro = {0}", ex.Message));
                return null;
            }
        }

        public async Task<ClienteVO> Get(int id)
        {
            try
            {
                var cliente =  await _context.TB_CLIENTE.Where(p => p.Id == id).Select(p => new ClienteVO()
                {
                    ID = p.Id,
                    NOME = p.NOME,
                    COD_EMPRESA = (CodEmpresaEnum)p.COD_EMPRESA,
                    CPF = p.CPF,
                    DATA_NASCIMENTO = p.DATA_NASCIMENTO,
                    RG = p.RG,
                    TELEFONE = p.TELEFONE,
                    EMAIL_ = p.EMAIL_,
                    ENDERECOS = p.ENDERECOS.Select(p => new EnderecoVO()
                    {
                        Id = p.Id,
                        RUA = p.RUA,
                        TB_CIDADE = new CidadeVO() { Id = p.TB_CIDADE.Id, ESTADO = p.TB_CIDADE.ESTADO, NOME = p.TB_CIDADE.NOME }

                    }).ToList()

                }).FirstOrDefaultAsync();

                if (cliente != null)
                {                    
                    return cliente;
                }
                else
                {                    
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(string.Format("Erro ao tentar buscar um cliente por id .  Mensagem erro = {0}", ex.Message));
                return null;
            }
        }
        public async Task<ResultVO> Insert(ClienteVO cliente)
        {
            var result = new ResultVO();
            try
            {
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
                var jaExisteCod = _context.TB_CLIENTE.Where(p => p.COD_EMPRESA == (int)cliente.COD_EMPRESA && p.CPF == cliente.CPF.Replace(".", "").Replace("-", "")).Any();
                if (!jaExisteCod)
                {
                    var dbCliente = new TB_CLIENTE()
                    {
                        COD_EMPRESA = (int)cliente.COD_EMPRESA,
                        CPF = cliente.CPF.Replace(".", "").Replace("-", ""),
                        DATA_NASCIMENTO = cliente.DATA_NASCIMENTO,
                        EMAIL_ = cliente.EMAIL_,
                        NOME = cliente.NOME,
                        RG = cliente.RG,
                        TELEFONE = cliente.TELEFONE
                    };
                    foreach (var item in cliente.ENDERECOS)
                    {
                        TB_ENDERECO dbEndereco = new TB_ENDERECO()
                        {
                            BAIRRO = item.BAIRRO,
                            CEP = item.CEP,
                            COMPLEMENTO = item.COMPLEMENTO,
                            NUMERO = item.NUMERO,
                            RUA = item.RUA,
                            TIPO_ENDERECO = item.TIPO_ENDERECO,
                            TB_CIDADE_ID = item.TB_CIDADE.Id,
                            TB_CLIENTE_ID = dbCliente.Id
                        };
                        dbCliente.ENDERECOS.Add(dbEndereco);
                    }
                    _context.TB_CLIENTE.Add(dbCliente);
                    _context.SaveChanges();
                    result.error = false;
                    result.message = "Cliente incluído com sucesso";
                    return result;
                }
                else
                {
                    result.error = true;
                    result.message = "CPF já cadastrado para o COD_EMPRESA " + cliente.COD_EMPRESA;
                    return result;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(string.Format("Erro ao tentar inserir um cliente.  Mensagem de erro= {0}", ex.Message));
                result.error = true;
                result.message = "Erro inesperado";
                return result;
            }
        }

        public async Task<ResultVO> Update(ClienteVO cliente)
        {
            var result = new ResultVO();
            try
            {
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
                var dbCliente = _context.TB_CLIENTE.Include("ENDERECOS").Where(p => p.Id == cliente.ID).FirstOrDefault();
                if (dbCliente != null)
                {
                    var existeCod = _context.TB_CLIENTE.Where(p => p.COD_EMPRESA == (int)cliente.COD_EMPRESA && p.CPF == cliente.CPF.Replace(".", "").Replace("-", "") && p.Id != cliente.ID).Any();
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
                                _context.TB_ENDERECO.Remove(endereco);
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
                        _context.SaveChanges();
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
            catch (Exception ex)
            {
                _logger.LogInformation(string.Format("Erro ao tentar atualizar um cliente .  Mensagem de Erro = {0}", ex.Message));
                result.error = true;
                result.message = "Erro inesperado";
                return result;
            }
        }
        public async Task<ResultVO> Delete(int id)
        {
            var result = new ResultVO();
            try
            {
                var cliente = _context.TB_CLIENTE.Find(id);
                if (cliente != null)
                {
                    _context.TB_CLIENTE.Remove(cliente);
                    _context.SaveChanges();
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
            catch (Exception ex)
            {
                _logger.LogInformation(string.Format("Erro ao tentar deletar um cliente .  Mensagem de erro = {0}", ex.Message));
                result.error = true;
                result.message = "Erro inesperado";
                return result;
            }
        }
    }
}
