using System;

namespace Tristan.Core.Models
{
    public abstract class Error
    {
        public string Source { get; }

        protected Error()
            : this(string.Empty)
        {
        }

        private Error(string source)
        {
            Source = source;
        }


        public sealed class CopyFailed : Error
        {
            public CopyFailed(string source)
                : base(source)
            {
            }
        }

        public sealed class SendFailed : Error
        {
            public SendFailed(string source)
                : base(source)
            {
            }
        }

        public sealed class DeleteFailed : Error
        {
            public DeleteFailed(string source)
                : base(source)
            {
            }
        }

        public sealed class Exceptional : Error
        {
            public Exception Exception { get; }

            public Exceptional(Exception exception)
            {
                Exception = exception;
            }
        }
    }
}