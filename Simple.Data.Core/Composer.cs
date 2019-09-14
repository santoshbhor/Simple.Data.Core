﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;

namespace Simple.Data
{
    public abstract class Composer
    {
        protected static readonly Assembly ThisAssembly = typeof (Composer).Assembly;
        private static Composer _composer = new MefHelper();
        protected static IList<string> _addionalPaths = new List<string>();

        public static Composer Default
        {
            get { return _composer; }
        }

        public abstract T Compose<T>();

        public abstract T Compose<T>(string contractName);

        internal static void SetDefault(Composer composer)
        {
            _composer = composer;
        }

        public static string GetSimpleDataAssemblyPath()
        {
            var path = ThisAssembly.CodeBase.Replace("file:///", "").Replace("file://", "//");
            path = Path.GetDirectoryName(path);
            if (path == null) throw new ArgumentException("Unrecognised assemblyFile.");
            if (!Path.IsPathRooted(path))
            {
                path = Path.DirectorySeparatorChar + path;
            }
            return path;
        }

        public static void RegisterAddionalCompositionPath(string directoryPath)
        {
            _addionalPaths.Add(directoryPath);
        }

        public static bool TryLoadAssembly(string assemblyFile, out Assembly assembly)
        {
            if (assemblyFile == null) throw new ArgumentNullException("assemblyFile");
            if (assemblyFile.Length == 0) throw new ArgumentException("Assembly file name is empty.", "assemblyFile");
            try
            {
                assembly = Assembly.ReflectionOnlyLoadFrom(assemblyFile);
                return true;
            }
            catch(System.PlatformNotSupportedException)
            {
                assembly = Assembly.LoadFrom(assemblyFile);
                return true;
            }
            catch (FileNotFoundException)
            {
                assembly = null;
                return false;
            }
            catch(FileLoadException)
            {
                assembly = null;
                return false;
            }
            catch (BadImageFormatException)
            {
                assembly = null;
                return false;
            }
            catch(SecurityException)
            {
                assembly = null;
                return false;
            }
            catch (PathTooLongException)
            {
                assembly = null;
                return false;
            }
        }
    }
}
