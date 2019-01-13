using System;
using System.Collections.Generic;
using System.Text;

namespace Html.Rt.Seperator
{
    public class ChFrequence
    {
        public char Ch { get; }
        public int Count { get; private set; }
        public int LastIndex { get; private set; }
        public ChFrequence(char ch, int lastIndex)
        {
            this.Ch = ch;
            this.Count = 1;
            this.LastIndex = lastIndex;
        }

        public ChFrequence(char ch)
        {
            this.Ch = ch;
        }

        public void Increase(int lastIndex)
        {
            this.Count++;
            this.LastIndex = lastIndex;
        }
    }
    public class StringBuilderAdapter :ICloneable
    {
        public int Length
        {
            get { return this._builder.Length; }
        }
        private StringBuilder _builder;
        private Emitter<StringBuilderAdapter> _isChangedEmiiter = new Emitter<StringBuilderAdapter>(new Sync<StringBuilderAdapter>());

        private Dictionary<char, ChFrequence> _frequences = new Dictionary<char, ChFrequence>();
        public StringBuilderAdapter(StringBuilder builder)
        {
            this._builder = builder;
            
        }

        public ChFrequence GetFrequenceOf(char ch)
        {
            return this._frequences[ch];
        }

        public bool AnyFrequenceOf(char ch)
        {
            return this._frequences.ContainsKey(ch);
        }

        public StringBuilderAdapter()
        {
            this._builder = new StringBuilder();
        }

        public StringBuilderAdapter(string content)
        {
            this._builder = new StringBuilder(content);
        }

        
        public void Change(Action<StringBuilderAdapter> action)
        {
            this._isChangedEmiiter.Hook(action);
        }

        public void Append(char value)
        {
            this._builder.Append(value.ToString());
            this.AddFrequence(value);
            this._isChangedEmiiter.Emit(this);
        }

        private void AddFrequence(char value)
        {
            if (this._frequences.ContainsKey(value))
                this._frequences[value].Increase(this._builder.Length-1);
            else this._frequences.Add(value, new ChFrequence(value, this._builder.Length-1));
        }
        
        

        public void Append(string value)
        {
            this._builder.Append(value);
            foreach (var ch in value)
                this.AddFrequence(ch);
            this._isChangedEmiiter.Emit(this);
        }

        public override string ToString()
        {
            return this._builder.ToString();
        }

        public object Clone()
        {
            var result = new StringBuilderAdapter(this.ToString());
            result._isChangedEmiiter = (Emitter<StringBuilderAdapter>) this._isChangedEmiiter.Clone();
            return result;
        }

        public void Clear()
        {
            this._builder.Clear();
            this._frequences.Clear();
            this._isChangedEmiiter.Emit(this);
        }
        
        
    }
}