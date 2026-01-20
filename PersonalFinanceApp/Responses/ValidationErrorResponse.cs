namespace PersonalFinanceApp.Responses
{
    public class ValidationErrorResponse
    {
        public string Message { get; set; } = "Erro de validação";
        public Dictionary<string, string[]> Errors { get; set; } = new();
    }
}
