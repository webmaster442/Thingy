using System;
using System.Collections.Generic;

namespace Thingy.API.Messages
{
    public class HandleFileMessage : MessageBase
    {
        public HandleFileMessage(Guid sender, string file) : base(sender)
        {
            Files = new List<string>(1)
            {
                file
            };
        }

        public HandleFileMessage(Guid sender, params string[] files) : base(sender)
        {
            Files = new List<string>(files);
        }

        public HandleFileMessage(Guid sender, IEnumerable<string> files) : base(sender)
        {
            Files = new List<string>(files);
        }

        public List<string> Files { get; private set; }
    }
}
