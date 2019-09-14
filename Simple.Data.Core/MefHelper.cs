using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Simple.Data
{
    using System.Diagnostics;

    class MefHelper : Composer
    {
        public override T Compose<T>()
        {
            using (var container = CreateAppDomainContainer())
            {
                var exports = container.GetExports<T>().ToList();
                if (exports.Count == 1)
                {
                    return exports.Single().Value;
                }
            }

            //Iterate over addional MEF paths and load providers through composition
            foreach (var _addionalPath in _addionalPaths)
            {
                using (var container = CreateFolderContainer(_addionalPath))
                {
                    var exports = container.GetExports<T>().ToList();
                    if (exports.Count == 1)
                    {
                        return exports.Single().Value;
                    }
                }
            }

            using (var container = CreateFolderContainer())
            {
                var exports = container.GetExports<T>().ToList();
                if (exports.Count == 0) throw new SimpleDataException("No ADO Provider found.");
                if (exports.Count > 1) throw new SimpleDataException("Multiple ADO Providers found; specify provider name or remove unwanted assemblies.");
                return exports.Single().Value;
            }

        }

        public override T Compose<T>(string contractName)
        {
            try
            {
                using (var container = CreateAppDomainContainer())
                {
                    var exports = container.GetExports<T>(contractName).ToList();
                    if (exports.Count == 1)
                    {
                        return exports.Single().Value;
                    }
                }
                using (var container = CreateFolderContainer())
                {
                    var exports = container.GetExports<T>(contractName).ToList();
                    if (exports.Count == 0) throw new SimpleDataException(string.Format("No {0} Provider found.", contractName));
                    if (exports.Count > 1) throw new SimpleDataException("Multiple ADO Providers found; specify provider name or remove unwanted assemblies.");
                    return exports.Single().Value;
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                SimpleDataTraceSources.TraceSource.TraceEvent(TraceEventType.Error, SimpleDataTraceSources.GenericErrorMessageId,
                    "Compose failed: {0}", ex.Message);
                throw;
            }
        }

        public static T GetAdjacentComponent<T>(Type knownSiblingType)
        {
            using (var assemblyCatalog = new AssemblyCatalog(knownSiblingType.Assembly))
            {
                using (var container = new CompositionContainer(assemblyCatalog))
                {
                    return container.GetExportedValueOrDefault<T>();
                }
            }
        }

        private static CompositionContainer CreateFolderContainer(string dirpath = "")
        {
            var path = string.IsNullOrEmpty(dirpath)?GetSimpleDataAssemblyPath(): dirpath;

            var assemblyCatalog = new AssemblyCatalog(ThisAssembly);
            var aggregateCatalog = new AggregateCatalog(assemblyCatalog);

            var asmfiles = Directory.EnumerateFiles(path, "Simple.Data.*.dll");
            if(!asmfiles.Any())
            {
                asmfiles = Directory.EnumerateFiles(path, "Simple.Data.*.Core.dll");
            }
            foreach (string file in asmfiles)
            {
                var catalog = new AssemblyCatalog(file);
                aggregateCatalog.Catalogs.Add(catalog);
            }
            return new CompositionContainer(aggregateCatalog);
        }

        private static CompositionContainer CreateAppDomainContainer()
        {
            var aggregateCatalog = new AggregateCatalog();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Where(IsSimpleDataAssembly))
            {
                aggregateCatalog.Catalogs.Add(new AssemblyCatalog(assembly));
            }
            return new CompositionContainer(aggregateCatalog);
        }

        private static bool IsSimpleDataAssembly(Assembly assembly)
        {
            try
            {
                return assembly.GetFullName().StartsWith("Simple.Data.", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}
