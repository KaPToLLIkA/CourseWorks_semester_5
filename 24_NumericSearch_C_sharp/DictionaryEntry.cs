using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _24_NumericSearch_C_sharp
{
    class DictionaryEntry
    {
        private byte[] key;
        private string desc;

        public DictionaryEntry()
        {

        }

        public DictionaryEntry(string key, string desc)
        {
            this.key = System.Text.Encoding.UTF8.GetBytes(key);
            this.desc = desc;
        }

        public string KeyS
        {
            get => System.Text.Encoding.UTF8.GetString(this.key);
            set => key = System.Text.Encoding.UTF8.GetBytes(value);
        }

        public byte[] KeyB
        {
            get => this.key;
        }

        public string Description
        {
            get => desc;
            set => desc = value;
        }

        public Int32 getBitAtI(Int32 i)
        {
            if (key.Length * 8 * sizeof(byte) <= i)
            {
                return 0;
            }

            Int32 item = i / (sizeof(byte) * 8);
            Int32 bit = i % (sizeof(byte) * 8);

            return Convert.ToInt32((key[item] & (1 << ((sizeof(byte) * 8) - 1 - bit))) != 0);
        }
    }


    class DictionaryEntryComparer : IComparer<DictionaryEntry>
    {
        public int Compare(DictionaryEntry x, DictionaryEntry y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y == null)
                {
                    return 1;
                }
                else
                {
                    return x.KeyS.CompareTo(y.KeyS);
                }
            }
        }
    }
}
