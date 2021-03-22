using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuffmanEncoding
{
    public class BinaryTreeNode<E>: IComparable where E : IComparable
    {
        private E m_data;
        private BinaryTreeNode<E> m_left;
        private BinaryTreeNode<E> m_right;

        public BinaryTreeNode(E data)
        {
            m_data = data;
            m_left = null;
            m_right = null;
        }

        public E Data
        {
            get
            {
                return m_data;
            }

            set
            {
                m_data = value;
            }
        }

        public BinaryTreeNode<E> Left
        {
            get
            {
                return m_left;
            }
            set 
            {
                m_left = value;
            }
        }

        public BinaryTreeNode<E> Right
        {
            get
            {
                return m_right;
            }
            set 
            {
                m_right = value;
            }
        }

        public Boolean isLeaf()
        {
            return (m_left == null && m_right == null);
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj == this)
                return 0;
            if (!(obj is BinaryTreeNode<E>))
                throw new ArgumentException("comparing obj is not a BinaryTree<E>");

            BinaryTreeNode<E> btn = obj as BinaryTreeNode<E>;
            return this.m_data.CompareTo(btn.m_data); //Compares the generic data value stored in the BinaryTreeNode<E>
        }//end CompareTo method

    }
}
