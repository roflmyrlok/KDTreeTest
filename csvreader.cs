namespace ConsoleApp2;

public class csvreader
{
	public List<point> listA { get; set; }
	public csvreader()
	{
		listA = new List<point>();
		StreamReader reader = new StreamReader(@"ukraine_poi.csv");
		while (!reader.EndOfStream)
		{
			
			var line = reader.ReadLine();
			var values = line.Split(';');
			
			var latL = values[0];
			latL = latL.Replace(',','.');
			var lonL = values[1];
			lonL = lonL.Replace(',','.');
			var newPoint = new point()
			{
				lat = Convert.ToDouble(latL),
				lon = Convert.ToDouble(lonL),
				adress = values[^2] + values[^1],
				name = values[^3],
				type = new List<string> {values[2],values[3]}
			};
			listA.Add(newPoint);
		}
	}
}