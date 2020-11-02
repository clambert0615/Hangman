using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Hangman.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> GetWord()
        {
            List<Words> wordList = await _api.GetWords();
            Random random = new Random();
            int index = random.Next(30);
            w.Word = wordList[index].Word;
            return View(w);
        }
        [Authorize]
        public IActionResult Guess(string word, char guess, int guessesLeft)
        {
            char[] encrypted = new char[word.Length];
            List<char> guessed = new List<char>();
            g = AnalyzeGuess(word, guess, guessesLeft, encrypted, guessed);
            return View(g);
        }
        [Authorize]
        public IActionResult RepeatedGuess(Guess modguess, char guess, string wordGuess)
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Record user = _context.Record.FirstOrDefault(x => x.UserId == id);
            if (wordGuess != null)
            {
                if (wordGuess == modguess.Word)
                {
                    ViewBag.Status = 1;
                    ViewBag.Message = $"Game Over, You Won!  The word was {modguess.Word}";
                    Record r = new Record { UserId = id, Wins = 1 };
                    _context.Add(r);
                    _context.SaveChanges();
                    return View(g);
                }
                else
                {
                    ViewBag.Status = 2;
                    ViewBag.Message = $"Game Over, Your guess was incorrect.  The word was {modguess.Word}";
                    Record r = new Record { UserId = id, Losses = 1 };
                    _context.Add(r);
                    _context.SaveChanges();
                    return View(g);
                }
            }
           else 
            {
                g = AnalyzeGuess(modguess.Word, guess, modguess.GuessesLeft, modguess.EncryptedWord, modguess.Guessed);

                if ((g.GuessesLeft <= 0) && (wordGuess != modguess.Word))
                {
                    ViewBag.Status = 3;
                    ViewBag.Message = $"Game Over, You are out of guesses!  The word was {modguess.Word}";
                     Record r = new Record { UserId = id, Losses = 1 };
                    _context.Add(r);
                    _context.SaveChanges();
                    return View(g);
                }
                else if(g.EncryptedWord.ToString() == g.Word)
                {
                    ViewBag.Status = 1;
                    ViewBag.Message = $"Game Over, You Won!  The word was {modguess.Word}";
                    Record r = new Record { UserId = id, Wins = 1 };
                    _context.Add(r);
                    _context.SaveChanges();
                    return View(g);
                }
                else
                {
                    return View(g);
                }
            }
            
            
        }

        public Guess AnalyzeGuess(string word, char guess, int guessesLeft, char[] encrypted, List<char> guessed)
        {
            if (encrypted == null)
            {
                for (int i = 0; i < word.Length; i++)
                {
                    encrypted[i] = '*';
                }
            }
            if (guessed.Contains(guess))
            {
                g.DupeMessage = $"You already guessed the letter {guess}, please guess a different letter";
                g.GuessesLeft = guessesLeft;
                g.Guessed = guessed;
                g.EncryptedWord = encrypted;
                g.Word = word;
                return g;

            }
            else
            {
                for (int j = 0; j < word.Length; j++)
                {
                    if (guess == word[j])
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
                return g;
            }
        }
        [Authorize]
        public IActionResult Statistics()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Record user = _context.Record.FirstOrDefault(x => x.UserId == id);
            int? wins = 0;
            int? losses = 0;
            if (user.UserId != null)
            {
                List<Record> userRecord = _context.Record.Where(x => x.UserId == id).ToList();
                foreach(Record rec in userRecord)
                {
                    wins += rec.Wins ?? 0;
                    losses += rec.Losses ?? 0;
                }

                Record r = new Record { Wins = wins ?? 0, Losses = losses ?? 0};
                return View(r);
            }
            else
            {
                ViewBag.Message = "You have no statistics yet.";
                return View();
            }
        }

    }
}
