namespace bitirmeProje.Dto
{
    public class Response<TResult> where TResult : class
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public TResult Result { get; set; }
    }
}
