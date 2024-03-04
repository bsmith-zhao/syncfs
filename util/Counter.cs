using System;
using util.ext;

namespace util
{
    public class Counter
    {
        public event Action<Counter> TimeIsUp;

        public long ExpireTime = 300 * TimeEx.TicksPerMS;
        public int TotalCount;
        public int FinishCount;
        public long TotalSize;
        public long FinishSize;

        long lastSize;
        long spanSize;
        long spanTime;
        long beginTime;
        long lastTime;

        public Counter()
        {
            beginTime = DateTime.UtcNow.Ticks;
            lastTime = beginTime;
        }

        long getSpeed(long size, long time)
            => (time < ExpireTime) ? 0 : (size * 1000) / (time / TimeEx.TicksPerMS);

        long SpeedValue => getSpeed(spanSize, spanTime);
        long AvgSpeedValue => getSpeed(lastSize, lastTime - beginTime);
        int SizePercentValue => FinishSize.r100(TotalSize);
        int CountPercentValue => TotalCount <= 0 ? 0 : FinishCount * 100 / TotalCount;

        public string Speed => $"{SpeedValue.byteSize(2)}/S";
        public string AvgSpeed => $"{AvgSpeedValue.byteSize(2)}/S";
        public string Duration => (DateTime.UtcNow - new DateTime(beginTime)).ToString(@"hh\:mm\:ss\.ff");
        public string SizePair => $"{FinishSize.byteSize(2)}/{TotalSize.byteSize(2)}";
        public string CountPair => $"{FinishCount}/{TotalCount}";
        public string SizePercent => $"{SizePercentValue}%";
        public string CountPercent => $"{CountPercentValue}%";

        public string CountInfo => $"{CountPair}, {CountPercent}";
        public string FullInfo => $"{SizePercent}, {AvgSpeed}, {CountPair}, {SizePair}, {Duration}";
        public string ShortInfo => $"{SizePair}, {CountPair}, {CountPercent}";
        public string SpeedInfo => $"{Speed}, {SizePair}, {SizePercent}";

        public void update() => add(0, 0);

        public void addCount(int count = 1)
        {
            add(count, 0);
        }

        public void addSize(long size)
        {
            add(0, size);
        }

        public void add(int count, long size)
        {
            FinishCount += count;
            FinishSize += size;

            long now = DateTime.UtcNow.Ticks;
            if (now - lastTime > ExpireTime)
            {
                spanSize = FinishSize - lastSize;
                spanTime = now - lastTime;

                lastTime = now;
                lastSize = FinishSize;

                trigger();
            }
        }

        public void trigger()
        {
            TimeIsUp?.Invoke(this);
        }
    }
}
