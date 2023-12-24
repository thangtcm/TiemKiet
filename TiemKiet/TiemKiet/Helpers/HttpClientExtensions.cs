using Newtonsoft.Json;

namespace TiemKiet.Helpers
{
    public static class HttpClientExtensions
    {
        public static async Task<T?> ReadContentAsync<T>(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode == false)
                throw new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");

            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            var result = JsonConvert.DeserializeObject<T>(dataAsString);

            return result;
        }
    }
}
