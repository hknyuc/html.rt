using System;

namespace Html.Rt.Seperator
{
    public class HtmlContent :IHtmlContent
    {

        public string RootContent { get; }
        public int From { get; private set; }
        public int Index { get; private set; }

        public char CurrentChar
        {
            get { return this.GetCharFromRoot(this.Index); }
        }

        public char BeforeChar
        {
            get { return this.GetCharFromRoot(this.Index - 1); }
        }

        public char NextChar
        {
            get { return this.GetCharFromRoot(this.Index + 1); }
        }


        private string _cacheContent = string.Empty;

        public Content Content { get; private set; }

        public int StartIndex { get; }

        public string NextContent
        {
            get { throw new Exception("qs"); }
        }

        private bool _isChanged;


        public HtmlContent(string content)
        {
            this.RootContent = content;
            this.Content = new Content();
           this.Reset(); 
        }

        private void SetContent(Content content)
        {
            this.Content = content;
        }

        private char GetCharFromRoot(int index)
        {
            if (index< 0) return default(char);
            if (this.RootContent.Length <= index) return default(char);
            return this.RootContent[index];
        }


        private bool IsOverLength(int index)
        {
            return index >= this.RootContent.Length;
        }
        
        public bool NextTo(int index)
        {
            if (index < this.Index) return false;
            var nextIndex = this.IsOverLength(index) ? this.RootContent.Length : index; 
            InsertContentTo(nextIndex);
            this.Index = nextIndex;
            return !this.IsOverLength(index);
        }

        private void InsertContentTo(int index)
        {
            var distance= index - this.Index;
            if(distance == 0) return;
            var first = this.Index + 1;
            var right = first + distance;
            var len = right > this.RootContent.Length ? this.RootContent.Length : right;
            for (var i = first; i < len; i++)
                this.Content.Append(this.RootContent[i]);
        }

        public bool NextTo(string content)
        {
            var result = this.NextIndexOf(content);
            return this.NextTo(result);
        }

        public int NextIndexOf(string content)
        {
            var from = this.From < 0 ? 0 : this.From;
            var result = this.RootContent.IndexOf(content, from, StringComparison.OrdinalIgnoreCase);
            if (result == -1) return -1;
            return result + content.Length;
        }
        

        public object Clone()
        {
            var result = new HtmlContent(this.RootContent)
            {
                From = this.From,
                Index =  this.Index
            };
            result.SetContent(this.Content);
            return result;
           
        }
        public bool BackTo(int index)
        {
             throw new NotSupportedException();
        }

        public void JumpLast()
        {
            this.NextTo(this.RootContent.Length);
        }
        

        public void Outstrip()
        {
            this.Content.Clear();
            this.From = this.Index;
            this.Next();
        }

        public bool Next()
        {
            return this.NextTo(this.Index + 1);
        }

        public void Reset()
        {
            this.From = 0;
            this.Content.Clear();
            this.Index = -1;
        }
    }
}