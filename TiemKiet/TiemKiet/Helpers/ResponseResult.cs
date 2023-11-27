namespace TiemKiet.Helpers
{
    public static class ResponseResult
    {
        public static object CreateResponse(string code, string message, object? data = null)
        {
            var response = new { status = code, message = message, Data = data };
            return response;
        }
    }
}
