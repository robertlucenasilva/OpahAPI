using Microsoft.AspNetCore.Mvc;
using Moq;
using Opah_API.Controllers;
using Opah_API.Domain.Service.Interface;
using Opah_API.Domain.VO;
using Opah_API.Infrastructure.Interfaces;
using Opah_API.Infrastructure.Repository;
using Opah_API.Test.Fake;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace Opah_API.Test
{
    public class ClienteControllerTest
    {
        ClienteController _controller;
        IClienteService _service;
        public ClienteControllerTest()
        {
            _service = new ClienteServiceFake();
            _controller = new ClienteController(_service);
        }

        #region Get Todos

        [Fact]
        public void Get_Chamado_RetornaOk()
        {
            var cliente = new ClienteFilterVO() { COD_EMPRESA = 1 };

            var result = _controller.Get(cliente);

            Assert.IsType<OkObjectResult>(result.Result);
        }
        [Fact]
        public void Get_ChamadoSemCodEmpresa_RetornaBadRequest()
        {
            var cliente = new ClienteFilterVO() { };

            var result = _controller.Get(cliente);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void Get_ChamadoComParametroIncorreto_RetornaNotFound()
        {
            var cliente = new ClienteFilterVO() { COD_EMPRESA = 9 };

            var result = _controller.Get(cliente);

            Assert.IsType<NotFoundResult>(result.Result);
        }

        #endregion

        #region Get por id

        [Fact]
        public void Get_ChamadoPorId_RetornaOk()
        {
            var clienteId = 1;

            var result = _controller.Get(clienteId);

            Assert.IsType<OkObjectResult>(result.Result.Result);
        }
        [Fact]
        public void Get_ChamadoPorIdComIdInvalido_RetornaOk()
        {
            var clienteId = 99;

            var result = _controller.Get(clienteId);

            Assert.IsType<NotFoundResult>(result.Result.Result);
        }

        #endregion

        #region Insert
        [Fact]
        public void Insert_TodosOsDadosEnviados_RetornaOk()
        {
            var cliente = new ClienteVO()
            {
                NOME = "Maria  Madalena",
                RG = "256322363",
                CPF = "36569866532",
                DATA_NASCIMENTO = new DateTime(1500, 7, 15),
                TELEFONE = "12965236532",
                EMAIL_ = "mm@gmail.com",
                COD_EMPRESA = Domain.VO.Enum.CodEmpresaEnum.Carrefour,
                ENDERECOS = new List<EnderecoVO>()
                {
                    new EnderecoVO()
                    {
                        RUA ="Rua Salvação",
                        BAIRRO="Bairro Alto",
                        NUMERO="52",
                        COMPLEMENTO="",
                        CEP="06845632",
                        TIPO_ENDERECO= 1,
                        TB_CIDADE = new CidadeVO(){ Id = 2}
                    },                
                    new EnderecoVO()
                    {
                        RUA = "Rua da Gloria",
                        BAIRRO = "Bairro da Graça",
                        NUMERO = "93,",
                        COMPLEMENTO = "",
                        CEP = "06542365",
                        TIPO_ENDERECO = 2,
                        TB_CIDADE = new CidadeVO() { Id = 1 }
                    }
                }
            };

            var result = _controller.Insert(cliente);

            Assert.IsType<OkObjectResult>(result.Result);
        }
        [Fact]
        public void Insert_MesmoTipoDeEndereco_RetornaBadRequest()
        {
            var cliente = new ClienteVO()
            {
                NOME = "Silvio Santos",
                RG = "42698235",
                CPF = "12563522967",
                DATA_NASCIMENTO = new DateTime(1962, 6, 15),
                TELEFONE = "11983669105",
                EMAIL_ = "silvio.santos@gmail.com",
                COD_EMPRESA = Domain.VO.Enum.CodEmpresaEnum.Carrefour,
                ENDERECOS = new List<EnderecoVO>()
                {
                    new EnderecoVO()
                    {
                        RUA ="Rua Salvação",
                        BAIRRO="Bairro Alto",
                        NUMERO="52",
                        COMPLEMENTO="",
                        CEP="06845632",
                        TIPO_ENDERECO= 1,
                        TB_CIDADE = new CidadeVO(){ Id = 2}
                    },
                    new EnderecoVO()
                    {
                        RUA = "Rua da Gloria",
                        BAIRRO = "Bairro da Graça",
                        NUMERO = "93,",
                        COMPLEMENTO = "",
                        CEP = "06542365",
                        TIPO_ENDERECO = 1,
                        TB_CIDADE = new CidadeVO() { Id = 1 }
                    }
                }
            };


            var result = _controller.Insert(cliente);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        [Fact]
        public void Insert_CPFJaPresenteParaCodEmpresa_RetornaBadRequest()
        {
            var cliente = new ClienteVO()
            {
                NOME = "Silvio Santos",
                RG = "42698235",
                CPF = "12563266539",
                DATA_NASCIMENTO = new DateTime(1962, 6, 15),
                TELEFONE = "11983669105",
                EMAIL_ = "silvio.santos@gmail.com",
                COD_EMPRESA = Domain.VO.Enum.CodEmpresaEnum.Carrefour,
                ENDERECOS = new List<EnderecoVO>()
                {
                    new EnderecoVO()
                    {
                        RUA ="Rua Salvação",
                        BAIRRO="Bairro Alto",
                        NUMERO="52",
                        COMPLEMENTO="",
                        CEP="06845632",
                        TIPO_ENDERECO= 1,
                        TB_CIDADE = new CidadeVO(){ Id = 2}
                    },
                    new EnderecoVO()
                    {
                        RUA = "Rua da Gloria",
                        BAIRRO = "Bairro da Graça",
                        NUMERO = "93,",
                        COMPLEMENTO = "",
                        CEP = "06542365",
                        TIPO_ENDERECO = 2,
                        TB_CIDADE = new CidadeVO() { Id = 1 }
                    }
                }
            };


            var result = _controller.Insert(cliente);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        #endregion

        #region Update
        [Fact]
        public void Update_TodosOsDadosEnviados_RetornaOk()
        {
            var cliente = new ClienteVO()
            {
                NOME = "Silvio Santos",
                RG = "42698235",
                CPF = "12563522967",
                DATA_NASCIMENTO = new DateTime(1962, 6, 15),
                TELEFONE = "11983669105",
                EMAIL_ = "silvio.santos@gmail.com",
                COD_EMPRESA = Domain.VO.Enum.CodEmpresaEnum.Carrefour,
                ENDERECOS = new List<EnderecoVO>()
                {
                    new EnderecoVO()
                    {
                        RUA ="Rua Salvação",
                        BAIRRO="Bairro Alto",
                        NUMERO="52",
                        COMPLEMENTO="",
                        CEP="06845632",
                        TIPO_ENDERECO= 1,
                        TB_CIDADE = new CidadeVO(){ Id = 2}
                    },
                    new EnderecoVO()
                    {
                        RUA = "Rua da Gloria",
                        BAIRRO = "Bairro da Graça",
                        NUMERO = "93,",
                        COMPLEMENTO = "",
                        CEP = "06542365",
                        TIPO_ENDERECO = 2,
                        TB_CIDADE = new CidadeVO() { Id = 1 }
                    }
                }
            };

            var result = _controller.Insert(cliente);

            Assert.IsType<OkObjectResult>(result.Result);
        }
        [Fact]
        public void Update_MaisDeUmTipoDeEndereco_RetornaBadRequest()
        {
            var cliente = new ClienteVO()
            {
                NOME = "Silvio Santos",
                RG = "42698235",
                CPF = "12563522967",
                DATA_NASCIMENTO = new DateTime(1962, 6, 15),
                TELEFONE = "11983669105",
                EMAIL_ = "silvio.santos@gmail.com",
                COD_EMPRESA = Domain.VO.Enum.CodEmpresaEnum.Carrefour,
                ENDERECOS = new List<EnderecoVO>()
                {
                    new EnderecoVO()
                    {
                        RUA ="Rua Salvação",
                        BAIRRO="Bairro Alto",
                        NUMERO="52",
                        COMPLEMENTO="",
                        CEP="06845632",
                        TIPO_ENDERECO= 1,
                        TB_CIDADE = new CidadeVO(){ Id = 2}
                    },
                    new EnderecoVO()
                    {
                        RUA = "Rua da Gloria",
                        BAIRRO = "Bairro da Graça",
                        NUMERO = "93,",
                        COMPLEMENTO = "",
                        CEP = "06542365",
                        TIPO_ENDERECO = 1,
                        TB_CIDADE = new CidadeVO() { Id = 1 }
                    }
                }
            };

            var result = _controller.Insert(cliente);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        [Fact]
        public void Update_CPFJaPresenteParaCodEmpresa_RetornaBadRequest()
        {
            var cliente = new ClienteVO()
            {
                NOME = "Silvio Santos",
                RG = "42698235",
                CPF = "25632566987",
                DATA_NASCIMENTO = new DateTime(1962, 6, 15),
                TELEFONE = "11983669105",
                EMAIL_ = "silvio.santos@gmail.com",
                COD_EMPRESA = Domain.VO.Enum.CodEmpresaEnum.Carrefour,
                ENDERECOS = new List<EnderecoVO>()
                {
                    new EnderecoVO()
                    {
                        RUA ="Rua Salvação",
                        BAIRRO="Bairro Alto",
                        NUMERO="52",
                        COMPLEMENTO="",
                        CEP="06845632",
                        TIPO_ENDERECO= 1,
                        TB_CIDADE = new CidadeVO(){ Id = 2}
                    },
                    new EnderecoVO()
                    {
                        RUA = "Rua da Gloria",
                        BAIRRO = "Bairro da Graça",
                        NUMERO = "93,",
                        COMPLEMENTO = "",
                        CEP = "06542365",
                        TIPO_ENDERECO = 2,
                        TB_CIDADE = new CidadeVO() { Id = 1 }
                    }
                }
            };

            var result = _controller.Insert(cliente);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        #endregion

        #region Delete
        [Fact]
        public void Delete_IdExistente_RetornaOk()
        {
            var id = 1;

            var result = _controller.Delete(id);

            Assert.IsType<OkObjectResult>(result.Result);
        }
        [Fact]
        public void Delete_IdInexistente_RetornaBadRequest()
        {
            var id = 98989898;

            var result = _controller.Delete(id);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        #endregion
    }
}
