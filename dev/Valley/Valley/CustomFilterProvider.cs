using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace Valley
{
    public class CustomFilterProvider : FilterAttributeFilterProvider
    {
        private readonly IUnityContainer container;

        public CustomFilterProvider(IUnityContainer container)
        {
            this.container = container;
        }

        protected override IEnumerable<FilterAttribute> GetControllerAttributes(
                    ControllerContext controllerContext,
                    ActionDescriptor actionDescriptor)
        {

            var attributes = base.GetControllerAttributes(controllerContext, actionDescriptor);
            foreach (var attribute in attributes)
            {
                container.BuildUp(attribute.GetType(), attribute);
            }

            return attributes;
        }

        protected override IEnumerable<FilterAttribute> GetActionAttributes(
                    ControllerContext controllerContext,
                    ActionDescriptor actionDescriptor)
        {

            var attributes = base.GetActionAttributes(controllerContext, actionDescriptor);
            foreach (var attribute in attributes)
            {
                container.BuildUp(attribute.GetType(), attribute);
            }

            return attributes;
        }
        public override IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
        {
            var filters = base.GetFilters(controllerContext, actionDescriptor);

            var enumerable = filters as IList<Filter> ?? filters.ToList();

            foreach (var filter in enumerable)
            {
                container.BuildUp(filter.Instance.GetType(), filter.Instance);
            }

            return enumerable;
        }
    }
}
