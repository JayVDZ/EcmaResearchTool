using System.Reflection;

namespace EcmaResearchTool
{
    internal static class ManagementAgentCapabilities
    {
        internal static void Get(Type type, Assembly library)
        {
            var interfaceName = "IMAExtensible2GetCapabilitiesEx";
            if (!type.GetInterfaces().Any(t => t.Name == interfaceName))
                return;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{type.FullName} implements {interfaceName}!");
            Console.ResetColor();

            var managementAgent = Activator.CreateInstance(type);
            if (managementAgent == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Couldn't create an instance that inherits type {interfaceName}");
                Console.ResetColor();
                return;
            }

            // would need passing in config params, so not sure on initial value during discovery phase
            //var capabilitiesMethodName = "GetCapabilitiesEx";
            //Console.WriteLine($"Invoking {capabilitiesMethodName}...");

            // get the MACapabilities object from the MA
            var propertyName = "Capabilities";
            var capabilityProperty = managementAgent.GetType().GetProperty(propertyName);
            if (capabilityProperty == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Couldn't get property't {propertyName}");
                Console.ResetColor();
                return;
            }

            object? capabilities = capabilityProperty.GetValue(managementAgent);
            var x = 1;
        }
    }
}
