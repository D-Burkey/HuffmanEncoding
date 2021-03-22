using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanEncoding
{
    /// <summary>
    /// IMPORTANT: These are the requirements for the command line arguments.
    /// 
    /// args[0] - the file that is to be modified (either to be compressed or decompressed)
    /// args[1] - the runMode. Either "c" for compression or "d" for decompression
    /// 
    /// All command line arguments are required.
    /// It is important not to modify or move any generated files after compression.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //Variables
            string runMode;
            string inputFile;
            string outputFile;
            string encodingTableFile;
            string originalFileName;
            string originalFilePath;

            //If there are not two arguments
            if(args.Length != 2)
            {
                Console.WriteLine("ERROR: Invalid Arguments. Arguments must include file name (including full file path) and runmode (\"c\" for compress, \"d\" for decompress)");
            }
            //If the file does not exist
            else if(!File.Exists(args[0]))
            {
                Console.WriteLine("ERROR: File being manipulated does not exist.");
            }
            //If the file is empty (or null)
            else if(new FileInfo(args[0]).Length == 0)
            {
                Console.WriteLine("ERROR: Cannot manipulate empty file.");
            }
            //If the runmode does not equal "c" or "d" (compress or decompress)
            else if(args[1].Substring(0, 1).ToLower() != "c" && args[1].Substring(0, 1).ToLower() != "d")
            {
                Console.WriteLine("ERROR: Runmode argument invalid. Must be \"c\" for compress or \"d\" for decompress");
            }

            //If arguments are valid
            else
            {
                //set runmode and inputFile
                runMode = args[1].Substring(0, 1).ToLower();
                inputFile = args[0];

                //Get original file path
                originalFilePath = inputFile.Substring(0, inputFile.LastIndexOf(@"\") + 1);

                //If compressing
                if (runMode.Equals("c"))
                {
                    //Get original file name
                    originalFileName = inputFile.Substring(inputFile.LastIndexOf(@"\") + 1);
                    originalFileName = originalFileName.Substring(0, originalFileName.LastIndexOf("."));

                    //Create outputFile and encodingTableFile
                    outputFile = originalFilePath + originalFileName + "-Compressed.txt";
                    encodingTableFile = originalFilePath + originalFileName + "-EncodingTable.txt";

                    //Perform compression
                    CompressFile(inputFile, outputFile, encodingTableFile);
                }

                //If decompressing
                else if (runMode.Equals("d"))
                {
                    if (!inputFile.Contains("-Compressed"))
                    {
                        Console.WriteLine("ERROR: Trying to decompress an uncompressed file.");
                    }
                    else
                    {
                        //Get original file name
                        originalFileName = inputFile.Substring(inputFile.LastIndexOf(@"\") + 1);
                        originalFileName = originalFileName.Substring(0, originalFileName.LastIndexOf("-"));

                        //Get outputFile and encodingTableFile
                        outputFile = originalFilePath + originalFileName + "-Decompressed.txt";
                        encodingTableFile = originalFilePath + originalFileName + "-EncodingTable.txt";

                        //Perform decompression
                        DecompressFile(inputFile, outputFile, encodingTableFile);
                    }
                }
                else
                    Console.WriteLine("RUNMODE ERROR: Runmode could not be determined.");
            }

            Console.WriteLine("Program Executed. Press any button to continue...");
            Console.ReadLine();

        }//end Main method

        /// <summary>
        /// This method simply utilizes five other methods to compress a file. It is used to maintain structure and neatness.
        /// </summary>
        /// <param name="inputFilePath">File to be compressed</param>
        /// <param name="outputFilePath">File location to put the compressed file</param>
        /// <param name="encodingTablePath">The file that stores the character encoding</param>
        static void CompressFile(string inputFilePath, string outputFilePath, string encodingTablePath)
        {
            //1. Count characters in file
                //Input: file location
                //Output: array of CharacterFrequences
            var cfArray = BuildCharArray(inputFilePath);

            //2. Create OrderedLinkedList
                //Input: array of CharacterFrequences
                //Output: OrderedLinkedList holding BinaryTreeNodes holding CharacterFrequences
            var CharOLL = BuildCharOLL(cfArray);

            //3. Buid encoding tree
                //Input: OrderedLinkedList holding BinaryTreeNodes holding CharacterFrequences
                //Output: BinaryTree object with fully structured and organized nodes
            var encodingTree = BuildEncTree(CharOLL);

            //4. Generate encoding table
                //Input: encoding tree, encodingTablePath
                //output: array holding CharacterEncoding objects that represent the recieved encoding tree (also print this encoding table to file)
                    //Similar to array of CF. Index for char is ASCII value of char
            var encodingTable = BuildEncTable(encodingTree, encodingTablePath);

            //5. Compress the file
                //Input: the encoding table, binary tree, and file path
                //Output: (new compressed file)
            EncodeFile(encodingTable, inputFilePath, outputFilePath);

        }//end CompressFile method



        /// <summary>
        /// This method simply utilizes three other methods to decompress a file. It is used to maintain structure and neatness.
        /// </summary>
        /// <param name="inputFilePath">The file to be decompressed</param>
        /// <param name="outputFilePath">The location to put the decompressed file</param>
        /// <param name="encodingTablePath">The file that stores the character encoding</param>
        static void DecompressFile(string inputFilePath, string outputFilePath, string encodingTablePath)
        {
            //1. Extract the encoding table
            //Input: encoding table file
            //Output: array of CharacterEncoding values
            var encodingTable = BuildEncTable(encodingTablePath);

            //2. Build encoding tree
            //Input: encodingTable
            //Output: formatted BinaryTree
            var encodingTree = BuildEncTree(encodingTable);

            //3. Uncompress the file
            //input: inputfile, outputfile
            //output: (new uncompressed file)
            DecodeFile(encodingTree, inputFilePath, outputFilePath, encodingTablePath);

        }//end DecompressFile method



        //
        //COMPRESSION METHODS
        //


        //
        //
        //Compression #1
        //This counts the character frequency of the input file and puts that data into an array. It then returns this array of character frequency objects.
        static Array BuildCharArray(string inputFilePath)
        {
            CharacterFrequency[] cfArray = new CharacterFrequency[256];
            FileStream fileRead = new FileStream(inputFilePath, FileMode.Open);

            //This fills each element in the CharacterFrequency array with its respective ASCII value. The index of the element is the ASCII value.
            for (int i = 0; i < cfArray.Length; ++i)
            {
                //Converts the array element to its respective ASCII character value.
                cfArray[i] = new CharacterFrequency(Convert.ToChar(i));
            }

            //for loop iterates once per character in the receieved file.
            long fileLength = fileRead.Length;
            for (int i = 0; i < fileLength; ++i)
            {
                //Creates a temporary CharacterFrequency object that stores the next read byte.
                CharacterFrequency tempCF = new CharacterFrequency(Convert.ToChar(fileRead.ReadByte()));

                //Creates a temporary int that stores the ASCII value found in the tempCF CharacterFrequency object.
                int tempASCII = Convert.ToInt32(tempCF.GetCh());

                //Searches the array that holds CharacterFrequency objects. The index for the search is the ASCII value for the character in the temporary CharacterFrequency object.
                if (cfArray[tempASCII].Equals(tempCF) == true)
                {
                    //If the CharacterFrequency object is found in the CharacterFrequency array, the object is incremented.
                    cfArray[tempASCII].Increment();
                }
            }

            fileRead.Close();
            return cfArray;

        }//end BuildCharArray method



        //
        //Compression #2
        //This uses the array of character frequency objects and encapsolates each object in a BinaryTree object that is in an OrderedLinkedList object. (i.e. OrderedLinkedList<BinaryTree<CharacterFrequency>>)
        //The output of this method is an OrderedLinkedList of BinaryTree objects containing CharacterFrequency objects. The list is ordered from most infrequent characters to most frequent.
        static OrderedLinkedList<BinaryTree<CharacterFrequency>> BuildCharOLL(Array cfArray)
        {
            OrderedLinkedList<BinaryTree<CharacterFrequency>> oll = new OrderedLinkedList<BinaryTree<CharacterFrequency>>();
            BinaryTree<CharacterFrequency> bt = new BinaryTree<CharacterFrequency>();

            //This loops once for every element in the Array filled withCharacterFrequency
            foreach(CharacterFrequency element in cfArray)
            {
                //If the CF element's frequency is zero, skip adding it to the OrderedLinkedList
                if(element.GetFrequency() != 0)
                {
                    //Create a new tree with the CharacterFrequency object. First parameter - what is being inserted. Second parameter - where in the tree it should be inserted to.
                    bt = new BinaryTree<CharacterFrequency>();
                    bt.Insert(element, BinaryTree<CharacterFrequency>.Relative.root);
                    //Add this new CharacterFrequency object to the list (this object is a Binary tree with only a value for root. They will be combined later).
                    oll.Add(bt);
                }
            }
            return oll;

        }//end BuildCharOLL method



        //
        //
        //Compression #3
        //This creates a binary tree using the recieved OrderdLinkedList<BinaryTree<CharacterFrequency>> object. This binary tree combines the first two objects in the OrderedLinkedList and puts it back into the OLL in its appropriate spot.
        //When combining the first two BT objects, the first object becomes the left child, the second becomes the right child, and a new BT object is created that sums the character frequency of the children. This object becomes the root.
        static BinaryTree<CharacterFrequency> BuildEncTree(OrderedLinkedList<BinaryTree<CharacterFrequency>> oll)
        {
            //While there is more than one object in the OrderedLinkedList
            while(oll.Count() != 1)
            {
                //Creates two temporary BinaryTree<CharacterFrequency> objects
                BinaryTree<CharacterFrequency> bt1 = null;
                BinaryTree<CharacterFrequency> bt2 = null;

                //These two BT objects refererence the first two objects in the OrderedLinkedList and the first two objects in the OLL are removed
                bt1 = oll.First.Value;
                oll.RemoveFirst();
                bt2 = oll.First.Value;
                oll.RemoveFirst();

                //Creates a new CharacterFrequency object based on the two CF objects stored in the two BinaryTree objects taken from the OrderedLinkedList. Sets the character to '\0' and the frequency to the sum of both CF frequencies
                int newFrequency = bt1.Current.Data.GetFrequency() + bt2.Current.Data.GetFrequency();
                CharacterFrequency rootCF = new CharacterFrequency('\0', newFrequency);

                //Creates a new BinaryTree object whose root value is the newly created CF object with the '\0' character
                //Adds the first BT object as the left child of this new BinaryTree object and adds the second BT object as the right child
                BinaryTree<CharacterFrequency> tree = new BinaryTree<CharacterFrequency>();
                tree.Insert(rootCF, BinaryTree<CharacterFrequency>.Relative.root);
                tree.Insert(bt1.Root, BinaryTree<CharacterFrequency>.Relative.leftChild);
                tree.Insert(bt2.Root, BinaryTree<CharacterFrequency>.Relative.rightChild);
                //Adds this new BT object that combined the first two objects in the OrderedLinkedList back into the OLL
                oll.Add(tree);

                //Resets the values
                rootCF = null;
                tree = null;
                bt1 = null;
                bt2 = null;
            }
            //Returns the completed BinaryTree object holding all the CharacterFrequency objects.
            return oll.First.Value; //The tree has the most frequent nodes on the left and the least frequent on the right

        }//end BuildEncTree method



        //
        //
        //Compression #4
        //This method builds an array of CharacterEncoding objects based on the recieved binary tree. It also writes all this CharacterEncoding data to the encoding table path.
        static CharacterEncoding[] BuildEncTable(BinaryTree<CharacterFrequency> encodingTree, string encodingTablePath)
        {
            //Array of CharacterEncoding objects. This is build using the BuildEncodingTable method in the BinaryTree class.
            CharacterEncoding[] encodingTable = encodingTree.BuildEncodingTable(encodingTree.Root) as CharacterEncoding[];

            //This adds the total number of characters as the first value for the output. (e.g. if there are 25 characters in the inputFile, the first line in the encodingTable file would be "25" represenging the total number of characters).
            string output = encodingTree.Root.Data.GetFrequency() + Environment.NewLine; //The first row in the output is the total number of characters being encoded
            //For every CharacterEncoding object in the CharacterEncoding table
            foreach(CharacterEncoding obj in encodingTable)
            {
                //If the object is not null (else skip it)
                if(obj != null)
                {
                    //add a new line containing the character and its encoding (e.g. the format could look something like this "A<>100101" with the <> as a seperator)
                    output = output + obj.GetCharacter() + "<>" + obj.GetEncoding() + Environment.NewLine; //The rest of the output is individual lines that comprise the character and the characters encoding

                }
            }
            //Write the character encoding data to the encoding table file
            File.WriteAllText(encodingTablePath, output);
            //Return the Array of CharacterEncoding objects
            return encodingTable;
        }//end BuildEncTable



        //
        //
        //Compression #5
        //This method actually encodes the input file. It does this using the array of CharacterEncoding objects. 
        //Basically, it uses the current character encoding and writes a bit with it. If the current number in the current character encoding is a 1, write it to the current bit and move to the next bit. If it is a 0, move to the next bit. 
        //If the bit is full, write the byte to the file and start over with a new, empty byte. If the current character encoding string has been fully used, move on to the next character.
        static void EncodeFile(CharacterEncoding[] encodingTable, string inputFilePath, string outputFilePath)
        {
            FileStream inFile = new FileStream(inputFilePath, FileMode.Open);
            FileStream outFile = new FileStream(outputFilePath, FileMode.Create);

            int charCounter = 0; //The number of characters that have been encoded
            int bitPosition = 7; //Start from the left of the bit
            int encodePosition = 0; //Start from the left of the encode value
            byte b = 0; //The current byte being worked on before being added to the file
            long fileLength = inFile.Length; //The number of characters in the file
            string currentCharEnc = String.Empty; //The current character encoding for the character read from the file

            if(fileLength != 0)
                currentCharEnc = encodingTable[(char)inFile.ReadByte()].GetEncoding();

            while (charCounter < fileLength) //Do until the number of encoded characters equal the total number of characters.
            {
                //If the bit is full - add the encoded byte to the encodedString and reset the bit and the bit position
                if (bitPosition < 0)
                {
                    outFile.WriteByte(b); //Write byte to file
                    b = 0; //reset the byte being worked on
                    bitPosition = 7; //reset the bit position
                }

                //If the next bit should be turned on
                if (currentCharEnc[encodePosition] == '1')
                    b = (byte)(b | (int)Math.Pow(2, bitPosition)); //Turn on the bit

                ++encodePosition; //move to the next bit position

                //If the end of the character encoding string has been reached (i.e. if the character encoding has been fully added)
                if (!(encodePosition < currentCharEnc.Length))
                {
                    //the character has been added to the string of bits
                    ++charCounter; //increase the encoded character counter

                    //If the next character from file is not EOF
                    if (!(charCounter == fileLength))
                        //move to the current charcters encoding
                        currentCharEnc = encodingTable[(char)inFile.ReadByte()].GetEncoding();

                    //reset the encoding position
                    encodePosition = 0;
                }

                --bitPosition; //move to the next bit position
            }//end totalFrequency while loop

            //Writes a partially encoded byte to the file.
            outFile.WriteByte(b); 

            inFile.Close();
            outFile.Close();
        }//end EncodeFile method



        //
        //DECOMPRESSION METHODS
        //


        //
        //
        //Decompression #1
        //This method simply parses through the encoding table file and creates a list of CharacterEncoding objects.
        static List<CharacterEncoding> BuildEncTable(string EncodingTablePath)
        {
            string encFile = File.ReadAllText(EncodingTablePath);
            List<CharacterEncoding> encodingTable = new List<CharacterEncoding>();

            string[] splitNL = { "\r\n" }; //This is a string that represents where to split (it is used in the next line.
            string[] encLines = encFile.Split(splitNL, StringSplitOptions.RemoveEmptyEntries); //Splits the file based on each new line.
            string[] splitMid = { "<>" }; //Splits each new line based on the "<>" values

            //loops once for every line in the file except the first line (which represents the number of encoded characters)
            for (int i = 1; i < encLines.Count(); ++i) 
            {
                string[] encodingData = encLines[i].Split(splitMid, StringSplitOptions.RemoveEmptyEntries); //Splits each line from the file (does not split the first line which represents the number of encoded characters)

                //Creates a new CharacterEncoding object and adds it to the list
                encodingTable.Add(new CharacterEncoding(Convert.ToChar(encodingData[0]), encodingData[1])); //ED[0] represents the character, ED[2] represents the character encoding of that charater
            }//end foreach

            return encodingTable;
        }//end BuildEncTable method



        //
        //
        //Decompression #2
        //This method creates the binary tree based on the character encoding. Basically, if the current character in the character encoding is a 1, move left in the tree. If it is a 0, move right in the tree. 
        //If the node doesn't exist, create it.
        static BinaryTree<char> BuildEncTree(List<CharacterEncoding> encodingTable)
        {
            BinaryTree<char> encodingTree = new BinaryTree<char>('\0'); //the new binary tree that will be developed later in this method.

            //for every CharacterEncoding object in the encoding table
            foreach(CharacterEncoding charEnc in encodingTable)
            {
                //each object being worked with to build the tree has to be instantiated, otherwise it is skipped.
                if(!(charEnc == null))
                {
                    encodingTree.Current = encodingTree.Root; //start at the top of the tree
                    int encLength = charEnc.GetEncoding().Length; //the length of the encoding string

                    //use every encoding character (i.e. string of 1s and 0s) in the current CharacterEncoding object to create the tree.
                    for (int i = 0; i + 1 <= encLength; ++i) //for loop executes for every character in the encoding string
                    {
                        char c = charEnc.GetEncoding()[i]; //the current character in the encoding string.

                        //1 - move left in the tree.
                        if (c == '1')
                        {
                            //if left doesn't exist, create it.
                            if (encodingTree.Current.Left == null)
                            {
                                //If the node should be a root node (i.e. if the end of the character encoding string has been reached)
                                if (i + 1 == encLength) 
                                    encodingTree.Insert(charEnc.GetCharacter(), BinaryTree<char>.Relative.leftChild);
                                //If node shouldn't be a root node
                                else
                                    encodingTree.Insert('\0', BinaryTree<char>.Relative.leftChild);
                            }
                            //move left
                            encodingTree.moveTo(BinaryTree<char>.Relative.leftChild);
                        }

                        //0 - move right in the tree.
                        else if (c == '0') 
                        {
                            //if right doesn't exist, create it.
                            if (encodingTree.Current.Right == null)
                            {
                                //If the node should be a root node (i.e. if the end of the character encoding string has been reached)
                                if (i + 1 == encLength)
                                    encodingTree.Insert(charEnc.GetCharacter(), BinaryTree<char>.Relative.rightChild);
                                //If node shouldn't be a root node
                                else
                                    encodingTree.Insert('\0', BinaryTree<char>.Relative.rightChild);
                            }
                            //move right
                            encodingTree.moveTo(BinaryTree<char>.Relative.rightChild);
                        }
                    }//end charEncoding.GetEncoding() for loop
                }
            }//end encodingTable foreach loop

            return encodingTree;
        }//end BuildEncTree method



        //
        //
        //Decompression #3
        //This method traverses the binary tree that was created based off of input from the file. It reads a byte from the file and finds that bytes character encoding. 
        //It then traverses the binary tree based on the encoding (moving left if 1, moving right if 0). If the current node in the tree is a leaf node, it writes the node character value to the output file (decompressed file) and returns to the root.
        //This method stops when the number of decompressed characters matches the number of characters in the original file (this is found at the top of the encoding table file).
        static void DecodeFile(BinaryTree<char> encodingTree, string inputFilePath, string outputFilePath, string EncodingTablePath)
        {
            try
            {
                //determins the number of characters to be decoded.
                StreamReader encFile = new StreamReader(EncodingTablePath);
                int encodedCharCount = Convert.ToInt32(encFile.ReadLine()); //The first line in the EncodingTable file is the count of characters to be decoded.
                encFile.Close();

                FileStream inFile = new FileStream(inputFilePath, FileMode.Open);
                FileStream outFile = new FileStream(outputFilePath, FileMode.Create);

                //Move to the root
                encodingTree.Current = encodingTree.Root;

                byte by = 0;
                //if input file is not empty
                if (inFile.Length != 0)
                    by = Convert.ToByte(inFile.ReadByte()); //convert the first character in the file to a byte
                //if input file is empty throw exception
                else
                    throw new Exception("File being decompressed is empty.");
                int bit;
                int bitPosition = 7; //Start from the left of the bit
                int decodedCharCount = 0; //Number of decoded characters

                //While there are bits to decode
                while (decodedCharCount < encodedCharCount)
                {
                    //Check to see if we are still within the limits of the byte (i.e. bit positions 7 - 0)
                    if (bitPosition < 0)
                    {
                        //If not
                        //Read next byte from the file
                        by = Convert.ToByte(inFile.ReadByte());
                        //Start at the left of the byte
                        bitPosition = 7;
                    }

                    //Check to see if the current bit is on or off
                    bit = by & (int)Math.Pow(2, bitPosition); //if on, returns [value > 0]. if off, returns [0];

                    //If bit is on (bit > 0) - move left
                    if (bit > 0)
                    {
                        //Move left
                        encodingTree.moveTo(BinaryTree<char>.Relative.leftChild);
                        //If node is a leaf node
                        if (encodingTree.Current.isLeaf())
                        {
                            //Add the character to the file
                            outFile.WriteByte(Convert.ToByte(encodingTree.Current.Data));
                            //Move back to the root of the tree
                            encodingTree.moveTo(BinaryTree<char>.Relative.root);
                            //Increase the decoded character count
                            ++decodedCharCount;
                        }
                    }

                    //If bit is off (bit == 0) - move right
                    else if (bit == 0)
                    {
                        //Move right
                        encodingTree.moveTo(BinaryTree<char>.Relative.rightChild);
                        //If node is a leaf node
                        if (encodingTree.Current.isLeaf())
                        {
                            //Add the character to the file
                            outFile.WriteByte(Convert.ToByte(encodingTree.Current.Data));
                            //Move back to the root of the tree
                            encodingTree.moveTo(BinaryTree<char>.Relative.root);
                            //Increase the decoded character count
                            ++decodedCharCount;
                        }
                    }

                    //move to the next bit in the byte
                    --bitPosition;
                }

                inFile.Close();
                outFile.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("Error: file(s) corrupted. Decompression failed.");
                Console.WriteLine(e.Message);
            }
            
        }//end DecodeFile method
    }//end Program class
}//end namespace
