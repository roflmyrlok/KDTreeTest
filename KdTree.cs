using System;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp2;

public class KdTree
{
	public Point kdTreeBuilder(Point[] valueList,int depth)
	{
		if (valueList.Length == 1)
		{
			valueList[0].left = null;
			valueList[0].right = null;
			valueList[0].depthOfTree = depth++;
			return valueList[0];
		}

		if (valueList.Length == 0)
		{
			return null;
		}

		var depthOfTree = depth + 1;
		var w8 = valueList;
		if (depthOfTree % 2 == 0)
		{
			Array.Sort(w8, new LatComparer());
		}
		else
		{
			Array.Sort(w8, new LonComparer());
		}
        
		var middle = w8.Length / 2;
		if (middle == 0)
		{
			return w8[0];
		}

       
		var left = w8[..(middle)];
		var right = w8[(middle + 1)..];
		var curr = w8[middle];
		if (depthOfTree % 2 == 0)
		{
			curr.leftBorder = w8[0].lat;
			curr.rightBorder = w8[^1].lat;
		}
		else
		{
			curr.leftBorder = w8[0].lon;
			curr.rightBorder = w8[^1].lon;
		}
		curr.depthOfTree = depthOfTree;
		curr.right = kdTreeBuilder(right, depthOfTree);
		curr.left = kdTreeBuilder(left, depthOfTree);
		return curr;
	}
	
	public List<Point> kdTreeSearch(Point centre, double range, Point kdTree)
	{
		var current = kdTree;
		var previous = new Point();
		var treeStack = new Stack<Point>();
		while (true)
		{
			if (current.left == null && current.right == null)
			{
				current = treeStack.Pop();
				continue;
			}
			if (current.left == null)
			{
				current = current.right;
				continue;
			}
    
			if (current.right == null)
			{
				current = current.left;
				continue;
			}
			if (distHaversine(centre,current) <= range)
			{
				break;
			}
			previous = current;
			var distToLeft = distHaversine(centre, current.left);
			var distToRight = distHaversine(centre, current.right);
			treeStack.Push(distToLeft > distToRight ? current.left : current.right);
			current = distToLeft < distToRight ? current.left : current.right;
            
		}
    
		var stack = new Stack<Point>();
		var ListToSearch = new List<Point>();
		stack.Push(previous);
		Console.WriteLine(current.lat + " " + current.lon);
		Console.WriteLine(previous.lat + " " + previous.lon);
		while (stack.Count > 0)
		{
			var curr = stack.Pop();
			if (curr.left != null)
			{
				stack.Push(curr.left);
			}
            
			if (curr.right != null)
			{
				stack.Push(curr.right);
			}
            
			ListToSearch.Add(curr);
		}
    
		return ListToSearch;
	}
	
	bool Haversine(Point current, Point centre, double range)
	{
		return distHaversine(current, centre) <= range; 
	}
	double distHaversine(Point current, Point centre)
	{
		double radius = 6371000;
		double convertor = Math.PI / 180;
		double latCentre = centre.lat * convertor;
		double latCurr = current.lat * convertor;
		double latCurrSubLatCentre = (current.lat - centre.lat) * convertor;
		double lonCurrSubLonCentre = (current.lon - centre.lon) * convertor;
		double a = Math.Sin(latCurrSubLatCentre / 2) * Math.Sin(latCurrSubLatCentre / 2)
		           + Math.Cos(latCentre) * Math.Cos(latCurr) * Math.Sin(lonCurrSubLonCentre / 2)
		           * Math.Sin(lonCurrSubLonCentre / 2);
		double d = radius * 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
		return d;
	}
	
	class LatComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return(new CaseInsensitiveComparer()).Compare(((Point)x).lat,
				((Point)y).lat);
		}
	}

	class LonComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return(new CaseInsensitiveComparer()).Compare(((Point)x).lon,
				((Point)y).lon);
		}
	}
}