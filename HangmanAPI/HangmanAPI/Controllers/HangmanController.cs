using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HangmanAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HangmanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangmanController : ControllerBase
    {
        private readonly HangmanAPIDbContext _context;
        public HangmanController(HangmanAPIDbContext context)
        {
            _context = context;
        }
        //Get all words with endpoint api/hangman
        [HttpGet]
        public async Task<ActionResult<List<Words>>> GetWords()
        {
            List<Words> wordList = await _context.Words.ToListAsync();
            return wordList;
        }
        //Get one word with endpoing api/hangman/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Words>> GetWord(int id)
        {
            Words word = await _context.Words.FindAsync(id);
            if(word == null)
            {
                return NotFound();
            }
            else
            {
                return word;
            }
        }
        //Add a word 
        [HttpPost]
        public async Task<ActionResult<Words>> AddWord(Words newWord)
        {
            if(ModelState.IsValid)
            {
                _context.Words.Add(newWord);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetWord), new { id = newWord.WordId }, newWord);

            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut]
        public async Task<ActionResult> UpdateWord (Words updatedWord)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                _context.Entry(updatedWord).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePlanet(int id)
        {
            Words word = await _context.Words.FindAsync(id);
            if(word == null)
            {
                return NotFound();
            }
            else
            {
                _context.Words.Remove(word);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

    }
}
