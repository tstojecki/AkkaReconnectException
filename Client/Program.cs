using Akka.Actor;

namespace Client
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
            port = 0 # bound to a dynamic port assigned by the OS
            hostname = localhost
        }
    }
}";


        static void Main(string[] args)
        {
            using (var sys = ActorSystem.Create("ClientSystem", Config))
            {
                var client = sys.ActorOf<Client>();
                client.Tell("awake");

                sys.AwaitTermination();
            }
        }
    }
}
