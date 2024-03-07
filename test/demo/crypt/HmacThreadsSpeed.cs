using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using util;
using util.crypt;
using util.ext;

namespace demo.crypt
{
    public class HmacThreadsSpeedDemo : Test
    {
        public override void test()
        {
            //Environment.ProcessorCount.log();

            //singleThread("SHA1", 1024*1024*1024);
            //singleThread("SHA256", 1024 * 1024 * 1024);
            //singleThread("SHA512", 1024 * 1024 * 1024);

            multiThreadCacheHmac("SHA256", 10 * 1024 * 1024 * 1024L);

            //multiThread("SHA1", 1024 * 1024 * 1024);
            multiThread("SHA256", 10*1024 * 1024 * 1024L);
            //multiThread("SHA512", 10 * 1024 * 1024 * 1024L);
        }

        byte[] key = 32.aesKey();
        byte[] block = 4096.aesRnd();

        public void singleThread(string hash, long total)
        {
            var hmacName = $"HMAC{hash}";

            $"\r\nhmac:{hmacName}".msg();

            var mac = HMAC.Create(hmacName);
            mac.Key = key;
            
            int blk = 1000;
            long bat = total/(block.Length*blk);
            var ctr = new Counter { TotalSize = bat * blk * block.Length };
            ctr.TimeIsUp += (t) => t.FullInfo.status();
            ctr.trigger();
            "singleThread".evalTime(() =>
            {
                while (bat-- > 0)
                {
                    int count = blk;
                    while (count-- > 0)
                    {
                        var tag = mac.ComputeHash(block);
                    }
                    ctr.addSize(blk * block.Length);
                }
            });
            ctr.trigger();
        }

        public void multiThread(string hash, long total)
        {
            var hmacName = $"HMAC{hash}";

            $"\r\nhmac:{hmacName}".msg();

            int threadCount = Environment.ProcessorCount.atLeast(8);
            int blockPerThread = 50;
            int blockPerIo = threadCount * blockPerThread;
            long totalIoCount = total / (block.Length * blockPerIo);
            long sizePerIo = blockPerIo * block.Length;
            long sizePerThread = blockPerThread * block.Length;
            new
            { threadCount, blockPerThread,
                blockPerIo, totalIoCount,
                sizePerIo,
                sizePerThread
            }
            .json().msg();

            var ctr = new Counter { TotalSize = totalIoCount * blockPerIo * block.Length };
            ctr.TimeIsUp += (t) => t.FullInfo.status();
            ctr.trigger();
            "multiThread".evalTime(() =>
            {
                var tasks = new List<Task>();
                while (totalIoCount-- > 0)
                {
                    tasks.Clear();
                    
                    for(var t = 0;t < threadCount; t++)
                    {
                        var task = Task.Run(()=> 
                        {
                            using (var mac = HMAC.Create(hmacName))
                            {
                                mac.Key = key;
                                for (int i = 0; i < blockPerThread; i++)
                                {
                                    var tag = mac.ComputeHash(block);
                                }
                            }
                        });
                        tasks.Add(task);
                    }
                    Task.WaitAll(tasks.ToArray());

                    ctr.addSize(blockPerIo * block.Length);
                }
            });
            ctr.trigger();
        }

        public void multiThreadCacheHmac(string hash, long total)
        {
            var hmacName = $"HMAC{hash}";

            $"\r\nhmac:{hmacName}".msg();

            int threadCount = Environment.ProcessorCount.atLeast(8);
            int blockPerThread = 50;
            int blockPerIo = threadCount * blockPerThread;
            long totalIoCount = total / (block.Length * blockPerIo);
            long sizePerIo = blockPerIo * block.Length;
            long sizePerThread = blockPerThread * block.Length;
            new
            {
                threadCount,
                blockPerThread,
                blockPerIo,
                totalIoCount,
                sizePerIo,
                sizePerThread
            }
            .json().msg();

            var macs = new HMAC[100];

            var ctr = new Counter { TotalSize = totalIoCount * blockPerIo * block.Length };
            ctr.TimeIsUp += (t) => t.FullInfo.status();
            ctr.trigger();
            "multiThreadCacheHmac".evalTime(() =>
            {
                var tasks = new List<Task>();
                while (totalIoCount-- > 0)
                {
                    tasks.Clear();

                    for (var t = 0; t < threadCount; t++)
                    {
                        var task = Task.Run(() =>
                        {
                            var tid = Thread.CurrentThread.ManagedThreadId;
                            var mac = macs[tid];
                            if (mac == null)
                            {
                                mac = HMAC.Create(hmacName);
                                mac.Key = key;
                                macs[tid] = mac;
                            }

                            for (int i = 0; i < blockPerThread; i++)
                            {
                                var tag = mac.ComputeHash(block);
                            }
                        });
                        tasks.Add(task);
                    }
                    Task.WaitAll(tasks.ToArray());

                    ctr.addSize(blockPerIo * block.Length);
                }
            });
            ctr.trigger();
        }
    }
}
