using System;

namespace Html.Rt.Seperator
{
    public class HtmlContent :ICloneable
    {

        private int _index = -1;
        /// <summary>
        /// Index of Content
        /// </summary>
        public int Index
        {
            get { return _index; }
        }

        private int _from = -1;
        public int From
        {
            get { return _from; }
        }

        private string _lastCacheContent;
        private int _lastFrom;
        private int _lastIndex;

        public string Content
        {
            get
            {
                if (this._lastFrom == this.From && this._lastIndex == this.Index && this._lastCacheContent != null)
                {
                    return this._lastCacheContent;
                } 
                this._lastCacheContent = this._rootContent.Substring(this.From, this.Index - this.From);
                this._lastFrom = this._from;
                this._lastIndex = this._index;
                return this._lastCacheContent;
            }
        }

        public int StartIndex
        {
            get { return this.Index - this.Content.Length +1; }
        }

        public string NextContent
        {
            get { return this.RootContent.Substring(Index+1, this._rootContent.Length - Index -1 ); }
        }
        

        private string _rootContent = string.Empty;
        public string RootContent => _rootContent;

        public HtmlContent(string content)
        {
            this._rootContent = content;
            this._from = 0;
            this._index = 0;
        }

        public bool NextTo(int index)
        {
            if (index > this._rootContent.Length) return false;
            this._index = index;
            return true;
        }

        public void NextTo(string content)
        {
            var indexOfNext = this._rootContent.IndexOf(content,this.Index, StringComparison.Ordinal);
            if (indexOfNext < 0) return;
            this.NextTo(indexOfNext + content.Length); // indexOfNext is first char of content in fullcontent
        }

        public HtmlContent JumpLast()
        {
            this._index = this.RootContent.Length;
            return this;
        }

        public void Outstrip()
        {
            this._from = this._index;
        }

        public int NextIndexOf(string content)
        {
            var result = this.RootContent.IndexOf(content, From, StringComparison.Ordinal);
            if (result > this.Index) return result;
            return -1;
        }

        public bool Next()
        {
            var newIndex = this.Index + 1;
            return this.NextTo(newIndex);
        }

        public object Clone()
        {
            var result =  new HtmlContent(this._rootContent);
            result._from = this.From;
            result._index = this.Index;
            return result;
        }
        
    }
}