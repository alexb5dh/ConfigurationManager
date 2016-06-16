using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Web;
using ConfigurationManager.WebService.Exceptions;

namespace ConfigurationManager.WebService.Models
{
    public sealed class Key : IEquatable<Key>
    {
        public const char SegmentSeparator = '/';

        private string _section;

        public string Section
        {
            get { return _section; }
            private set { _section = value.ToLower(); }
        }

        private string _subkey;

        public string Subkey
        {
            get { return _subkey; }
            private set { _subkey = value.ToLower(); }
        }

        private static bool IsKeyValid(string normalizedKey)
        {
            if (normalizedKey == SegmentSeparator.ToString())
            {
                return false;
            }

            var lastCharIsSeparator = false;
            foreach (var @char in normalizedKey)
            {
                if (@char == SegmentSeparator)
                {
                    if (lastCharIsSeparator)
                    {
                        return false;
                    }
                    lastCharIsSeparator = true;
                }
                else
                {
                    lastCharIsSeparator = false;
                    if (!char.IsLetterOrDigit(@char))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private static bool IsSectionValid(string section)
        {
            return (section == "" || section == SegmentSeparator.ToString() || IsKeyValid(section));
        }

        private static bool IsSubkeyValid(string subkey)
        {
            return subkey.All(char.IsLetterOrDigit);
        }

        private static string Normalize(string key)
        {
            var separator = SegmentSeparator.ToString();

            if (key == separator)
            {
                return key;
            }

            if (!key.StartsWith(separator))
            {
                key = SegmentSeparator + key;
            }
            if (key.EndsWith(separator))
            {
                key = key.Substring(0, key.Length - 1);
            }

            return key;
        }

        public Key(string key) : this(key, false)
        {
        }

        public Key(string section, string subkey)
        {
            if (!IsSectionValid(section))
            {
                throw new ArgumentException("", "section");
            }
            if (!IsSubkeyValid(subkey))
            {
                throw new ArgumentException("", "subkey");
            }

            Section = Normalize(section);
            Subkey = subkey;
        }

        public Key(string key, bool isSectionKey)
        {
            if ((!isSectionKey && !IsKeyValid(key))
                || (isSectionKey && !IsSectionValid(key)))
            {
                throw new ArgumentException("", "key");
            }

            key = Normalize(key);
            var lastSegmentIndex = key.LastIndexOf(SegmentSeparator.ToString(), StringComparison.Ordinal);
            if (!isSectionKey)
            {
                Section = key.Substring(0, lastSegmentIndex + 1);
                Subkey = key.Substring(lastSegmentIndex + 1, key.Length - lastSegmentIndex - 1);
            }
        }

        public override string ToString()
        {
            return Section == SegmentSeparator.ToString()
                       ? Section + Subkey
                       : Section + SegmentSeparator + Subkey;
        }

        public static implicit operator string(Key key)
        {
            return key.ToString();
        }

        public bool Equals(Key other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Section, other.Section) && string.Equals(Subkey, other.Subkey);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Key && Equals((Key) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Section != null ? Section.GetHashCode() : 0)*397) ^ (Subkey != null ? Subkey.GetHashCode() : 0);
            }
        }

        public static bool operator ==(Key left, Key right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Key left, Key right)
        {
            return !Equals(left, right);
        }
    }
}