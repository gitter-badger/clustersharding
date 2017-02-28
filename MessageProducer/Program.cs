using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Actors;
using Akka.Actor;
using Akka.Cluster.Sharding;

namespace MessageProducer
{
    class Program
    {
        // Random keys
        private static readonly IEnumerable<Guid> SampleKeys = Enumerable.Range(0, 10).Select(_ => Guid.NewGuid());

        static void Main(string[] args)
        {
            Console.Title = "Only hosts shardedregion proxy";

            using (var system = ActorSystem.Create("sample"))
            {
                var proxy = ClusterSharding.Get(system).StartProxy(
                    typeName: nameof(ProcessorActor),
                    role: null,
                    messageExtractor: new SampleMessageExtractor());


                foreach (var key in SampleKeys)
                {
                    for (var i = 0; i < 5; i++)
                    {
                        proxy.Tell(new SampleMessage(key, $"Message {i} from Key {key}"));
                    }
                }

                system.WhenTerminated.Wait();
            }
        }
    }
}
