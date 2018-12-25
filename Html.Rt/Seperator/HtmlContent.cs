using System;

namespace Html.Rt.Seperator
{
    public class HtmlContent :ICloneable
    {
        /// <summary>
        /// Index of Content
        /// </summary>
        public int Index {
            get { return this.Content.Length-1; } }


        public string Content
        {
            get;
            private set;
        }

        public string NextContent
        {
            get { return this._fullContent.Substring(Index, this._fullContent.Length - Index ); }
        }
        
        private string _fullContent;
        
        public HtmlContent(string content)
        {
            this._fullContent = content;
            this.Content = string.Empty;
        }

        public bool NextTo(int index)
        {
            if (index >= this._fullContent.Length-1) return false;
            for (var i = this.Index+1; i< index+1; i++)
            {
                this.Content += this._fullContent[i].ToString();
            }
            return true;
        }

        public void Outstrip()
        {
            this._fullContent = this.NextContent;
            this.Content = string.Empty;
       
        }

        public void NextTo(string content)
        {
            var indexOfNext = this._fullContent.IndexOf(content, StringComparison.Ordinal);
            if (indexOfNext < 0) return;
            this.NextTo(indexOfNext + content.Length-1); // indexOfNext is first char of content in fullcontent
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
            var newIndex = this.Index + 1;
            return this.NextTo(newIndex);
        }

        public object Clone()
        {
            var result =  new HtmlContent(this._fullContent);
            result.Content = this.Content;
            return result;
        }
        
    }
}