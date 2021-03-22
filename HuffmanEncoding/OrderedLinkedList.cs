using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanEncoding
{
    //Note: This data structure supports duplicates. If a duplicate is being added, it is added after the existing duplicate element.
    public class OrderedLinkedList<T> where T : IComparable
    {
        private LinkedList<T> list;

        public LinkedListNode<T> First
        {
            get { return list.First; }
        }
        public LinkedListNode<T> Last
        {
            get { return list.Last; }
        }

        public OrderedLinkedList()
        {
            list = new LinkedList<T>();
        }//end zero variable constructor

        public void Add(T element)
        {
            LinkedListNode<T> node;

            //1. If LL is empty
            if (list.Count == 0)
            {
                list.AddFirst(element);
            }

            //2. If node is smaller than the first element
            else if (list.First.Value.CompareTo(element) > 0)
            {
                list.AddFirst(element);
            }

            //3. If node is larger than or equal to the last element
            else if (list.Last.Value.CompareTo(element) < 0 || list.Last.Value.CompareTo(element) == 0)
            {
                list.AddLast(element);
            }

            //4. If node is larger than first and smaller than last - Iterate through LL.
            else
            {
                node = list.First;  //Node is the first node in the LL.
                node = node.Next;  //The element was already compared to the first node in the list. We do not need to compare element and first node again.
                do
                {
                    if (node.Value.CompareTo(element) > 0)  //If the node is smaller than the next element.
                    {
                        list.AddBefore(node, element);  //add the node before the next element.
                        break;  //There is no need to iterate through the linked list after the element is added.
                    }

                    node = node.Next;
                } while (node != null);  //loop executes as many times as there are nodes in the LL.
            }

        }//end Add method

        public void PrintList()
        {
            LinkedListNode<T> node;
            node = list.First;

            if (!(list.Count() == 0))
            {
                for (int i = 1; i <= list.Count(); ++i)
                {
                    Console.Write($"{node.Value} ");
                    node = node.Next;
                }
                Console.WriteLine($"\n");
            }

        }//end PrintList method

        public void Clear()
        {
            list.Clear();
        }//end Clear method

        public int Count()
        {
            return list.Count();
        }//end Count method

        public void RemoveFirst()
        {
            list.RemoveFirst();
        }//end RemoveFirst method

        public void RemoveLast()
        {
            list.RemoveLast();
        }//end RemoveLast method

    }//end OrderedLinkedList class
}//end namespace
