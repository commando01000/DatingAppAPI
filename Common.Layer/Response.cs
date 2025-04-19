namespace Common.Layer
{
    public class Response<T> where T : class
    {
        public bool Status { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string RedirectURL { get; set; }
        public T? Data { get; set; }
    }
}
