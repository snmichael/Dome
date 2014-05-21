using System.Web.Mvc;

namespace Demo.Framework.Web.Mvc.ModelBinders
{
    public class TrimModelBinder : DefaultModelBinder
    {
        protected override void SetProperty(ControllerContext controllerContext,
          ModelBindingContext bindingContext,
          System.ComponentModel.PropertyDescriptor propertyDescriptor, object value)
        {
            if (propertyDescriptor.PropertyType == typeof(string))
            {
                var stringValue = (string)value;
                if (!string.IsNullOrEmpty(stringValue))
                    stringValue = stringValue.Trim();

                value = stringValue;
            }

            base.SetProperty(controllerContext, bindingContext,
                                propertyDescriptor, value);
        }

    }

    public class StringTrimModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = base.BindModel(controllerContext, bindingContext);
            if (value is string) return (value as string).Trim();
            return value;
        }
    }
}