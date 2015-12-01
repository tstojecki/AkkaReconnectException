using System.Collections.Generic;
using Akka.Actor;

namespace Messages
{
    public class Subscription
    {
        public IActorRef Server { get; }

        public List<string> Items { get; }

        public Subscription(IActorRef server, List<string> items)
        {
            Server = server;
            Items = items;
        }
    }
}