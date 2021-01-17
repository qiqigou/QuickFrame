using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace QuickFrame.Web
{
    /// <summary>
    /// 提供string转byte[]的IModelBinder
    /// </summary>
    public class MyByteArrayModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(byte[]))
            {
                return new MyByteArrayModelBinder();
            }
            return default;
        }
    }
    /// <summary>
    /// string到byte[]的转换器
    /// </summary>
    public class MyByteArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            //var bind = bindingContext as DefaultModelBindingContext;
            //var valueProviders = bind?.OriginalValueProvider as CompositeValueProvider;
            //var routeValue = valueProviders?[0] as RouteValueProvider;
            //var dict = typeof(RouteValueProvider).GetField("_values", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(routeValue) as RouteValueDictionary;

            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);

            var value = valueProviderResult.FirstValue;
            if (string.IsNullOrEmpty(value))
            {
                return Task.CompletedTask;
            }

            try
            {
                //解决时间戳转换失败的问题
                //value = WebUtility.UrlDecode(value);
                value = value.Replace("%2f", "/");
                var model = Convert.FromBase64String(value);
                bindingContext.Result = ModelBindingResult.Success(model);
            }
            catch (Exception exception)
            {
                bindingContext.ModelState.TryAddModelError(
                    bindingContext.ModelName,
                    exception,
                    bindingContext.ModelMetadata);
            }

            return Task.CompletedTask;
        }
    }
}