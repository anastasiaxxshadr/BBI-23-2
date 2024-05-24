using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


abstract class Task
{
	private string _text;
	private string Text()
	{
		return _text;
	}

	public Task(string text) { _text = text; }

	public virtual void ParseText(string text) { }
}

class Task1 : Task
{

	public Task1(string text) : base(text) { ParseText(text); }

	[JsonConstructor]
	public Task1() : base("") { }
	[JsonInclude]
	public string removed; 

	public override void ParseText(string text)
	{
		string pattern = @"[.,!?;:-]";
		removed = Regex.Replace(text, pattern, "");
	}

	public override string ToString()
	{
		return removed;
	}
}
class Task2 : Task
{

	public Task2(string text) : base(text) { ParseText(text); }

	[JsonConstructor]
	public Task2() : base("") { }
	[JsonInclude]
	public string result; 

	public override void ParseText(string text)
	{
		int[] letters = new int[33]; 

		int max = 0;
		char popular = ' ';

		foreach (char c in text.ToLower())
		{
			if (char.IsLetter(c))
			{
				int index = c - 'а';
				if (index >= 0 && index < 33) 
				{
					letters[index]++;

					if (letters[index] > max)
					{
						max = letters[index];
						popular = c;
					}
				}
			}
		}

		string[] words = text.Split(new[] { ' ', ',', '.', '!', '?' });


		foreach (string word in words)
		{
			bool isPopular = false;
			foreach (char c in word.ToLower())
			{
				if (c == popular || char.ToLower(c) == popular)
				{
					isPopular = true;
					break;
				}
			}

			if (!isPopular)
			{
				result += word + " ";
			}
		}
	}

	public override string ToString()
	{
		return result;
	}
}
class Json
{
	public static void Write<T>(T text, string file) 
	{
		using (FileStream filestream = new FileStream(file, FileMode.OpenOrCreate))
		{
			JsonSerializer.Serialize(filestream, text);
		}
	}
	public static T Read<T>(string file) 
	{
		using (FileStream filestream = new FileStream(file, FileMode.OpenOrCreate))
		{
			return JsonSerializer.Deserialize<T>(filestream);
		}
		return default(T);
	}
}

class Program
{
	static void Main()
	{
		string txt = " Самая лучшая песня у царя всех певцов – соловья! Соловей прилетает к нам тогда, когда по  лесу уже давно распевают на все лады зяблики, дрозды и другие певчие птички. Выберут себе соловушка и его подружка тихое, укромное, тенистое местечко где-нибудь поближе к воде. Совьют себе гнёздышко под развесистым кустом, под которым скопилось много прошлогодней листвы. Они вьют его очень низко над землёй, а иногда и прямо на земле. Густая чаща кустов защищает его так хорошо, что редко-редко когда и удастся увидать соловьиное гнёздышко. В опавшей листве соловьи будут копаться, разыскивая себе корм – разных червей и личинок. В эту пору, по вечерам, когда смолкнут голоса других птиц, соловей взлетает на дерево, неподалёку от гнезда, на котором сидит его подружка, и начинает петь. И долго далеко-далеко несётся по лесу и над рекой его сильная, звонкая песня.";
		Task1 task1 = new Task1(txt);
		Task2 task2 = new Task2(txt);


		Task[] task =
		{
			task1,
			task2
		};
        Console.WriteLine("Текст без знаков препинания");
        Console.WriteLine(task[0].ToString());
		Console.WriteLine();
		Console.WriteLine("Текст без слов с самой часто встречающейся буквой");
		Console.WriteLine(task[1].ToString());
		Console.WriteLine("JSON:");


		
		string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		string folder = "Control work";

		path = Path.Combine(path, folder);
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}

		string File1 = "task_1.json";
		string File2 = "task_2.json";

		File1 = Path.Combine(path, File1);
		File2 = Path.Combine(path, File2);

		if (!File.Exists(File1)) 
        {
            var f = File.Create(File1);
            f.Close();
        }

        if (!File.Exists(File2))
        {
            var f = File.Create(File2);
            f.Close();
        }

		if (!File.Exists(File1))
		{
			Json.Write<Task1>((Task1)task[0], File1);
		}
		else
		{
			var read = Json.Read<Task1>(File1);
			Console.WriteLine(read);
		}

		if (!File.Exists(File2))
		{
			Json.Write<Task2>((Task2)task[1], File2);
		}
		else
		{
			var read = Json.Read<Task2>(File2);
			Console.WriteLine(read);
		}
	}
}
