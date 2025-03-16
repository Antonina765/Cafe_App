namespace Cafe_App.Services.Apis
{
    public class HttpNumberApi
    {
        public HttpClient _httpClient;

        public HttpNumberApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetFactAsync(int number)
        {
            return await _httpClient.GetStringAsync($"{number}");
        }

        public async Task<string> GetFactAboutDateAsync()
        {
            
            var today = DateTime.Now;
            int month = today.Month;
            int day = today.Day;
            
            string path = $"{month}/{day}/date";
            
            return await _httpClient.GetStringAsync(path);
        }
    }
}
