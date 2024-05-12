using SlackDataAnonymizer.Repositories;
using SlackDataAnonymizer.Services;
using SlackDataAnonymizer.Services.Anonymizers;
using System.Text.Json;
using System.Text.Json.Serialization;

const string sourceMessagesFilePath = "C:\\dev\\slack-data-anonymizer\\data\\tech_community\\2024-05-06.json";
const string targetMessagesFilePath = "C:\\dev\\slack-data-anonymizer\\data\\tech_community\\2024-05-06-anonymized.json";
const string targetMessagesSensitiveDataFilePath = "C:\\dev\\slack-data-anonymizer\\data\\tech_community\\2024-05-06-sensitive-data.json";

var jsonSerializerOptions = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
var messagesReadRepository = new MessagesReadRepository(sourceMessagesFilePath, jsonSerializerOptions);
var messagesWriteRepository = new MessagesWriteRepository(targetMessagesFilePath, jsonSerializerOptions);
var sensitiveDataWriteRepository = new SensitiveDataWriteRepository(targetMessagesSensitiveDataFilePath, jsonSerializerOptions);

var repliesAnonymizerService = new RepliesAnonymizerService();
var reactionsAnonymizerService = new ReactionsAnonymizerService();
var textAnonymizerService = new TextAnonymizerService();

var messageAnonymizerService = new MessageAnonymizerService(
    textAnonymizerService,
    repliesAnonymizerService,
    reactionsAnonymizerService);

var messagesService = new MessagesService(
    messagesReadRepository,
    messagesWriteRepository,
    sensitiveDataWriteRepository,
    messageAnonymizerService);

await messagesService.AnonymizeMessagesAsync(CancellationToken.None);