using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster.Sharding;

namespace Actors
{
    public class ProcessorActor : ReceiveActor
    {
        public ProcessorActor()
        {
            Receive<SampleMessage>(msg => Console.WriteLine(msg.Key));
        }
    }

    public class SampleMessage
    {
        public Guid Key { get; }
        public string Message { get; }

        public SampleMessage(Guid key, string message)
        {
            Key = key;
            Message = message;
        }
    }

    public sealed class Envelope<T> where T : class
    {
        public int ShardId { get; }
        public int EntityId { get; }
        public T Message { get; }

        public Envelope(int shardId, int entityId, T message)
        {
            ShardId = shardId;
            EntityId = entityId;
            Message = message;
        }
    }

    public sealed class SampleMessageExtractor : IMessageExtractor
    {
        public string EntityId(object message)
        {
            var dm = message as SampleMessage;
            if (dm == null)
                throw new Exception();

            return Math.Abs(dm.Key.GetHashCode()).ToString();
        }

        public object EntityMessage(object message)
        {
            var dm = message as SampleMessage;
            if (dm == null)
                throw new Exception();

            return dm.Message;
        }

        public string ShardId(object message)
        {
            var dm = message as SampleMessage;
            if (dm == null)
                throw new Exception();

            return Math.Abs(dm.Key.GetHashCode() % 5).ToString();
        }
    }
}
