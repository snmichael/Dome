using System;
using System.Web.Mvc;
using Demo.Framework.Web.Mvc.Provider;

namespace Demo.Framework.Web.Mvc.Extensions{


    public class CustomDataAnnotationsModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {

        protected override System.ComponentModel.ICustomTypeDescriptor GetTypeDescriptor(Type type)
        {

            var descriptor = TypeDescriptorHelper.Get(type);

            if (descriptor == null)
            {

                descriptor = base.GetTypeDescriptor(type);

            }

            return descriptor;

        }

    }
}