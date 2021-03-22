using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HuffmanEncoding
{
    public class BinaryTree<E> : IComparable where E : IComparable
    {
        private BinaryTreeNode<E> root;
        private BinaryTreeNode<E> current;
        private int size;
        private StringBuilder encoding;
        private CharacterEncoding[] characterEncodingString = new CharacterEncoding[255];

        public enum Relative : int 
        {
            leftChild, rightChild, parent, root
        };

        public BinaryTree()
        {
            root = null;
            current = null;
            size = 0;
            encoding = new StringBuilder();
        }

        public BinaryTree(E data)
        {
            root = new BinaryTreeNode<E>(data);
            current = null;
            size = 0;
            encoding = new StringBuilder();
        }

        public void Destroy(BinaryTreeNode<E> node)
        {
            if (node != null)
            {
                Destroy(node.Left);
                Destroy(node.Right);
                node = null;
                size--;
            }
        }

        public Boolean isEmpty()
        {
            return root == null;
        }

        public int Size
        {
            get
            {
                return size;
            }
        }

        public BinaryTreeNode<E> Current
        {
            get
            {
                return current;
            }
            set
            {
                current = value;
            }
        }

        public BinaryTreeNode<E> Root
        {
            get
            {
                return root;
            }
            set
            {
                root = value;
            }
        }


        /// <summary>
        /// This method traverses the binary tree and creates a unique character encoding value for each leaf in the tree.
        /// </summary>
        /// <param name="p"></param>
        public object BuildEncodingTable(BinaryTreeNode<E> p)
        {
            //If the BinaryTreeNode exists
            if (p != null)
            {
                //if p is the root node and there are no children nodes (i.e. if the tree is just a root node)
                if(p.isLeaf() && p.Equals(Root))
                {
                    //append the encoding by 0.
                    encoding.Append("0");
                }

                //if p is not a leaf node
                //First iterate to the left
                if (!(p.isLeaf()))
                {
                    //for every iteration to the left, append the character encoding stringbuilder with a 1 and move to the left child node
                    encoding.Append("1");
                    BuildEncodingTable(p.Left);
                }

                //Once fully iterated to the left, check to see if p is a leaf nod
                if (p.isLeaf())
                {
                    //If so, create a new CharacterEncoding object with the current leaf node character and with the current character encoding string
                    CharacterEncoding ce = new CharacterEncoding(Convert.ToChar(p.Data.GetHashCode()), encoding.ToString());
                    //Add this CE object to the array of character encoding objects.
                    characterEncodingString[ce.GetCharacter()] = ce;
                }

                //If the current object is not a leaf node and after fully iterating to the left, iterate to the right.
                if (!(p.isLeaf()))
                {
                    //for every iteration to the right, append the character encoding stringbuilder with a 0 and move to the right child node
                    encoding.Append("0");
                    BuildEncodingTable(p.Right);
                }

                //Once a leaf node has been reached and a CharacterEncoding object has been created, or if a pathway to a leaf node has been created, remove the last number in the encoding stringbuilder (assuming the stringbuilder is greater than 0).
                if(encoding.Length != 0)
                {
                    encoding.Remove((encoding.Length - 1), 1);
                }

                //after the CharacterEncoding array has been built, return it.
                return characterEncodingString;
            }
            else
            {
                // Remove a character from the encoding string
                Console.WriteLine("remove!");

                return null;
            }
        }//end BuildEncodingTable method

        public void preOrder(BinaryTreeNode<E> p)
        {
            if (p != null)
            {
                Console.WriteLine(p.Data.ToString());
                preOrder(p.Left);
                preOrder(p.Right);
            }
        }

        public void postOrder(BinaryTreeNode<E> p)
        {
            if (p != null)
            {
                postOrder(p.Left);
                postOrder(p.Right);
                Console.WriteLine(p.Data.ToString());
            }
        }

        public void inOrder(BinaryTreeNode<E> p)
        {
            if (p != null)
            {
                inOrder(p.Left);
                Console.WriteLine(p.Data.ToString());
                inOrder(p.Right);
            }
        }

        private BinaryTreeNode<E> findParent(BinaryTreeNode<E> n)
        {
            Stack<BinaryTreeNode<E>> s = new Stack<BinaryTreeNode<E>>();
            n = root;
            while (n.Left != current && n.Right != current)
            {
                if (n.Right != null)
                    s.Push(n.Right);

                if (n.Left != null)
                    n = n.Left;
                else
                    n = s.Pop();
            }
            s.Clear();
            return n;
        }

        public Boolean Insert(BinaryTreeNode<E> node, Relative rel)
        {
            Boolean inserted = true;

            if ((rel == Relative.leftChild && current.Left != null)
                    || (rel == Relative.rightChild && current.Right != null))
            {
                inserted = false;
            }
            else
            {
                switch (rel)
                {
                    case Relative.leftChild:
                        current.Left = node;
                        break;
                    case Relative.rightChild:
                        current.Right = node;
                        break;
                    case Relative.root:
                        if (root == null)
                            root = node;
                        current = root;
                        break;
                }
                size++;
            }

            return inserted;
        }

        public Boolean Insert(E data, Relative rel)
        {
            Boolean inserted = true;

            BinaryTreeNode<E> node = new BinaryTreeNode<E>(data);

            if ((rel == Relative.leftChild && current.Left != null)
                    || (rel == Relative.rightChild && current.Right != null))
            {
                inserted = false;
            }
            else
            {
                switch (rel)
                {
                    case Relative.leftChild:
                        current.Left = node;
                        break;
                    case Relative.rightChild:
                        current.Right = node;
                        break;
                    case Relative.root:
                        if (root == null)
                            root = node;
                        current = root;
                        break;
                }
                size++;
            }

            return inserted;
        }


        public Boolean moveTo(Relative rel)
        {
            Boolean found = false;

            switch (rel)
            {
                case Relative.leftChild:
                    if (current.Left != null)
                    {
                        current = current.Left;
                        found = true;
                    }
                    break;
                case Relative.rightChild:
                    if (current.Right != null)
                    {
                        current = current.Right;
                        found = true;
                    }       
                    break;
                case Relative.parent:
                    if (current != root)
                    {
                        current = findParent(current);
                        found = true;
                    }
                    break;
                case Relative.root:
                    if (root != null)
                    {
                        current = root;
                        found = true;
                    }
                    break;
            } // end Switch relative

            return found;
        }


        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj == this)
                return 0;
            if (!(obj is BinaryTree<E>))
                throw new ArgumentException("comparing obj is not a BinaryTree<E>");

            BinaryTree<E> bt = obj as BinaryTree<E>;
            return this.current.CompareTo(bt.current); //Compares the generic data from the received current node to this objects generic data
        }//end CompareTo method


    }
}
