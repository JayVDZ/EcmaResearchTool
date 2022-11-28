using System.Reflection;

namespace EcmaResearchTool
{
    internal class ManagementAgentParameters
    {
        internal static void Get(Type managementAgentType, Assembly library)
        {
            var interfaceName = "IMAExtensible2GetParameters";
            if (!managementAgentType.GetInterfaces().Any(t => t.Name == interfaceName))
                return;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{managementAgentType.FullName} implements {interfaceName}!");
            Console.ResetColor();

            var managementAgent = Activator.CreateInstance(managementAgentType);
            if (managementAgent == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Couldn't create an instance that inherits type {interfaceName}");
                Console.ResetColor();
                return;
            }

            // instantiate required params:
            // ConfigParameterKeyedCollection
            var configParameterKeyedCollectionType = library.ExportedTypes.Single(q => q.Name == "ConfigParameterKeyedCollection");
            var configParameterKeyedCollection = Activator.CreateInstance(configParameterKeyedCollectionType);

            // Generic SQL MA specifics:
            // add required config params
            //var configParameterType = library.ExportedTypes.Single(q => q.Name == "ConfigParameter");
            //var dnsConfigParamArgs = new object[] { "DSN File", "QzpcdGVtcFxETlNzXEdlbmVyaWNTUUwuZHNu", false };
            //var dsnConfigParam = Activator.CreateInstance(configParameterType, dnsConfigParamArgs);
            //var addConfigParamMethod = configParameterKeyedCollectionType.GetMethod("Add");
            //var addConfigParamMethodArgs = new object[] { dsnConfigParam };
            //addConfigParamMethod.Invoke(configParameterKeyedCollection, addConfigParamMethodArgs);

            // ConfigParameterPage
            var configParameterPageType = library.ExportedTypes.Single(q => q.Name == "ConfigParameterPage");
            var globalConfigParameterPage = Enum.Parse(configParameterPageType, "Global");

            // call GetConfigParametersEx()
            var getConfigParamsExMethod = managementAgentType.GetMethod("GetConfigParameters");
            var args = new object[] { configParameterKeyedCollection, globalConfigParameterPage };
            var results = getConfigParamsExMethod.Invoke(managementAgent, args);


            var x = 1;
        }
    }
}
