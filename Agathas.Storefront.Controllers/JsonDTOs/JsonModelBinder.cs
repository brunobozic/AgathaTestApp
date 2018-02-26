using System;
using System.Runtime.Serialization.Json;
using System.Web.Mvc;

namespace Agathas.Storefront.Controllers.JsonDTOs
{
    public class JsonModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
                                ModelBindingContext bindingContext)
        {
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");
            if (bindingContext == null)
                throw new ArgumentNullException("bindingContext");

            var serializer = new DataContractJsonSerializer(bindingContext.ModelType);
            var ms = controllerContext.HttpContext.Request.InputStream;
            ms.Position = 0;
            return serializer.ReadObject(ms) ;
        }
    } 

}
