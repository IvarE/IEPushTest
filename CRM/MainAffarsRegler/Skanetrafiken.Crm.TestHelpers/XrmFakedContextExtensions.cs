using FakeXrmEasy;
using Microsoft.Xrm.Sdk;

namespace Skanetrafiken.Crm.TestHelpers
{
    public static class XrmFakedContextExtensions
    {
        public static XrmFakedPluginExecutionContext GetFakePluginExecutionContext(this XrmFakedContext context, Entity target, string message, Entity preImage = null, Entity postImage = null)
        {
            var pluginContext = context.GetDefaultPluginContext();
            pluginContext.MessageName = message;
            pluginContext.InputParameters.Add("Target", target);

            if (preImage is { })
            {
                pluginContext.PreEntityImages.Add("image", preImage);
            }

            if (postImage is { })
            {
                pluginContext.PostEntityImages.Add("image", postImage);
            }

            return pluginContext;
        }
    }
}
