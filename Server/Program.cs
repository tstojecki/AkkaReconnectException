using System.Timers;
using Akka.Actor;

namespace Server
{
    static class Program
    {
        const string Config = @"
akka {  
    actor {
        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
    }
    remote {
        helios.tcp {
            port = 8081 #bound to a specific port
            hostname = localhost
        }
    }
}";


        static void Main(string[] args)
        {
            using (var sys = ActorSystem.Create("ServerSystem", Config))
            {
                var counter = 0;
                var timer = new Timer(2000);
                var host = sys.ActorOf<Host>("Host");

                timer.Elapsed += (_, __) => host.Tell($"{counter++}");
                timer.Start();

                sys.AwaitTermination();
            }
        }
    }
}
