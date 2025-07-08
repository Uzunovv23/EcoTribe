using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            foreach (var destProp in destProps)
            {
                PropertyInfo sourceProp = null;

                sourceProp = sourceProps.FirstOrDefault(p =>
                    p.Name == destProp.Name &&
                    p.PropertyType == destProp.PropertyType);

                if (sourceProp == null)
                {
                    var mapFromAttr = destProp.GetCustomAttribute<MapFromAttribute>();
                    if (mapFromAttr != null)
                    {
                        sourceProp = sourceProps.FirstOrDefault(p =>
                            p.Name == mapFromAttr.SourceProperty &&
                            p.PropertyType == destProp.PropertyType);
                    }
                }

                if (sourceProp == null)
                {
                    sourceProp = sourceProps.FirstOrDefault(sp =>
                    {
                        var attr = sp.GetCustomAttribute<MapFromAttribute>();
                        return attr != null &&
                               attr.SourceProperty == destProp.Name &&
                               sp.PropertyType == destProp.PropertyType;
                    });
                }

                if (sourceProp != null && sourceProp.CanRead && destProp.CanWrite)
                {
                    var value = sourceProp.GetValue(source);
                    destProp.SetValue(destination, value);
                }
            }

            return destination;
        }
    }
}
