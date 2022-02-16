using System;
using System.Runtime.Serialization;

namespace Loopover.Holders
{
    [Serializable]
    [DataContract]
    readonly struct Stat
    {
        [DataMember]
        public readonly int startX;
        [DataMember]
        public readonly int startY;

        [DataMember]
        public readonly string moves;
        [DataMember]
        public readonly int[] scramble;

        [DataMember]
        public readonly TimeSpan time;

        [DataMember]
        public readonly int numMoves;

        [DataMember]
        public readonly DateTime date;

        public Stat(int x, int y, string mov, int[] scr, TimeSpan tim, int numoves)
        {
            startX = x;
            startY = y;
            moves = mov;
            scramble = scr;
            time = tim;
            numMoves = numoves;
            date = DateTime.Now;
        }

        public bool IsInvalid() => numMoves < 0;

        public static readonly Stat Invalid = new(0, 0, "", Array.Empty<int>(), new(0), -1);
    }
}
