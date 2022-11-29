using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Reflection;

namespace EcmaResearchTool
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the ECMA Research Tool...");

            // loads all the dlls from a given folder and identifies the management agent 
            if (args.Length != 1)
            {
                Console.WriteLine("No dll folder path specified as argument. Quitting.");
                return;
            }

            // load each dll into memory
            List<Assembly> assemblies = new List<Assembly>();
            foreach (var dll in Directory.GetFiles(args[0]))
                assemblies.Add(Assembly.LoadFrom(dll));

            // enumerate the assemblies and find which one is the ECMA 2.x management agent
            // and which one contains the MIM libraries
            Assembly managementAgentAssembly = null;
            Assembly metaDirectoryAssembly = null;
            Type[] managementAgentInterfaces = null;
            Type managementAgentType = null;
            List<string> ecmaInterfaces = new List<string>()
            {
                "IMAExtensible2CallExport",
                "IMAExtensible2CallImport",
                "IMAExtensible2FileExport",
                "IMAExtensible2FileImport",
                "IMAExtensible2GetCapabilities",
                "IMAExtensible2GetCapabilitiesEx",
                "IMAExtensible2GetHierarchy",
                "IMAExtensible2GetParameters",
                "IMAExtensible2GetParametersEx",
                "IMAExtensible2GetPartitions",
                "IMAExtensible2GetSchema",
                "IMAExtensible2Password"
            };

            foreach (var assembly in assemblies)
            {
                if (assembly.ExportedTypes.Any(q => q.Namespace == "Microsoft.MetadirectoryServices"))
                {
                    metaDirectoryAssembly = assembly;
                    Console.WriteLine($"Found the library: {assembly.FullName}");
                }

                foreach (var type in assembly.ExportedTypes)
                {
                    var typeInterfaces = type.GetInterfaces();
                    if (typeInterfaces.Any(i => ecmaInterfaces.Contains(i.Name)))
                    {
                        managementAgentInterfaces = typeInterfaces;
                        managementAgentAssembly = assembly;
                        managementAgentType = type;

                        Console.WriteLine($"Found the Management Agent dll: {assembly.FullName}");
                        Console.WriteLine($"The Management Agent class is: {type.Name}");
                        Console.WriteLine("It implements the following ECMA interfaces:");
                        foreach (var implementedInterface in managementAgentInterfaces.Where(i => ecmaInterfaces.Contains(i.Name)))
                            Console.WriteLine($"\t{implementedInterface}");

                        break;
                    }
                }
            }

            if (managementAgentAssembly == null || managementAgentInterfaces == null || managementAgentType == null)
            {
                Console.Error.WriteLine("No ECMA 2.0 Management Agent found in the folder! Exiting");
                return;
            }

            if (metaDirectoryAssembly == null)
            {
                Console.Error.WriteLine("No Microsoft.MetadirectoryServices type found in folder dlls! Exiting");
                return;
            }

            // now inspect management agent parameters
            if (managementAgentInterfaces.Any(i => i.Name == "IMAExtensible2GetParametersEx"))
            {
                ManagementAgentParametersEx.Get(managementAgentType, metaDirectoryAssembly);
            }

            if (managementAgentInterfaces.Any(i => i.Name == "IMAExtensible2GetParameters"))
            {
                ManagementAgentParameters.Get(managementAgentType, metaDirectoryAssembly);
            }

            return;
        }
    }
}