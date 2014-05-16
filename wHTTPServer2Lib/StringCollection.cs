using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItzWarty;

namespace ItzWarty.wHTTPServer2Lib
{
    /// <summary>
    /// implements a php-like string collection [similar to $_GET]
    /// </summary>
    public class StringCollection : IDisposable
    {
        Dictionary<string, string> keyValues = new Dictionary<string, string>();
        public StringCollection() { }
        public StringCollection(string nameEqualsValues, string delimiter)
        {
            string[] cookiesArray = nameEqualsValues.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < cookiesArray.Length; i++)
            {
                string cookie = cookiesArray[i];
                string[] cookieParts = cookie.Split("=");
                string cookieName = cookieParts[0].Trim();
                string cookieContent = String.Join("=", cookieParts.SubArray(1, cookieParts.Length - 1)).Trim();
                this.keyValues.Add(cookieName.ToLower(), cookieContent);
            }
        }
        public bool ContainsKey(string name)
        {
            return this.keyValues.ContainsKey(name.ToLower());
        }
        public string this[string s]
        {
            get
            {
                if (this.ContainsKey(s))
                    return this.keyValues[s.ToLower()];
                else
                    return "";
            }
            set
            {
                if (this.isFrozen)
                {
                    throw new MutatedFrozenObjectException();
                }
                else
                {
                    if (ContainsKey(s))
                        this.keyValues[s.ToLower()] = value;
                    else
                        this.keyValues.Add(s.ToLower(), value);
                }
            }
        }
        public string[] Keys
        {
            get
            {
                return new List<string>(this.keyValues.Keys).ToArray();
            }
        }

        private bool isFrozen = false;
        public void Freeze() { isFrozen = true; }
        public bool IsFrozen { get { return isFrozen; } }

        public override string ToString()
        {
            return ToString(";");
        }
        public string ToString(string delimiter)
        {
            List<string> keys = new List<string>(this.keyValues.Keys);
            string result = "";
            for (int i = 0; i < keys.Count; i++)
            {
                result += keys[i] + "=" + this.keyValues[keys[i]] + delimiter;
            }
            return result;
        }

        public void Dispose()
        {
        }
    }
}
