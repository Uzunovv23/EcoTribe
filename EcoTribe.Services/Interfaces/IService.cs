using EcoTribe.BusinessObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Interfaces
{
    public interface IService<TViewModel, TInputModel>
    {
        IEnumerable<TViewModel> GetAll();
        IEnumerable<TViewModel> GetAll(string userId);
        TViewModel? GetById(int id);
        void Create(TInputModel inputModel);
        void Update(int id, TInputModel inputModel);
        void Delete(int id);
    }
}
