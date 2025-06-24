using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Utils
{
    public class ModelConverter : IModelConverter
    {
        public TViewModel ConvertToViewModel<TModel, TViewModel>(TModel model)
             where TViewModel : new()
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return Convert<TModel, TViewModel>(model);
        }

        public TModel ConvertToModel<TInputModel, TModel>(TInputModel inputModel)
            where TModel : new()
        {
            if (inputModel == null) throw new ArgumentNullException(nameof(inputModel));
            return Convert<TInputModel, TModel>(inputModel);
        }

        public List<TViewModel> ConvertListToViewModel<TModel, TViewModel>(IEnumerable<TModel> models)
            where TViewModel : new()
        {
            if (models == null) return new List<TViewModel>();
            return models.Select(m => Convert<TModel, TViewModel>(m)).ToList();
        }

        public List<TModel> ConvertListToModel<TInputModel, TModel>(IEnumerable<TInputModel> inputModels)
            where TModel : new()
        {
            if (inputModels == null) return new List<TModel>();
            return inputModels.Select(m => Convert<TInputModel, TModel>(m)).ToList();
        }

        private TDestination Convert<TSource, TDestination>(TSource source)
            where TDestination : new()
        {
            var destination = new TDestination();
            var sourceProps = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var destProps = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var sourceProp in sourceProps)
            {
                var destProp = destProps.FirstOrDefault(p => p.Name == sourceProp.Name && p.PropertyType == sourceProp.PropertyType);
                if (destProp != null && destProp.CanWrite)
                {
                    destProp.SetValue(destination, sourceProp.GetValue(source));
                }
            }

            return destination;
        }

    }
}
