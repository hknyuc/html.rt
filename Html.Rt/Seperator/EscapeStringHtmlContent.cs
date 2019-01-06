using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;

namespace Html.Rt.Seperator
{
    
    public class  EscapeStringHtmlContent :HtmlContentDecorator
    {
        private bool _inQuotes = false;
        private List<RangeIndex> _quonteses = new List<RangeIndex>();
        public ChangeRangeIndex CurrentRange = new ChangeRangeIndex();
        public bool InQuotes
        {
            get { return this._inQuotes; }
        }

        private char _quotes;

        public char Quotes
        {
            get { return this._quotes; }
        }

        public int LastQuotesIndex
        {
            get
            {
                var max = this._quonteses.Max(x => x.End);
                if (max < this.Index) return max;
                return this.From;
            }
        }

        public bool AnyQuotesAfterFrom(int from)
        {
            var max = this.LastQuotesIndex;
            return max > @from;
        }
        

        public EscapeStringHtmlContent(IHtmlContent content) : base(content)
        {
        }

        private static bool IsQuotes(char current)
        {
            return current == '\'' || current == '"';
        }

        public bool IsQuotes(int index)
        {
            return this._quonteses.Any(x => x.From >= index && index <= x.End);
        }
        

        public override bool Next()
        {
            var content = this.Content;
            var result = base.Next();
            if (result == false) return false;
            if (!IsQuotes(this.CurrentChar) && !this.InQuotes) return true;
            if (this.InQuotes && this.Quotes != this.CurrentChar)
                return this.Next();
            if (this.InQuotes && this.Quotes == this.CurrentChar && this.BeforeChar != '\\')
            {
                this._inQuotes = false;
                this.CurrentRange.Set(Index, 0);
                return base.Next();
            }
            if (IsQuotes(this.CurrentChar) && this.BeforeChar != '\\')
            {
                this._inQuotes = true;
                this._quotes = this.CurrentChar;
                this.CurrentRange.Set(this.CurrentRange.From, this.Index);
                this._quonteses.Add(new RangeIndex(this.CurrentRange.From, this.CurrentRange.End));
                this.CurrentRange.Reset();
                return this.Next();
            }
            return true;
        }
    }
}