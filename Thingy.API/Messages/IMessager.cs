using System;

namespace Thingy.API.Messages
{
    public interface IMessager
    {
        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="target">target type</param>
        /// <param name="msg">Message to send</param>
        void SendMessage(Type target, MessageBase msg);
        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="target">target type</param>
        /// <param name="id">target id</param>
        /// <param name="msg">Message to send</param>
        void SendMessage(Type target, Guid id, MessageBase msg);
        /// <summary>
        /// Send a message
        /// </summary>
        /// <param name="targetId">target id</param>
        /// <param name="msg">Message to send</param>
        void SendMessage(Guid targetId, MessageBase msg);
    }
}
