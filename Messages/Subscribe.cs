using Akka.Actor;

namespace Messages
{
    public class Subscribe
    {
        public IActorRef Recipient { get; }

        public Subscribe(IActorRef recipient)
        {
            Recipient = recipient;
        }
    }
}
