using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comparitter.Compare.Exception
{
    public class CompareException : System.Exception
    {
        private static readonly string DefaultMessage = "An error occurred during the compare operation.";

        /// <summary>
        /// Creates a new <see cref="CompareException"/> with a default message.
        /// </summary>
        public CompareException() : base(DefaultMessage)
        { }

        /// <summary>
        /// Creates a new <see cref="CompareException"/> with a supplied message.
        /// </summary>
        /// <param name="message">Message describing the error.</param>
        public CompareException(string message) : base(message)
        { }

        /// <summary>
        /// Creates a new <see cref="CompareException"/> with a default message and a linked inner exception.
        /// </summary>
        /// <param name="innerException">A caught exception that is related to the error condition.</param>
        public CompareException(System.Exception innerException) : base (DefaultMessage, innerException)
        { }

        /// <summary>
        /// Creates a new <see cref="CompareException"/> with a supplied message and a linked inner exception.
        /// </summary>
        /// <param name="message">Message describing the error.</param>
        /// <param name="innerException">A caught exception that is related to the error condition.</param>
        public CompareException(string message, System.Exception innerException) : base (message, innerException)
        { }
    }
}
