using System;
using System.Collections.Generic;
using System.Linq;
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
        
    }
}
