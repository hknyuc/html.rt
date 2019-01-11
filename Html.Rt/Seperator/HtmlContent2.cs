using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

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

        public string Content { get; private set; } = string.Empty;
        public int StartIndex { get; }

        public string NextContent
        {
            get { throw new Exception("qs"); }
        }


        public HtmlContent(string content)
        {
            this.RootContent = content;
           this.Reset(); 
        }

        private char GetCharFromRoot(int index)
        {
            if (index< 0) return default(char);
            if (this.RootContent.Length <= index) return default(char);
            return this.RootContent[index];
        }

        private bool AnyChar(int index)
        {
            return this.GetCharFromRoot(index) != default(char);
        }
        
        public bool NextTo(int index)
        {
            if (index > this.RootContent.Length)
            {
                
            }

            if (!this.AnyChar(index))
            {
                if (index > this.RootContent.Length)
                {
                    
                }
            };
            if (index < Index) return false;
            InsertContentTo(index); 
            this.Index = index;
            return true;
        }

        private void InsertContentTo(int index)
        {
            var distance= index - this.Index;
            if(distance == 0) return;
            var first = this.Index + 1;
            var right = first + distance;
            var len = right > this.RootContent.Length ? this.RootContent.Length : right;
            for (var i = first; i < len; i++)
            {
                Content += this.RootContent[i];
            }
        }

        public bool NextTo(string content)
        {
            var result = this.NextIndexOf(content);
            return this.NextTo(result);
        }

        public int NextIndexOf(string content)
        {
            var result = this.RootContent.IndexOf(content, this.From, StringComparison.OrdinalIgnoreCase);
            if (result == -1) return -1;
            return result + content.Length;
        }
        

        public object Clone()
        {
            var result = new HtmlContent(this.RootContent);
            result.Content = this.Content;
            result.From = this.From;
            this.Index = this.Index;
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
            this.Content = string.Empty;
            this.From = this.Index;
            this.Next();
        }

        public bool Next()
        {
            return this.NextTo(this.Index + 1);
        }

        public void Reset()
        {
            this.From = -1;
            this.Index = -1;
        }
    }
}