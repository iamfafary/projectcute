namespace Gradpath
{
    public class AlertMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string Cancel { get; set; }

        public AlertMessage(string title, string message, string cancel)
        {
            Title = title;
            Message = message;
            Cancel = cancel;
        }
    }
}
