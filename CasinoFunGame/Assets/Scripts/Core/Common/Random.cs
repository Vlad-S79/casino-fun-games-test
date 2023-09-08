using System;

namespace Core.Common
{
    public class Random
    {
        private long _seed;

        public Random() : this(DateTime.Now.Ticks) { }

        public Random(long seed)
        {
            SetSeed(seed);
        }

        public void SetSeed(long seed)
        {
            _seed = seed;
        }

        public int NextInt()
        {
            _seed ^= _seed << 21;
            _seed ^= _seed >> 35;
            _seed ^= _seed << 4;
            return (int)_seed;
        }

        public int NextInt(int max)
            => Math.Abs(NextInt() % (max + 1)) * Math.Sign(max);
        public int NextInt(int min, int max)
            => (int) (Math.Abs(NextInt() % (uint) (max - min + 1)) + min);

        public long NextLong()
            => ((long)NextInt() << 32) + NextInt();

        public double NextDouble()
            => (NextInt() & 0x7FFFFFFF) / ((double)0x7FFFFFFF + 1);

        public byte NextByte() => (byte)NextInt();

        public sbyte NextSbyte() => (sbyte)NextInt();
    }
}