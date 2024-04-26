using System;

abstract class TextAnalyzer
{
    protected string text;

    public TextAnalyzer(string text)
    {
        this.text = text;
    }

    public abstract void Analyze();
}

class WordCounter : TextAnalyzer
{
    public int WordCount { get; private set; }

    public WordCounter(string text) : base(text) { }

    public override void Analyze()
    {
        WordCount = 0;
        bool inWord = false;

        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                if (!inWord)
                {
                    WordCount++;
                    inWord = true;
                }
            }
            else
            {
                inWord = false;
            }
        }
    }
}

class WordReverser : TextAnalyzer
{
    public string ReversedText { get; private set; }

    public WordReverser(string text) : base(text) { }

    public override void Analyze()
    {
        char[] reversedText = new char[text.Length];
        int wordStart = 0;

        for (int i = 0; i < text.Length; i++)
        {
            if (char.IsLetter(text[i]))
            {
                wordStart = i;
                while (i < text.Length && char.IsLetter(text[i]))
                {
                    i++;
                }

                int wordEnd = i - 1;

                for (int j = wordStart; j <= wordEnd; j++)
                {
                    reversedText[j] = text[wordEnd - (j - wordStart)];
                }
            }
            else
            {
                reversedText[i] = text[i];
            }
        }

        ReversedText = new string(reversedText);
    }
}

class Program
{
    static void Main()
    {
        string inputText = "Я плохая, ты хороший. Рот от гнева перекошен, не кричи, я не глухая. Ты хороший, я плохая. Целый день это в голове, простите. Ну и циферки 1, 3672, 3.673";

        WordCounter wordCounter = new WordCounter(inputText);
        wordCounter.Analyze();
        Console.WriteLine("Количество слов: " + wordCounter.WordCount);

        WordReverser wordReverser = new WordReverser(inputText);
        wordReverser.Analyze();
        Console.WriteLine("Задом наперед: " + wordReverser.ReversedText);
    }
}

