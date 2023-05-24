namespace CoreAssistant.Models.Assistants;

public class ImagePrompt
{
    public string Content { get; set; } = string.Empty;

    public ImagePrompt(string content)
    {
        Content = content;
    }
}

public class ImageResult
{
    public string Url { get; set; } = string.Empty;
}

public class ImageSize
{
    private string Value { get; set; }
    private ImageSize(string value) => Value = value;

    public static ImageSize Resolution_256x256 { get => new ImageSize("256x256"); }
    public static ImageSize Resolution_512x512 { get => new ImageSize("512x512"); }
    public static ImageSize Resolution_1024x1024 { get => new ImageSize("1024x1024"); }

    public override string ToString()
    {
        return Value;
    }
}