﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FastTextProcess.Entities
{
    public class Dict
    {
        internal const string FldnWord = "Word";
        internal const string FldnVect = "Vect";

        public long Id { get; set; }
        public string Word { get; set; }
        public byte[] Vect { get; set; }

        public static Dict Create(string str)
        {
            var sarr = str.Trim().Split(' ');
            var sfarr = new string[sarr.Length-1];
            Array.Copy(sarr, 1, sfarr, 0, sfarr.Length);
            var farr = Array.ConvertAll(sfarr, float.Parse);
            var barr = new byte[farr.Length * 4];
            Buffer.BlockCopy(farr, 0, barr, 0, barr.Length);
            return new Dict { Word = sarr[0], Vect = barr };
        }
    }
}