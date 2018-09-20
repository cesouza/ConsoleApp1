﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.Serialization;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("teste");
            PublishAsync().Wait();
            Consume();
        }
   
        static async Task PublishAsync()
        {
            var config = new Dictionary<string, object>
            {
                 { "bootstrap.servers", "localhost:9092" }
            };


            using (var producer = new Producer<Null, string>(config, null, new StringSerializer(Encoding.UTF8)))
            {
                await producer.ProduceAsync("topic", null, "fiap");
            }
        }

        static void Consume()
        {
            var config = new Dictionary<string, object> {
{ "group.id", "test-consumer-group" },
{ "bootstrap.servers", "localhost:9092" },
{ "auto.commit.interval.ms", 5000 },
{ "auto.offset.reset", "earliest" }}; // latest
            using (var consumer = new Consumer<Null, string>(
            config,
            null,
            new StringDeserializer(Encoding.UTF8)))
            {
                consumer.OnMessage += (_, msg) => Console.WriteLine(msg.Value);
                consumer.Subscribe("topic");
                while (true) { consumer.Poll(TimeSpan.FromMilliseconds(100)); }
            }
        }
    }
}