using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanEncoding
{
    public class CharacterFrequency : IComparable
    {
        private char ch;
        private int frequency;

        //CONSTRUCTORS
        public CharacterFrequency(char ch)
        {
            SetCh(ch);
            SetFrequency(0);
        }//end one variable constructor

        public CharacterFrequency(char ch, int frq)
        {
            SetCh(ch);
            SetFrequency(frq);
        }


        //GETTERS/SETTERS
        public char GetCh()
        {
            return ch;
        }//end GetCh method

        public void SetCh(char ch)
        {
            this.ch = ch;
        }//end SetCh method

        public int GetFrequency()
        {
            return frequency;
        }//end GetFrequency method

        public void SetFrequency(int frequency)
        {
            this.frequency = frequency;
        }//end SetFrequency


        //METHODS
        public void Increment()
        {
            ++frequency;
        }//end Increment method

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (this == obj)
            {
                return true;
            }
            if (!(obj is CharacterFrequency))
            {
                return false;
            }

            CharacterFrequency cf = obj as CharacterFrequency;
            return this.GetCh() == cf.GetCh();
        }//end Equals method

        public override string ToString()
        {
            return $"{GetCh()}" +
                $"({Convert.ToInt16(GetCh())})" +
                $"{GetFrequency(),7}";
        }//end ToString method

        public override int GetHashCode()
        {
            return GetCh();
        }//end GetHashCode method

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (obj == this)
                return 0;
            if (!(obj is CharacterFrequency))
                throw new ArgumentException("comparing obj is not a CharacterFrequency");

            CharacterFrequency cf = obj as CharacterFrequency;
            return this.GetFrequency().CompareTo(cf.GetFrequency()); //Compares the character frequency.
        }//end CompareTo method

    }//end CharacterFrequency class
}//end namespace
