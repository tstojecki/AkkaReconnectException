using System;
using System.Collections.Generic;
using Akka.Actor;
using Messages;

namespace Server
{
    class Host : ReceiveActor
    {
        private readonly HashSet<IActorRef> _clients = new HashSet<IActorRef>();
        private readonly List<string> _bufferedMessages = new List<string>(); 

        public Host()
        {
            Receive<string>(item =>
            {
                _bufferedMessages.Add(item);

                foreach (var client in _clients)
                    client.Tell(new Imported(item));
            });

            Receive<Subscribe>(subscribe =>
            {
                var sender = subscribe.Recipient;

                Console.WriteLine($"{sender} subscribed");
                
                if (!_clients.Contains(sender))
                    _clients.Add(sender);

                Context.Watch(sender);
                sender.Tell(new Subscription(Self, _bufferedMessages));
            });

            Receive<Terminated>(t =>
            {
                Console.WriteLine($"{t.ActorRef} terminated");

                if (_clients.Contains(t.ActorRef))
                    _clients.Remove(t.ActorRef);

                Context.Unwatch(t.ActorRef);
            });
        }
    }
}
