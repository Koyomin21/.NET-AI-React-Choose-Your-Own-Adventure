using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Ollama;
using Options;
using Persistence;
using Persistence.Entities;
using Services.Interfaces;

namespace Services.Implementations;

public class ChatService : IChatService
{
    private readonly AiOptions _options;
    private readonly ChatSettingsOptions _chatSettings;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ChatService> _logger;

    public ChatService(
        IOptions<AiOptions> options,
        IOptions<ChatSettingsOptions> chatSettings,
        IHttpClientFactory httpClientFactory,
        ILogger<ChatService> logger)
    {
        _options = options.Value;
        _chatSettings = chatSettings.Value;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<Story> GenerateStoryAsync(string theme, CancellationToken cancellationToken)
    {
        var httpClient = _httpClientFactory.CreateClient(_options.Ollama.ClientName);
        using var ollama = new OllamaApiClient(httpClient);

        try
        {
            var modelListResponse = await ollama.Models.ListRunningModelsAsync(); // or equivalent method

            _logger.LogInformation($"Models loaded: {string.Join(", ", modelListResponse?.Models?.Select(m => m.Model) ?? [])}");

            // Check if the model from appsettings exists(regardless the case)
            if (!modelListResponse.Models.Any(x => x.Model.Equals(_options.Ollama.ChatModel, StringComparison.OrdinalIgnoreCase)))
            {

                _logger.LogInformation($"Trying to pull the model: {_options.Ollama.ChatModel}");
                await ollama.Models.PullModelAsync(_options.Ollama.ChatModel, cancellationToken: cancellationToken).EnsureSuccessAsync();
                _logger.LogInformation($"Successfully pulled the model");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Ollama error: {ex}");
        }
        string outputStoryFormat = @"
        {
            ""title"": ""Story Title"",
            ""rootNode"": {
                ""content"": ""The starting situation of the story"",
                ""isEnding"": false,
                ""isWinningEnding"": false,
                ""options"": [
                    {
                        ""text"": ""Option 1 text"",
                        ""nextNode"": {
                            ""content"": ""What happens for option 1"",
                            ""isEnding"": false,
                            ""isWinningEnding"": false,
                            ""options"": [
                                // More nested options
                            ]
                        }
                    },
                    // More options for root node
                ]
            }
        }";
        string systemMessage = $@"You are a creative story writer that creates engaging choose-your-own-adventure stories.
                Generate a complete branching story with multiple paths and endings in the JSON format I'll specify.

                The story should have:
                1. A compelling title
                2. A starting situation (root node) with 2-3 options
                3. Each option should lead to another node with its own options
                4. Some paths should lead to endings (both winning and losing)
                5. At least one path should lead to a winning ending

                Story structure requirements:
                - Each node should have 2-3 options except for ending nodes
                - The story should be 3-4 levels deep (including root node)
                - Add variety in the path lengths (some end earlier, some later)
                - Make sure there's at least one winning path

                Output your story in this exact JSON structure:
                {outputStoryFormat}

                Don't simplify or omit any part of the story structure. 
                Don't add any text outside of the JSON structure.";

        // var chat = ollama.Chat(
        //     model: _options.Ollama.ChatModel,
        //     systemMessage: systemMessage
        // );

        try
        {

            var request = new GenerateCompletionRequest
            {
                Model = _options.Ollama.ChatModel,
                Prompt = $"Generate a story with a theme: {theme}",
                System = systemMessage,
                Stream = false,
                Format = ResponseFormatEnum.Json
            };

            _logger.LogInformation("Making request to Ollama: {request}", JsonSerializer.Serialize(request));
            var response = await ollama.Completions.GenerateCompletionAsync(request, cancellationToken: cancellationToken);

            _logger.LogInformation("Ollama response: {Response}", response.Response);

            var story = JsonSerializer.Deserialize<Story>(response.Response!);

            if (story == null)
            {
                _logger.LogError("Failed to deserialize story from response: {Response}", response.Response);
                throw new InvalidOperationException("Failed to generate a valid story.");
            }

            return story;
        }
        catch (Exception ex)
        {
            // TODO: Log error and find out why deserialization is not successful
        }
        finally
        {
            // chat.PrintMessages();
            _logger.LogInformation("Finished interaction with ollama");
        }


        return default;
    }
}