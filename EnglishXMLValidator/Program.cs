using System.Text;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Place your merged 'english.xml' in the same folder as this programm.");
        Console.WriteLine("Press any key to start the validation.");

        Console.ReadKey();
        Console.WriteLine("");

        if (!File.Exists("english.xml"))
        {
            Console.WriteLine("Unable to access 'english.xml'.");
            Console.ReadKey();
            Environment.Exit(2);
        }

        var uidLines = GetUidAppearances();

        // Output.
        bool duplicatesExist = false;
        foreach (var kv in uidLines) 
        {
            if (kv.Value.Count > 1)
            {
                Console.Write($"The uid {kv.Key} appears multiple times in lines ");
                for (int i = 0; i < kv.Value.Count; i++)
                {
                    if (i < kv.Value.Count - 1)
                    {
                        Console.Write($"{kv.Value[i]}, ");
                    }
                    else
                    {
                        Console.Write($"{kv.Value[i]}.");
                    }
                }
                Console.Write("\n");

                duplicatesExist = true;
            }
        }

        if (!duplicatesExist) 
        {
            Console.WriteLine("No duplicates detected.");
        }

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }

    private static IEnumerable<int> AllIndicesOf(string str, string searchString)
    {
        int minIndex = str.IndexOf(searchString);
        while (minIndex != -1)
        {
            yield return minIndex;
            minIndex = str.IndexOf(searchString, minIndex + searchString.Length);
        }
    }

    private static Dictionary<string, List<int>> GetUidAppearances()
    {
        const string uidDeclaration = "contentuid=\"";
        var lines = File.ReadLines("english.xml");
        // uidAppearances: <uid string, lines where this uid string appears>.
        var uidAppearances = new Dictionary<string, List<int>>();

        foreach (var line in lines.Index())
        {
            var uidDelcarationPositions = AllIndicesOf(line.Item, uidDeclaration);
            if (!uidDelcarationPositions.Any())
            {
                continue;
            }

            // Line numbers are not zero based.
            int lineNumber = line.Index + 1;

            foreach (int declarationPosition in uidDelcarationPositions)
            {
                // Get starting position of the uid string.
                int index = declarationPosition + uidDeclaration.Length;
                char currChar;
                var uid = new StringBuilder();

                while ((currChar = line.Item[index]) != '\"')
                {
                    uid.Append(currChar);
                    index++;
                }

                // Document that the uid appeared in this line.
                string uidString = uid.ToString();
                if (!uidAppearances.TryGetValue(uidString, out List<int>? appearances))
                {
                    List<int> linesList = [lineNumber];
                    uidAppearances.Add(uidString, linesList);
                }
                else
                {
                    appearances.Add(lineNumber);
                }
            }
        }

        return uidAppearances;
    }
}
            