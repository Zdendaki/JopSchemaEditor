using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JopSchemaEditor
{
    public static class KamenickyEncoding
    {
        private static readonly Dictionary<char, char> _convert = new()
        {
            { 'Č', (char)0x80 },
            { 'ü', (char)0x81 },
            { 'é', (char)0x82 },
            { 'ď', (char)0x83 },
            { 'ä', (char)0x84 },
            { 'Ď', (char)0x85 },
            { 'Ť', (char)0x86 },
            { 'č', (char)0x87 },
            { 'ě', (char)0x88 },
            { 'Ě', (char)0x89 },
            { 'Ĺ', (char)0x8A },
            { 'Í', (char)0x8B },
            { 'ľ', (char)0x8C },
            { 'ĺ', (char)0x8D },
            { 'Ä', (char)0x8E },
            { 'Á', (char)0x8F },
            { 'É', (char)0x90 },
            { 'ž', (char)0x91 },
            { 'Ž', (char)0x92 },
            { 'ô', (char)0x93 },
            { 'ö', (char)0x94 },
            { 'Ó', (char)0x95 },
            { 'ů', (char)0x96 },
            { 'Ú', (char)0x97 },
            { 'ý', (char)0x98 },
            { 'Ö', (char)0x99 },
            { 'Ü', (char)0x9A },
            { 'Š', (char)0x9B },
            { 'Ľ', (char)0x9C },
            { 'Ý', (char)0x9D },
            { 'Ř', (char)0x9E },
            { 'ť', (char)0x9F },
            { 'á', (char)0xA0 },
            { 'í', (char)0xA1 },
            { 'ó', (char)0xA2 },
            { 'ú', (char)0xA3 },
            { 'ň', (char)0xA4 },
            { 'Ň', (char)0xA5 },
            { 'Ů', (char)0xA6 },
            { 'Ô', (char)0xA7 },
            { 'š', (char)0xA8 },
            { 'ř', (char)0xA9 },
            { 'ŕ', (char)0xAA },
            { 'Ŕ', (char)0xAB }
        };

        private const byte UNK = (byte)'?';

        private static readonly char[] SPECIAL_CHARS = ['(', ')', '[', ']', '{', '}', '~', '/', '\\', '<', '>', '=', '+', '-', '.', ',', '^', '_', '\'', '"', '|', '?', '!', '#', '$', ':', ';', '*', '&', '%', ' ', '@', '`'];

        public static string Encode(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            StringBuilder sb = new(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (_convert.TryGetValue(c, out char value))
                    sb.Append(value);
                else if (c <= 0xFF)
                    sb.Append(c);
                else
                    sb.Append('?');
            }

            return sb.ToString();
        }

        public static IEnumerable<byte> EncodeByte(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                yield break;

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];
                if (_convert.TryGetValue(c, out char value))
                    yield return (byte)value;
                else if (c <= 0xFF)
                    yield return (byte)c;
                else
                    yield return UNK;
            }
        }

        public static string Decode(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            StringBuilder sb = new(input.Length);

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (_convert.ContainsValue(c))
                    sb.Append(_convert.Single(x => x.Value == c).Key);
                else
                    sb.Append(c);
            }

            return sb.ToString();
        }

        public static bool IsKamenickyLetterOrDigit(this char c)
        {
            if (char.IsAsciiLetterOrDigit(c) || SPECIAL_CHARS.Contains(c))
                return true;

            foreach (var x in _convert)
            {
                if (x.Value == c) return true;
            }

            return false;
        }
    }
}
