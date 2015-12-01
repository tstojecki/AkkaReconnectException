using System;
using Akka.Actor;
using Messages;

namespace Client
{
    class Client : ReceiveActor
    {
        private readonly ActorSelection _serverSelection;
        private IActorRef _server;

        public Client()
        {
            _serverSelection = Context.ActorSelection("akka.tcp://ServerSystem@localhost:8081/user/Host");

            Become(Connecting);
        }

        private void Connecting()
        {
            Receive<Subscription>(subscription =>
            {
                Become(Connected);
                Console.WriteLine("Connection established");

                _server = subscription.Server;
                Context.Watch(_server);

                Console.WriteLine("Buffered items:");
                subscription.Items.ForEach(Console.WriteLine);
            });

            Receive<Status.Failure>(_ =>
            {
                Console.WriteLine("Connection failed");
                Subscribe();
            });

            Subscribe();
        }

        private void Connected()
        {
            Receive<Imported>(imported => Console.WriteLine($"Imported new item: {imported.Item}"));
            Receive<Terminated>(terminated =>
            {
                Console.WriteLine("Connection terminated");

                Context.Unwatch(_server);
                Become(Connecting);
            });
        }

        private void Subscribe()
            => _serverSelection.Ask<Subscription>(new Subscribe(Self), TimeSpan.FromSeconds(3)).PipeTo(Self);
    }
}
