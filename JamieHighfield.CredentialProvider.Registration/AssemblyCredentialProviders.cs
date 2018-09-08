using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace JamieHighfield.CredentialProvider.Registration
{
    public static class AssemblyCredentialProviders
    {
        #region Variables



        #endregion

        #region Properties



        #endregion

        #region Methods

        public static AssemblyCredentialProviderCollection GetCredentialProviders(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            AssemblyCredentialProviderCollection assemblyCredentialProviders = new AssemblyCredentialProviderCollection();

            foreach (AssemblyCredentialProvider assemblyCredentialProvider in assembly.GetTypes()
                .Where((type) =>
                {
                    return
                        (typeof(CredentialProviderBase).IsAssignableFrom(type)
                        && (type.GetCustomAttribute<ComVisibleAttribute>() != null)
                        && (type.GetCustomAttribute<GuidAttribute>() != null)
                        && (type.GetCustomAttribute<ProgIdAttribute>() != null));
                })
                .Select((type) =>
                {
                    GuidAttribute comGuid = type.GetCustomAttribute<GuidAttribute>();
                    ProgIdAttribute comName = type.GetCustomAttribute<ProgIdAttribute>();

                    return new AssemblyCredentialProvider(Guid.Parse(comGuid.Value), comName.Value, IsComRegistered(Guid.Parse(comGuid.Value), comName.Value), IsCredentialProviderRegistered(Guid.Parse(comGuid.Value), comName.Value));
                }))
            {
                assemblyCredentialProviders.Add(assemblyCredentialProvider);
            }

            return assemblyCredentialProviders;
        }

        private static bool IsComRegistered(Guid comGuid, string comName)
        {
            if (comName == null)
            {
                throw new ArgumentNullException(nameof(comName));
            }

            return ((RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64)
                .OpenSubKey(@"CLSID\{" + comGuid.ToString() + @"}\ProgId")
                ?.GetValue(null)
                ?.ToString() ?? string.Empty).ToLower() == comName.ToLower());
        }

        private static bool IsCredentialProviderRegistered(Guid comGuid, string comName)
        {
            if (comName == null)
            {
                throw new ArgumentNullException(nameof(comName));
            }

            return ((RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                .OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers\{" + comGuid.ToString() + @"}")
                ?.GetValue(null)
                ?.ToString() ?? string.Empty).ToLower() == comName.ToLower());
        }

        public static void RegisterCredentialProviders(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            AssemblyCredentialProviderCollection assemblyCredentialProviders = new AssemblyCredentialProviderCollection();

            foreach (AssemblyCredentialProvider assemblyCredentialProvider in assembly.GetTypes()
                .Where((type) =>
                {
                    return
                        (typeof(CredentialProviderBase).IsAssignableFrom(type)
                        && (type.GetCustomAttribute<ComVisibleAttribute>() != null)
                        && (type.GetCustomAttribute<GuidAttribute>() != null)
                        && (type.GetCustomAttribute<ProgIdAttribute>() != null));
                })
                .Select((type) =>
                {
                    GuidAttribute comGuid = type.GetCustomAttribute<GuidAttribute>();
                    ProgIdAttribute comName = type.GetCustomAttribute<ProgIdAttribute>();

                    return new AssemblyCredentialProvider(Guid.Parse(comGuid.Value), comName.Value, IsComRegistered(Guid.Parse(comGuid.Value), comName.Value), IsCredentialProviderRegistered(Guid.Parse(comGuid.Value), comName.Value));
                }))
            {
                string frameworkInstallationPath = RuntimeEnvironment.GetRuntimeDirectory();

                ProcessStartInfo processStartInfo = new ProcessStartInfo(
                    Path.Combine(frameworkInstallationPath, "regasm.exe"), "\"" + assembly.Location + "\"");

                processStartInfo.CreateNoWindow = false;

                Process.Start(processStartInfo).WaitForExit();

                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                    .OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers", true)
                    .DeleteSubKey("{" + assemblyCredentialProvider.ComGuid.ToString() + "}", false);

                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                    .OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers", true)
                    .CreateSubKey("{" + assemblyCredentialProvider.ComGuid.ToString() + "}")
                    .SetValue(null, assemblyCredentialProvider.ComName);

                assemblyCredentialProviders.Add(assemblyCredentialProvider);
            }
        }

        public static void UnregisterCredentialProviders(Assembly assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            AssemblyCredentialProviderCollection assemblyCredentialProviders = new AssemblyCredentialProviderCollection();

            foreach (AssemblyCredentialProvider assemblyCredentialProvider in assembly.GetTypes()
                .Where((type) =>
                {
                    return
                        (typeof(CredentialProviderBase).IsAssignableFrom(type)
                        && (type.GetCustomAttribute<ComVisibleAttribute>() != null)
                        && (type.GetCustomAttribute<GuidAttribute>() != null)
                        && (type.GetCustomAttribute<ProgIdAttribute>() != null));
                })
                .Select((type) =>
                {
                    GuidAttribute comGuid = type.GetCustomAttribute<GuidAttribute>();
                    ProgIdAttribute comName = type.GetCustomAttribute<ProgIdAttribute>();

                    return new AssemblyCredentialProvider(Guid.Parse(comGuid.Value), comName.Value, IsComRegistered(Guid.Parse(comGuid.Value), comName.Value), IsCredentialProviderRegistered(Guid.Parse(comGuid.Value), comName.Value));
                }))
            {
                string frameworkInstallationPath = RuntimeEnvironment.GetRuntimeDirectory();

                ProcessStartInfo processStartInfo = new ProcessStartInfo(
                    Path.Combine(frameworkInstallationPath, "regasm.exe"), "/unregister \"" + assembly.Location + "\"");

                processStartInfo.CreateNoWindow = false;

                Process.Start(processStartInfo).WaitForExit();

                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64)
                    .OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\Credential Providers", true)
                    .DeleteSubKey("{" + assemblyCredentialProvider.ComGuid.ToString() + "}", false);

                assemblyCredentialProviders.Add(assemblyCredentialProvider);
            }
        }

        #endregion
    }
}