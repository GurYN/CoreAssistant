# CoreAssistant 
A simple library enabling the power of [ChatGPT](https://chat.openai.com) & others services from OpenAI in your application.

The library is in active development, stay connected to get new features!

[![Build library](https://github.com/GurYN/CoreAssistant/actions/workflows/build-library.yml/badge.svg?branch=main)](https://github.com/GurYN/CoreAssistant/actions/workflows/build-library.yml) 
[![Nuget](https://img.shields.io/nuget/v/VinciDev.CoreAssistant)](https://www.nuget.org/packages/VinciDev.CoreAssistant/)


# Playground examples
Small app examples are provided to test the library :

## [ConsoleChat](playground/ConsoleChat)
A simple console app allowing you to interact in a chat mode
![ConsoleChat](documentation/_assets/ConsoleChat.png)

To test ConsoleChat example:
```bash
cd playground
cd ConsoleChat

dotnet restore
dotnet run
```

## [SmartShop](playground/SmartShop)
A web app allowing you to generate title, description, & image of a product based on keywords
![SmartShop](documentation/_assets/SmartShop.png)

To test SmartShop example:

1/ Duplicate the [appsettings.json](playground/SmartShop/appsettings.json) file and rename it as `appsettings.Development.json`

2/ Update the parameter `ApiKey` in the new file created with your OpenAI Api key

3/ Launch the app
```bash
cd playground
cd SmartShop

dotnet restore
dotnet run

# open your browser to the url displayed
```

# Install the library
You can install the library using nuget
```bash
dotnet add package VinciDev.CoreAssistant
```

# Quick Start
You can use the library directly or using dependency injection.

## 1/ Using directly
```csharp
using CoreAssistant;
using CoreAssistant.Models;

...

var options = new CoreAssistantOptions("YOUR OPENAI API KEY");
var assistant = new Assistant(options);

// Get an answer from ChatGPT Api
var question = new ChatQuestion("You question");
var answer = await assistant.Chat.AskForSomething(question);

Console.WriteLine(answer.Content);

// Generate an image with Dall-E Api
var prompt = new ImagePrompt("A black cat walking on a street during the night");
var result = await assistant.Image.Generate(prompt);

Console.WriteLine($"Url of the image: {result.Url}");
```

## 2/ Using dependency injection
In your Program.cs :

```csharp
using CoreAssistant.Extensions;

...

builder.Services.AddCoreAssistant(options => { 
    options.ApiKey = "YOU OPENAI API KEY"
});
```
__Warning__ : Do not store your API key in source code. Use `appsettings.json` instead.

In a class of your project :
```csharp
using CoreAssistant;
using CoreAssistant.Models;

public class ClassName
{
    private readonly Assistant _assistant;

    public ClassName(Assistant assistant)
    {
        _assistant = assistant;
    }

    // Get a ChatGPT answer
    public async Task<string> GetAnswer(string query)
    {
        var question = new ChatQuestion(query);
        var answer = await _assistant.Chat.AskForSomething(question);

        return answer.Content;
    }

    // Get a Dall-E image
    public async Task<string> GenerateImage(string query)
    {
        var prompt = new ImagePrompt(query);
        var result = await _assistant.Image.Generate(prompt);

        return result.Url;
    }
}
````

# Advanced use
## History context
The library will keep the conversation history during the lifecycle of your CoreAssistant instance. You can then ask any question and get answers based on the entire history (like ChatGPT website).

## Default Context
You can define a default context to specialize the answers of your assistant. To do so, just add the default context in your `CoreAssistantOptions` object. 

Ex:
```csharp
var options = 
    new CoreAssistantOptions("YOUR OPENAI API KEY") {
        DefaultContext = "YOUR DEFAULT CONTEXT"
    };
```

## Async vs Stream for Chat answer
You can use async or stream method to receive an answer. To do so, just call the right method based on your desired result.

Ex:
```csharp
using CoreAssistant;
using CoreAssistant.Models;

...

var options = new CoreAssistantOptions("YOUR OPENAI API KEY");
var assistant = new Assistant(options);
var question = new ChatQuestion("You question");

# Async call
var answer = await assistant.Chat.AskForSomething(question);
Console.WriteLine(answer.Content);

# Stream call
var stream = assistant.Chat.AskForSomethingAsStream(question);
await foreach (var item in stream)
{
    Console.Write(item.Content);
}
```

## ChatGPT Model
You can define the GPT model used by the library. To do so, set it when calling `AskForSomething()` or `AskForSomethingAsStream()` method.

__Note__: GPT-4 model access is restricted, join the [waiting list](https://openai.com/waitlist/gpt-4-api) to access it.

Ex:
```csharp
using CoreAssistant;
using CoreAssistant.Models;

...

var question = new ChatQuestion("Your question");

# Async call
var answer = await assistant.Chat.AskForSomething(question, ChatModel.GPT3_5);

# Stream call
var stream = assistant.Chat.AskForSomethingAsStream(question, ChatModel.GPT4);
```
