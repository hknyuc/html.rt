using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Html.Rt
{
    internal interface IEmitterObserver
    {
        
    }

    internal class EmitterFuncObserver<TResult> : IEmitterObserver
    {
        public Func<TResult, bool> Func { get; private set; }
        public EmitterFuncObserver(Func<TResult,bool> func)
        {
            this.Func = func;
        }
    }

    internal class EmitterActionObserver<TResult> : IEmitterObserver
    {
        public Action<TResult> Action { get; private set; }

        public EmitterActionObserver(Action<TResult> action)
        {
            this.Action = action;
        }
    }
    internal class Emitter<TResult> :ICloneable
    {
        private  List<IEmitterObserver> _observers;
        private readonly IEmitterStrategy<TResult> _emitterStrategy;
        public Emitter(IEmitterStrategy<TResult> emitterStrategy)
        {
            this._observers = new List<IEmitterObserver>();
            this._emitterStrategy = emitterStrategy;
        }

        public Emitter()
        {
            this._observers = new List<IEmitterObserver>();
            this._emitterStrategy = new Sync<TResult>();
        }

        public Emitter<TResult> Hook(Func<TResult, bool> hookFn)
        {
            this._observers.Add(new EmitterFuncObserver<TResult>(hookFn));
            return this;
        }

        public Emitter<TResult> Hook(Action<TResult> hookFn)
        {
            this._observers.Add(new EmitterActionObserver<TResult>(hookFn));
            return this;
        }

        public void Emit(TResult value)
        {
            this._emitterStrategy.Emit(value, this._observers.ToArray());
        }

        public object Clone()
        {
            var result = new Emitter<TResult>(this._emitterStrategy);
            result._observers = this._observers.ToList();
            return result;
        }
    }

    internal interface IEmitterStrategy<in TResult>
    {
        bool Emit(TResult value, IEnumerable<IEmitterObserver> observers);
    }

    internal class Sync<TResult> : IEmitterStrategy<TResult>
    {
        public bool Emit(TResult value, IEnumerable<IEmitterObserver> observers)
        {
            var current = value;
            foreach (var observer in observers)
            {
                if (observer is EmitterActionObserver<TResult> actionObserver)
                {
                    actionObserver.Action(current);
                }else if (observer is EmitterFuncObserver<TResult> funcObserver)
                {
                    if(!funcObserver.Func(current));
                    return false;
                }
            }
            return true;
        }

    }

    internal class Async<TResult> : IEmitterStrategy<TResult>
    {

        public bool Emit(TResult value, IEnumerable<IEmitterObserver> observers)
        {
            foreach (var observer in observers)
            {
                Task.Factory.StartNew(() =>
                {
                    switch (observer)
                    {
                        case EmitterActionObserver<TResult> actionObserver:
                            actionObserver.Action(value);
                            break;
                        case EmitterFuncObserver<TResult> funcObserver:
                            funcObserver.Func(value);
                            break;
                    }
                });
            }

            return true;
        }
    }

}