using PKInfo.Domain.Entity;
using System;
using System.IO;
using System.Reflection;

namespace PKInfo.Domain.UseCase.Self
{
    internal class SelfInfoInteractor
    {
        private static Assembly _executingAssembly;

        #region .ctors
        public SelfInfoInteractor() 
        { 
        }
        #endregion

        public SelfInfo Execute(string assemblyNameWithExt)
        {
            _executingAssembly ??= Assembly.UnsafeLoadFrom(assemblyNameWithExt);
            return new(GetName(), GetVersion(), GetBuildDateTime());
        }

        private static string GetName() =>
            _executingAssembly.GetCustomAttribute<AssemblyTitleAttribute>()
                ?.Title
                ?? GetSelfFilename();

        private static string GetSelfFilename() =>
            Path.GetFileNameWithoutExtension(_executingAssembly.CodeBase);

        private static string GetVersion() =>
            _executingAssembly.GetName().Version.ToString();

        private static DateTime GetBuildDateTime() =>
            File.GetLastAccessTime(_executingAssembly.Location);
    }
}
