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
        Content Content { get; }
        int StartIndex { get; }
        string NextContent { get; }
        bool NextTo(int index);
        bool NextTo(string content);
        int NextIndexOf(string content);
        bool BackTo(int index);
        void JumpLast();
        void Outstrip();
        bool Next();
        void Reset();
    }
  /*  
    public class HtmlContent3 :IHtmlContent
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
                if (_from == -1 || this.Index == -1) return string.Empty;
                if (this._lastFrom == this.From && this._lastIndex == this.Index && this._lastCacheContent != null)
                {
                    return this._lastCacheContent;
                }

                this._lastCacheContent = this._rootContent.Substring(this.From, this.Index-this.From);
                if (this.CurrentChar != default(char))
                    this._lastCacheContent += this.CurrentChar; 
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

        public HtmlContent3(string content)
        {
            this._rootContent = content;
            this._from =0;
            this._index = -1;
        }

        public bool NextTo(int index)
        {
    
            var isDone = index >= this._rootContent.Length;
            if (index < this._rootContent.Length)
                this._index = index;
            else this._index = this._rootContent.Length;
            return !isDone;
        }
        

        public bool NextTo(string content)
        {
            var index = this.Index < 0 ? 0 : this.Index;
            var indexOfNext = this._rootContent.IndexOf(content,index, StringComparison.Ordinal);
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
            this.Next();
            this._from = this._index;
        }

        public int NextIndexOf(string content)
        {
            var result = this.RootContent.IndexOf(content, From, StringComparison.Ordinal);
            if (result > this.Index) return result+content.Length;
            return -1;
        }

        public bool Next()
        {
            var newIndex = this.Index + 1;
            return this.NextTo(newIndex);
        }

        public void Reset()
        {
            this._from = 0;
            this._index = -1;
        }

        public object Clone()
        {
            var result =  new HtmlContent3(this._rootContent);
            result._from = this.From;
            result._index = this.Index;
            return result;
        }
        
    }
*/
    public class HtmlContentDecorator : IHtmlContent
    {
        protected IHtmlContent HtmlContent { get; }

        public string RootContent
        {
            get { return this.HtmlContent.RootContent; }
        }

        public int From
        {
            get { return this.HtmlContent.From; }
        }

        public int Index
        {
            get { return this.HtmlContent.Index; }
        }

        public char CurrentChar
        {
            get { return this.HtmlContent.CurrentChar; }
        }

        public char BeforeChar
        {
            get { return this.HtmlContent.BeforeChar; }
        }

        public char NextChar
        {
            get { return this.HtmlContent.NextChar; }
        }


        public Content Content
        {
            get { return this.HtmlContent.Content; }
        }

        public int StartIndex
        {
            get { return this.HtmlContent.StartIndex; }
        }

        public string NextContent
        {
            get { return this.HtmlContent.Content.ToString(); }
        }

        public HtmlContentDecorator(IHtmlContent content)
        {
            this.HtmlContent = content;
        }
        
        public virtual object Clone()
        {
            return new HtmlContentDecorator((IHtmlContent)this.HtmlContent.Clone());
        }

        public virtual bool NextTo(int index)
        {
            return this.HtmlContent.NextTo(index);
        }

        public virtual bool NextTo(string content)
        {
            return this.HtmlContent.NextTo(content);
        }

        public virtual int NextIndexOf(string content)
        {
            return this.HtmlContent.NextIndexOf(content);
        }

        public virtual bool BackTo(int index)
        {
            return this.HtmlContent.BackTo(index);
        }

        public virtual void JumpLast()
        {
            this.HtmlContent.JumpLast();
        }

        public virtual void Outstrip()
        {
            this.HtmlContent.Outstrip();
        }

        public virtual bool Next()
        {
            return this.HtmlContent.Next();
        }

        public virtual void Reset()
        {
            this.HtmlContent.Reset();
        }
    }


    public class Content
    {
        private StringBuilderAdapter _stringBuilderAdapter;
        private string _cache;
        private bool _isChange = false;

        public int Length
        {
            get { return this._stringBuilderAdapter.Length; }
        }
        public Content(StringBuilderAdapter stringBuilderAdapter)
        {
            this._stringBuilderAdapter = stringBuilderAdapter;
            this._stringBuilderAdapter.Change((e) => { this._isChange = true; });
        }

        public Content()
        {
            this._stringBuilderAdapter = new StringBuilderAdapter();
            this._stringBuilderAdapter.Change((e) => this._isChange = true);
        }

        public bool AnyFrequence(char ch)
        {
            return this._stringBuilderAdapter.AnyFrequenceOf(ch);
        }

        public ChFrequence GetFrequence(char ch)
        {
            return this._stringBuilderAdapter.GetFrequenceOf(ch);
        }

        public void Append(char ch)
        {
            this._stringBuilderAdapter.Append(ch);
        }

        public string Substring(int startIndex, int length)
        {
            return this.ToString().Substring(startIndex, length);
        }

        public void Append(string value)
        {
            this._stringBuilderAdapter.Append(value);
        }

        public void Clear()
        {
            this._stringBuilderAdapter.Clear();
            
        }


        public override string ToString()
        {
            if (!this._isChange) return this._cache;
            this._cache = this._stringBuilderAdapter.ToString();
            this._isChange = false;
            return this._cache;
        }
    }
}

