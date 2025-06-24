using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Utils
{
    public interface IModelConverter
    {
        TViewModel ConvertToViewModel<TModel, TViewModel>(TModel model)
            where TViewModel : new();

        TModel ConvertToModel<TInputModel, TModel>(TInputModel inputModel)
            where TModel : new();

        List<TViewModel> ConvertListToViewModel<TModel, TViewModel>(IEnumerable<TModel> models)
            where TViewModel : new();

        List<TModel> ConvertListToModel<TInputModel, TModel>(IEnumerable<TInputModel> inputModels)
            where TModel : new();
    }
}
