# HuffmanEncoding
--SUMMARY--
  This program enables a user to compress and decompress text files using the huffman encoding algorithm.

--HOW TO USE--
  
  -OVERVIEW
  To compress: run the program with the first parameter being the text file being compressed (and its full file path) and the second parameter being the letter "c"
    e.g. HuffmanEncoding.exe fileNameAndPath.txt c
  To decompress: run the program with the first parameter being the compressed text file (and its full file path) and the second parameter being the letter "d"
    e.g. HuffmanEncoding.exe fileNameAndPath-Compressed.txt d

  -FULL DESCRIPTION
  This program has two command line arguments. The first is the file to be manipulated along with it's full file path, and the second is the runmode.
  The runmode consists of either "c" for compression and "d" for decompression.

  If the user wanted to compress a file, they would run the HuffmanEncoding program with the parameters being the file to compress and the letter "c"
  An example of this from the command line would look like this:
    HuffmanEncoding.exe C:/OriginalFile.txt c
  
  Assuming the file exists and has content, after the program is run two new files would be created.
  The first is the encoding table text file. This file contains the number of characters, all the encoded characters in the table, and the characters encoding.
  Using the previous example, the encoding table text file would be named:
    OriginalFile-EncodingTable.txt
  
  The second is the compressed text file. This file contains the compressed content of the original file.
  Using the previous example, the compressed file would be named:
    OriginalFile-Compressed.txt
  
  If the user wanted to decompress a previously compressed file, they would run the HuffmanEncoding program with the parameters being the file to decompress and the leter "d"
  An example of this from the command line would look like this:
    HuffmanEncoding.exe C:/OriginalFile-Compressed.txt d
  
  Assuming the file exists, has content, and the encoding table file also exists, one new file would be created.
  This file is the decompressed text file. This file would contain the exact same content as the original file.
  Using the previous example, the decompressed file would be named:
    OriginalFile-Decompressed.txt
  
--NOTES--
  It is important to keep the compressed file and the encoding table in the same file location and to not modify their names.
  Doing so would likely break the program, making it impossible to decompress the file.
