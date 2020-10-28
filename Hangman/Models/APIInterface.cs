using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangman.Models
{
    public interface APIInterface
    {
        public Task<List<Words>> GetWords();
    }
}
