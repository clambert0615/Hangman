﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.Models
{
    public class Guess
    {
        public string Word { get; set; }
        public char[] EncryptedWord { get; set; }
        public List<char>  Guessed { get; set; }

        public int GuessesLeft { get; set; }
        public string DupeMessage { get; set; }
    }
}
