using System;
using Thingy.API.Messages;
using Msg = AppLib.MVVM.MessageHandler.Messager;

namespace Thingy.Implementation
{
    public class Messager : IMessager
    {
        public void SendMessage(Type target, MessageBase msg)
        {
            Msg.Instance.SendMessage(target, msg);
        }

        public void SendMessage(Type target, Guid id, MessageBase msg)
        {
            Msg.Instance.SendMessage(target, id, msg);
        }

        public void SendMessage(Guid targetId, MessageBase msg)
        {
            Msg.Instance.SendMessage(targetId, msg);
        }
    }
}
