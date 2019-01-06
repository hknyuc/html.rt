using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Html.Rt
{
    public class Attributes:IEnumerable<IAttribute>
    {
        private readonly IAttribute[] _attributes;
        public Attributes(IAttribute[] attributes)
        {
            this._attributes = attributes.Where(x => !string.IsNullOrWhiteSpace(x.Key)).ToArray();
        }

        public Attributes()
        {
            this._attributes = Array.Empty<IAttribute>();
        }
        
        
        public IEnumerator<IAttribute> GetEnumerator()
        {
            return ((IEnumerable<IAttribute>) this._attributes).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public string this[string key]
        {
            get
            {
                return this._attributes.FirstOrDefault(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                    ?.Value;
            }
        }
    }
}