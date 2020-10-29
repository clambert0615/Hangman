using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangman.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hangman.Controllers
{
    public class HangmanController : Controller
    {
        private readonly HangmanContext _context;
        private readonly APIInterface _api;
        public Words w = new Words();
        public Guess g = new Guess();
        public HangmanController(HangmanContext context, APIInterface api)
        {
            _context = context;
            _api = api;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetWord()
        {
            List<Words> wordList = await _api.GetWords();
            Random random = new Random();
            int index = random.Next(30);
            w.Word = wordList[index].Word;
            return View(w);
        }
        
        public IActionResult Guess(string word, char guess, int guessesLeft)
        {
            StringBuilder encrypted = new StringBuilder(word.Length);
            List<char> guessed = new List<char>();
           for (int i = 0; i < word.Length; i++)
            {
                encrypted.Append('*');
            }
            for (int j = 0; j < word.Length; j++)
            {
                if(guess == word[j])
                {
                    encrypted[j] = guess;
                }
            }
            guessed.Add(guess);
            guessesLeft -= 1; 
            g.Guessed = guessed;
            g.Word = word;
            g.EncryptedWord = encrypted;
            g.GuessesLeft = guessesLeft;
            return View(g);
        }

    }
}
