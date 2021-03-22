using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HuffmanEncoding
{
    /// <summary>
    /// This is a very simple data structure. It simply holds a character and its encoding. It uses a regular expression to ensure that the encoding is in the right format.
    /// </summary>
    public class CharacterEncoding
    {
        private Regex encodingFormat = new Regex(@"^(0|1)*$");

        private char _character;
        private string _encoding;

        public CharacterEncoding()
        {
            _character = '\0';
            _encoding = string.Empty;
        }

        public CharacterEncoding(char rCharacter, string rEncoding)
        {
            SetCharacter(rCharacter);
            SetEncoding(rEncoding);
        }

        //GETTERS/SETTERS
        public char GetCharacter() { return _character; }
        public void SetCharacter(char rCharacter) { _character = rCharacter; }
        public string GetEncoding() { return _encoding; }
        public void SetEncoding(string rCharacter)
        {
            if (encodingFormat.IsMatch(rCharacter))
            {
                _encoding = rCharacter;
            }
            else
                Console.WriteLine($"Invalid encoding for \'{this._character}\'");
        }

    }//end CharacterEncoding class
}//end namespace
