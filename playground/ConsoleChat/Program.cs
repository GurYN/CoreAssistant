using CoreAssistant;

Console.Clear();

Console.WriteLine("Welcome to your Chat Assistant");
Console.WriteLine("------------------------------");
Console.WriteLine();

Console.WriteLine("Please give your OpenAI Api Key:");

var apiKey = Console.ReadLine();
Console.WriteLine();
if (string.IsNullOrEmpty(apiKey))
{
    Console.WriteLine("You must type your OpenAI Api Key");
    return;
}

Console.WriteLine("Type your default context (or leave empty):");
var context = Console.ReadLine();
Console.WriteLine();

var options = new CoreAssistantOptions(apiKey);
if (!string.IsNullOrEmpty(context))
{
    options.DefaultContext = context;
}
var assistant = new Assistant(options);

while (true) 
{
    Console.WriteLine("Type your question (or clear, or exit):");
    var question = Console.ReadLine();

    if (string.IsNullOrEmpty(question))
    {
        Console.WriteLine("You must type a question");
        continue;
    }

    if (question.ToLower() == "clear")
    {
        assistant.ClearHistory();
        Console.WriteLine();
        Console.WriteLine("History cleared");
        Console.WriteLine();
        continue;
    }

    if (question.ToLower() == "exit")
    {
        Console.WriteLine();
        Console.WriteLine("Bye bye");
        break;
    }

    Console.WriteLine();

    var answer = assistant.AskForSomethingAsStream(new Question(question));
    Console.WriteLine("Answer from your assistant:");
    await foreach (var item in answer)
    {
        Console.Write(item.Content);
    }

    Console.WriteLine();
    Console.WriteLine();
}

