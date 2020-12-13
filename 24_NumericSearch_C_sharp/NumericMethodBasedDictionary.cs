using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _24_NumericSearch_C_sharp
{
    class NumericMethodBasedDictionary
    {
        private class Node
        {
            public List<DictionaryEntry> values = null;
            public Node[] next = new Node[2] { null, null };
        }

        

        private static Int32 maxAtoms = 200;
        private static Int32 minSearchLen = 3;
        private static Int32 maxKeyLenInBits = sizeof(byte) * 8 * maxAtoms;



        private Node root = null;

        private List<DictionaryEntry> AddUnique(List<DictionaryEntry> list, DictionaryEntry e)
        {
            DictionaryEntryComparer c = new DictionaryEntryComparer();
            if (list.BinarySearch(e, c) < 0)
            {
                list.Add(e);
            }
            return list;
        }

        public NumericMethodBasedDictionary()
        {
            root = new Node();
            root.next[0] = new Node();
            root.next[1] = new Node();
        }

        public void AddEntry(DictionaryEntry e)
        {
            Node current = root;
            List<DictionaryEntry> tmp = null;
            for (Int32 i = 0; i < maxKeyLenInBits; ++i)
            {
                Int32 bit = e.getBitAtI(i);

                if (i == maxKeyLenInBits - 1)
                {
                    if (current.next[bit] == null)
                    {
                        current.next[bit] = new Node();
                        current.next[bit].values = new List<DictionaryEntry>();
                    }
                    current.next[bit].values = AddUnique(current.next[bit].values, e);

                    if (tmp != null)
                    {
                        Int32 bit2 = tmp[0].getBitAtI(i);
                        if (current.next[bit] == null)
                        {
                            current.next[bit2] = new Node();
                            current.next[bit2].values = new List<DictionaryEntry>();
                        }
                        current.next[bit2].values = AddUnique(current.next[bit2].values, tmp[0]);
                    }

                    return;
                }

                if (tmp != null)
                {
                    Int32 bit2 = tmp[0].getBitAtI(i);
                    if (bit2 == bit)
                    {
                        current.next[bit] = new Node();
                        current = current.next[bit];
                    } 
                    else
                    {
                        current.next[bit] = new Node();
                        current.next[bit2] = new Node();
                        current.next[bit].values = new List<DictionaryEntry>();
                        current.next[bit].values.Add(e);
                        current.next[bit2].values = tmp;
                        return;
                    }
                } 
                else if (current.next[bit] == null)
                {
                    if (current.values == null) 
                    {
                        current.values = new List<DictionaryEntry>();
                        current.values.Add(e);
                        return;
                    }
                    else
                    {
                        Int32 bit2 = current.values[0].getBitAtI(i);
                        
                        if (bit2 == bit)
                        {
                            tmp = current.values;
                        } 
                        else
                        {
                            current.next[bit2] = new Node();
                            current.next[bit2].values = current.values;
                        }

                        current.next[bit] = new Node();
                        
                        current.values = null;
                        current = current.next[bit];
                    }
                } 
                else
                {
                    current = current.next[bit];
                }
            }
        }

        public List<DictionaryEntry> Find(DictionaryEntry key)
        {
            List<DictionaryEntry> d = new List<DictionaryEntry>();
            

            if (key.KeyS.Length < minSearchLen)
            {
                return d;
            }

            Int32 keyLen = (key.KeyB.Length * 8 * sizeof(byte)) - 1;
            List<Node> qNextLvl = new List<Node>();
            List<Node> qCurLvl = new List<Node>();
            qCurLvl.Add(root);

            for (Int32 i = 0; i < maxKeyLenInBits; ++i)
            {
                if (qCurLvl.Count == 0)
                {
                    return d;
                }

                
                while (qCurLvl.Count > 0)
                {
                    Node current = qCurLvl[0];
                    qCurLvl.RemoveAt(0);
                    if (i > keyLen)
                    {
                        for (Int32 bit = 0; bit < 2; ++bit)
                        {
                            if (current.next[bit] != null)
                            {
                                qNextLvl.Add(current.next[bit]);
                            }
                        }
                    }
                    else
                    {
                        Int32 bit = key.getBitAtI(i);
                        if (current.next[bit] != null)
                        {
                            qNextLvl.Add(current.next[bit]);
                        }

                    }

                    if (current.values != null && i >= keyLen)
                    {
                        foreach (DictionaryEntry e in current.values)
                        {
                            d.Add(e);
                        }
                    }
                }

                qCurLvl = qNextLvl;
                qNextLvl = new List<Node>();
            }

            return d;
        }
       
    }
}
