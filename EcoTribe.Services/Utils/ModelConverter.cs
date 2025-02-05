using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Utils
{
    public class ModelConverter
    {
        public static TViewModel ConvertToViewModel<TModel, TViewModel>(TModel model)
            where TViewModel : new()
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            return Convert<TModel, TViewModel>(model);
        }

        public static TModel ConvertToModel<TInputModel, TModel>(TInputModel inputModel)
            where TModel : new()
        {
            if (inputModel == null) throw new ArgumentNullException(nameof(inputModel));

            return Convert<TInputModel, TModel>(inputModel);
        }

        private static TDestination Convert<TSource, TDestination>(TSource source)
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
