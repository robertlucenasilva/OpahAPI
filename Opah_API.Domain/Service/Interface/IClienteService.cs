using Opah_API.Domain.VO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opah_API.Domain.Service.Interface
{
    public interface IClienteService
    {
        Task<IReadOnlyList<ClienteVO>> Get(ClienteFilterVO filter);
        Task<ClienteVO> Get(int id);
        Task<ResultVO> Insert(ClienteVO cliente);
        Task<ResultVO> Update(ClienteVO cliente);
        Task<ResultVO> Delete(int id);
    }
}
