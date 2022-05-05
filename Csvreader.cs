using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleApp2;

public class csvreader
{
	public List<Point> listA { get; set; }
	public csvreader(String db)
	{
		listA = new List<Point>();
		StreamReader reader = new StreamReader(@db);
		while (!reader.EndOfStream)
		{
			
			var line = reader.ReadLine();
			var values = line.Split(';');
			
			var latL = values[0];
			latL = latL.Replace(',','.');
			var lonL = values[1];
			lonL = lonL.Replace(',','.');
			var newPoint = new Point()
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