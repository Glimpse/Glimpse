using System;

namespace Glimpse.Core.Extensibility
{
    public class MessageBase
    {
        private readonly Guid id = Guid.NewGuid();

        public Guid Id 
        {
            get { return id; }
        } 
    }
}