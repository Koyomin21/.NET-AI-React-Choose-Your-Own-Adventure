namespace Options;

public class OllamaOptions
{
    public string ClientName { get; set; } = string.Empty;
    public string BaseAdress { get; set; } = string.Empty;
    public string ChatModel { get; set; } = string.Empty;
    public int TimeoutInSeconds { get; set; } = 120;
}