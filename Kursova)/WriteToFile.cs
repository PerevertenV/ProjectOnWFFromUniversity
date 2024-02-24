using System.IO;
using Kursova_;
public class WriteToFile : ITakePath// композиція
{
    public string pathToWrite, WhatToWrite;
    public int WhichClassToWrite;
   
    public WriteToFile(string whatToWrite, int whichClassToWrite)
    {
        pathToWrite = TakePath();
        WhatToWrite = whatToWrite;
        WhichClassToWrite = whichClassToWrite;
        if (WhichClassToWrite == 1) { pathToWrite += "/OrderFile.txt"; }
        else if (WhichClassToWrite == 2) { pathToWrite += "/AgronomistFile.txt"; }
        else if (WhichClassToWrite == 3) { pathToWrite += "/CustomerFile.txt"; }
        MethodToWrite();
    }

    public void MethodToWrite() 
    { using (var WTF = new StreamWriter(pathToWrite, true)){ WTF.WriteLine(WhatToWrite);}}
    public string TakePath() 
    {
        SMF ToUseInfo = new SMF();
        string pathFromRead = ToUseInfo.TakePath();
        return pathFromRead;
    }
}

