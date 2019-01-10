using System;

namespace Html.Rt.Seperator
{
    public class ObservabletHtmlContent : HtmlContentDecorator
    {
        private  Emitter<IHtmlContent> _emitter;
     
        public ObservabletHtmlContent(IHtmlContent content) : base(content)
        {
            this._emitter = new Emitter<IHtmlContent>();
    
        }

        public ObservabletHtmlContent(string content):base ( new HtmlContent(content))
        {
            this._emitter = new Emitter<IHtmlContent>();
        }

        public ObservabletHtmlContent Observe(Func<IHtmlContent,bool> funcSelector)
        {
            this._emitter.Hook(funcSelector);
            return this;
        }
        public ObservabletHtmlContent Observe(Action<IHtmlContent> actionSelector)
        {
            this._emitter.Hook(actionSelector);
            return this;
        }

        public override object Clone()
        {
            var resultEmitter = new ObservabletHtmlContent((IHtmlContent) this.HtmlContent.Clone())
            {
                _emitter = (Emitter<IHtmlContent>) this._emitter.Clone()
            };
            return resultEmitter;
        }

       

        public override bool Next()
        {
            this._emitter.Emit(this.HtmlContent);
            return base.Next();
        }
    }
}