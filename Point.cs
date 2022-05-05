using System.Collections.Generic;

namespace ConsoleApp2;

public class Point
{
	public double lon { get; set; }
	public double lat { get; set; }
	public double leftBorder { get; set; }
	public double rightBorder { get; set; }
	
	public Point left { get; set; }
	

	public int depthOfTree { get; set; } = 0;
	public Point right { get; set; }
	
	public List<string> type { get; set; }
	public string name { get; set; }
	public string adress { get; set; }

	public double getValue(int depth)
	{
		return depth % 2 == 0 ? lon : lat;
	}
}