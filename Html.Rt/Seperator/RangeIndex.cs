using System.Runtime.Serialization;

namespace Html.Rt.Seperator
{
    public class RangeIndex
    {
        public int From { get; protected set; }
        public int End { get; protected set; }

        public RangeIndex(int from, int end)
        {
            this.From = from;
            this.End = end;
        }
    }

    public class ChangeRangeIndex : RangeIndex
    {
        public ChangeRangeIndex(int @from, int end) : base(@from, end)
        {
        }

        public ChangeRangeIndex() : base(0, 0)
        {
            
        }

        public void Set(int from, int end)
        {
            this.From = from;
            this.End = end;
        }

        public void Reset()
        {
            this.From = -1;
            this.End = -1;
        }
    }
}