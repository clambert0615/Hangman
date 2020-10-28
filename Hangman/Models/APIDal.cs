using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hangman.Models
{
    public class APIDal : APIInterface
    {
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44325/");
            return client;
        }
        public async Task<List<Words>> GetWords()
        {
            HttpClient client = GetClient();
            var response = await client.GetAsync("api/hangman");
            //Install-package Microsoft.AspNet.WebAPI.Client
            List<Words> wordList = await response.Content.ReadAsAsync<List<Words>>();
            return wordList;

        }
    }
}
