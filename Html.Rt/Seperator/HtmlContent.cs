using System;

namespace Html.Rt.Seperator
{
    public interface IHtmlContent :ICloneable
    {
        string RootContent { get; }
        int From { get; }
        int Index { get; }
        char CurrentChar { get; }
        char BeforeChar { get; }
        char NextChar { get; }
        string Content { get; }
        int StartIndex { get; }
        string NextContent { get; }
        bool NextTo(int index);
        bool NextTo(string content);
        int NextIndexOf(string content);
        bool BackTo(int index);
        void JumpLast();
        void Outstrip();
        bool Next();
    }
    public class HtmlContent :IHtmlContent
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

        public char CurrentChar
        {
            get
            {
                if (this.Index == -1) return default(char);
                if (this.Index >= this._rootContent.Length) return default(char);
                return this._rootContent[this.Index];
            }
        }

        public char BeforeChar
        {
            get
            {
                if (this.Index - 1 < 0) return default(char);
                return this._rootContent[this.Index - 1];
            }
        }

        public char NextChar
        {
            get
            {
                if (this.Index + 1 > this._rootContent.Length) return default(char);
                return this._rootContent[this.Index + 1];
            }
        }

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
        

        public bool NextTo(string content)
        {
            var indexOfNext = this._rootContent.IndexOf(content,this.Index, StringComparison.Ordinal);
            if (indexOfNext < 0) return false;
            return this.NextTo(indexOfNext + content.Length); // indexOfNext is first char of content in fullcontent
        }

        public bool BackTo(int index)
        {
            if (index < 0) return false;
            if (this._from > index) return false;
            this._index = index;
            return true;
        }

        public void JumpLast()
        {
            this._index = this.RootContent.Length;
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

    public class HtmlContentDecorator : IHtmlContent
    {
        private IHtmlContent _content;

        public string RootContent
        {
            get { return this._content.RootContent; }
        }

        public int From
        {
            get { return this._content.From; }
        }

        public int Index
        {
            get { return this._content.Index; }
        }

        public char CurrentChar
        {
            get { return this._content.CurrentChar; }
        }

        public char BeforeChar
        {
            get { return this._content.BeforeChar; }
        }

        public char NextChar
        {
            get { return this._content.NextChar; }
        }


        public string Content
        {
            get { return this._content.Content; }
        }

        public int StartIndex
        {
            get { return this._content.StartIndex; }
        }

        public string NextContent
        {
            get { return this._content.Content; }
        }

        public HtmlContentDecorator(IHtmlContent content)
        {
            this._content = content;
        }
        
        public object Clone()
        {
            return new HtmlContentDecorator((IHtmlContent)this._content.Clone());
        }

        public virtual bool NextTo(int index)
        {
            return this._content.NextTo(index);
        }

        public virtual bool NextTo(string content)
        {
            return this._content.NextTo(content);
        }

        public virtual int NextIndexOf(string content)
        {
            return this._content.NextIndexOf(content);
        }

        public virtual bool BackTo(int index)
        {
            return this._content.BackTo(index);
        }

        public virtual void JumpLast()
        {
            this._content.JumpLast();
        }

        public virtual void Outstrip()
        {
            this._content.Outstrip();
        }

        public virtual bool Next()
        {
            return this._content.Next();
        }
    }
}