using Opah_API.Domain.Repository.Interface;
using Opah_API.Domain.Service.Interface;
using Opah_API.Domain.VO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opah_API.Domain.Service
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;
        public ClienteService(IClienteRepository repository)
        {
            _clienteRepository = repository;
        }
        public async Task<IReadOnlyList<ClienteVO>> Get(ClienteFilterVO filter)
        {
            return await _clienteRepository.Get(filter);
        }
        public async Task<ClienteVO> Get(int id)
        {
            return await _clienteRepository.Get(id);
        }
        public async Task<ResultVO> Insert(ClienteVO cliente)
        {
            return await _clienteRepository.Insert(cliente);
        }
        public async Task<ResultVO> Update(ClienteVO cliente)
        {
            return await _clienteRepository.Update(cliente);
        }
        public async Task<ResultVO> Delete(int id)
        {
            return await _clienteRepository.Delete(id);
        }
    }
}