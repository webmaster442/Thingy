using System;

namespace Thingy.API.Messages
{
    /// <summary>
    /// Message base
    /// </summary>
    public abstract class MessageBase
    {
        /// <summary>
        /// Sender Id
        /// </summary>
        public Guid Sender { get; }

        /// <summary>
        /// Create a new message
        /// </summary>
        /// <param name="sender">Sender Id</param>
        public MessageBase(Guid sender)
        {
            Sender = sender;
        }
    }
}
