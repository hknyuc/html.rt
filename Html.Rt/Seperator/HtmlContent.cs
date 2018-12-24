using System;

namespace Html.Rt.Seperator
{
    public class HtmlContent :ICloneable
    {
        /// <summary>
        /// Index of Content
        /// </summary>
        public int Index { get; private set; }


        public string Content
        {
            get;
            set;
        }

        public string NextContent
        {
            get { return this._fullContent.Substring(Index, this._fullContent.Length - Index - 1); }
        }
        
        private string _fullContent;
        
        public HtmlContent(string content)
        {
            this._fullContent = content;
        }

        public void NextTo(int index)
        {
            for (var i = Index; i< index; i++)
            {
                this.Content += this._fullContent[i].ToString();
            }
        }

        public void NextTo(string content)
        {
            var indexOfNext = this.NextContent.IndexOf(content, StringComparison.Ordinal);
            if (indexOfNext < 0) return;
            var newIndex = indexOfNext + this.Index;
            this.NextTo(newIndex);
        }

        public HtmlContent JumpLast()
        {
            this.Content = this._fullContent;
            return this;
        }

        public int IndexOf(string content)
        {
            return this._fullContent.IndexOf(content, StringComparison.Ordinal);
        }

        public bool Next()
        {
            if (this.Index + 1 >= this._fullContent.Length -1) return false;
            this.Index++;
            return true;
        }

        public object Clone()
        {
            var result = new HtmlContent(this._fullContent);
            result.Index = this.Index;
            return result;
        }
        
    }
}