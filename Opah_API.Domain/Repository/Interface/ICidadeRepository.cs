using Opah_API.Domain.VO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Opah_API.Domain.Repository.Interface
{
    public interface ICidadeRepository
    {
        //Task<CidadeVO> GetById(Guid id);
        Task<IReadOnlyList<CidadeVO>> Get();
        //Task<CidadeVO> Insert(CidadeVO obj);
        //Task<CidadeVO> Update(CidadeVO obj);
        //Task<bool> Delete(Guid id);
    }
}
