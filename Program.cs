namespace studentdb;

class Program
{
	static readonly string[] names = [
		"Justin Jones", "DeAngelo Robinson", "Martina Basquez", "Alain Rene", "David Goodwin",
		"Joey Molinski", "Wren Grasley", "Brady Hartman", "David Brameijer", "Afseen Salam", "Ethan Thomas"
	];

	static readonly string[] hometowns = [
		"Westerville", "Detroit", "Edinburg", "Homestead", "Jersey City", "Toledo", "Richmond", "Saranac",
		"Grand Rapids", "India", "Bolivar"
	];

	static readonly string[] favoriteFoods = [
		"Baja Blast", "Pizza", "Leftovers", "Chicken Wings", "Sushi", "Dill Pickles", "Pizza", "Chicken Wings",
		"Tacos", "Shawarma", "Grapes"
	];

	static void Main(string[] args)
	{
		Console.WriteLine("Welcome to the student database!");

		while (true)
		{
			Console.WriteLine($"{names.Length} students registered!");
			Console.WriteLine("Enter a student name or number to view their information, or enter 'students' to view all.");

			int id;
			while (!TryGetInput(Console.ReadLine(), out id))
				Console.WriteLine("Sorry, I didn't get that. Could you please try again?");

			if (id < 0)
				ListAllStudents();
			else
				ListStudent(id);

			if (!PromptYesNo(true, "Would you like to view another student?"))
				break;
			Console.WriteLine(" ");
		}

		Console.WriteLine("Goodbye!");
	}

	static void ListAllStudents()
	{
		// automatic column sizing
		int[] columns = [
			names.Append("Name").Max(name => name.Length),
			hometowns.Append("Hometown").Max(town => town.Length),
			favoriteFoods.Append("Favorite Food").Max(food => food.Length)
		];
		string format = $"{{0,-3}}    {{1, -{columns[0]}}}    {{2, -{columns[1]}}}    {{3, -{columns[2]}}}";
		// becomes {0, -3}    {1, -#}    {2, -#}    {3, -#}

		// print header text with divider and spacing
		string header = string.Format(format, "ID#", "Name", "Hometown", "Favorite Food");
		Console.WriteLine();
		Console.WriteLine(header);
		Console.WriteLine(new string('\x2500', header.Length));
		Console.WriteLine();

		// print entries
		for (int i = 0; i < names.Length; i++)
			Console.WriteLine(string.Format(format, i, names[i], hometowns[i], favoriteFoods[i]));

		Console.WriteLine();
	}

	static void ListStudent(int id)
	{
		Console.WriteLine(string.Format("#{0,-3} '{1}'", id, names[id]));
		Console.WriteLine("Enter 'Favorite Food' or 'Hometown' to view information.");

		while (true)
		{
			var line = Console.ReadLine();

			if (line != null)
			{
				line = line.Trim(); // important

				if ("favorite food".Contains(line, StringComparison.OrdinalIgnoreCase))
				{
					Console.WriteLine($"{names[id]}'s favorite food is {favoriteFoods[id]}");
					return;
				}

				if (
					"hometown".Contains(line, StringComparison.OrdinalIgnoreCase) ||
					"home town".Contains(line, StringComparison.OrdinalIgnoreCase)
				)
				{
					Console.WriteLine($"{names[id]}'s home town is {hometowns[id]}");
					return;
				}
			}

			Console.WriteLine("Sorry, I didn't get that. Could you please try again?");
		}
	}

	static bool TryGetInput(string? input, out int studentId)
	{
		studentId = -1;

		// invalid input
		if (input is null)
			return false;

		// show all students
		if (input.Equals("students", StringComparison.OrdinalIgnoreCase))
			return true;

		// check for id #
		if (int.TryParse(input, out studentId))
			return studentId >= 0 && studentId < names.Length;

		// find a partial or complete match by name
		for (studentId = 0; studentId < names.Length; studentId++)
			if (
				names[studentId].Equals(input, StringComparison.OrdinalIgnoreCase) || 
				names[studentId].Split(' ').Contains(input, StringComparer.OrdinalIgnoreCase)
			)
				return true;

		// no results
		return false;
	}

	static bool PromptYesNo(bool allowEscape, string message)
	{
		Console.WriteLine(message + " [Y/N]");

		while (true)
		{
			// get keystroke
			char key = Console.ReadKey().KeyChar;

			// deletes echoed keystroke from output
			Console.Write("\b\\\b");

			// process keystroke
			switch (key)
			{
				// yes
				case 'y':
				case 'Y':
					return true;

				// no
				case 'n':
				case 'N':
					return false;

				// escape key
				case '\x1b':
					if (allowEscape)
						return false;
					break;
			}
		}
	}
}
