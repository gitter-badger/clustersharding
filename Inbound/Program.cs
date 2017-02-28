using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Actors;
using Akka.Actor;
using Akka.Cluster.Sharding;

namespace Inbound
{



    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Can host shardedregion actors";
            using (var system = ActorSystem.Create("sample"))
            {
                var shardRegion = ClusterSharding.Get(system).Start(
                        typeName: nameof(ProcessorActor),
                        entityProps: Props.Create<ProcessorActor>(),
                        settings: ClusterShardingSettings.Create(system),
                        messageExtractor: new SampleMessageExtractor());

                system.WhenTerminated.Wait();
            }
        }

        #region Commented
        /*
         
         var dictionary = new Dictionary<int, int>();
            for (var i = 0; i < 99; i++)
            {
                var key = Math.Abs(Guid.NewGuid().GetHashCode()%6);

                if (!dictionary.ContainsKey(key))
                    dictionary[key] = 1;
                else
                    dictionary[key]++;

            }

            foreach(var kv in dictionary)
                Console.WriteLine($"{kv.Key} = {kv.Value}");
    */

        #endregion
    }

   
}
