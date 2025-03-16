namespace Cafe_App.Services.Apis
{
    public class HttpJokeApi
    {
        private HttpClient _httpClient;
        public HttpJokeApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetRandomJokeAsync()
        {

            var response = await _httpClient.GetAsync(string.Empty);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при получении шутки: {response.StatusCode}");
            }

            // Десериализуем ответ в объект Joke
            var joke = await response.Content.ReadFromJsonAsync<Joke>();

            if(joke == null)
            {
                throw new Exception("Zero joke");
            }

            var jokeString = $"{joke.Setup} {joke.Punchline}";

            return jokeString;
        }
    }
    public class Joke
    {
        public string Type { get; set; }
        public string Setup { get; set; }
        public string Punchline { get; set; }
    }
}

