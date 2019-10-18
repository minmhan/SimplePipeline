using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePipeLine
{
    class Program
    {
        public static BlockingCollection<int> queue = new BlockingCollection<int>(10);
        public static BlockingCollection<int> procQueue = new BlockingCollection<int>(10);
        BlockingCollection<int> transQueue = new BlockingCollection<int>(10);
        BlockingCollection<int> taggingQueue = new BlockingCollection<int>(10);

        static void Main(string[] args)
        {
            var f = new TaskFactory(TaskCreationOptions.LongRunning, TaskContinuationOptions.None);
            var t1 = f.StartNew(() => Read());
            var t2 = f.StartNew(() => Process());
            var t3 = f.StartNew(() => Display());

            Task.WaitAll(t1, t2, t3);
        }

        public static async Task Read()
        {
            var count = 0;
            while (true)
            {
                queue.Add(count++);
                await Task.Delay(1);
            }
        }

        public static async Task Process()
        {
            var random = new Random();
            while (true)
            {
                var i = queue.Take();
                procQueue.Add(i + 1);
                var r = random.Next(1, 3000);
                await Task.Delay(r);
            }
        }

        public static void Display()
        {
            while (true)
            {
                var i = procQueue.Take();
                Console.WriteLine(i);
            }
        }

        public static async Task FuncAsync()
        {
            await Task.Delay(10);
        }
    }
}
