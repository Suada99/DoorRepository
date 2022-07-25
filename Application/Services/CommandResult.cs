namespace Application.Services
{
    public class CommandResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public int? TotalDataCount { get; set; }

        public CommandError CommandError { get; set; }
    }

}
