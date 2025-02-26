using PKInfo.Data.Implementation.Reporter.PlainTextReporter.PlainTextErrorReporter;
using PKInfo.Domain.Entity;
using System;

namespace PKInfo.Data.Implementation.Reporter
{
    internal abstract class BaseReporter 
    {
        protected readonly RootError __rootError;

        #region .ctors
        protected BaseReporter(RootError rootError)
        {
            __rootError = rootError;
        }
        #endregion

        protected abstract T Report<T>() where T: class;

        public static T BuildReport<T>(RootError rootError) where T : class =>
            GetReporter(typeof(T), rootError).Report<T>();

        private static BaseReporter GetReporter(Type type, RootError rootError)
            => type switch
            {
                Type t when t == typeof(string) => new PlainTextErrorReporter(rootError),
                _ => throw new Exception()
            };
    }
}
