using System.Collections;
using System.Diagnostics;
using ConsoleApp2;

Main(args);

static void Main(string[] args)
{
    var db = args[1].Substring(args[1].LastIndexOf('=') + 1);
    var latitude = Convert.ToDouble(args[2].Substring(args[2].LastIndexOf('=') + 1));
    var longtitude = Convert.ToDouble(args[3].Substring(args[3].LastIndexOf('=') + 1));
    var size = Convert.ToDouble(args[4].Substring(args[4].LastIndexOf('=') + 1));
    var roadmap = new csvreader(db);
    var points = roadmap.listA;
    var currentKdTree = kdTree(points.ToArray(),0);
    // findPointsInRange(latitude, longtitude, size, currentKdTree);
    
    Point kdTree(Point[] valueList,int depth)
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
        curr.depthOfTree = depthOfTree;
        curr.right = kdTree(right, depthOfTree);
        curr.left = kdTree(left, depthOfTree);
        return curr;
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

    void readFromPointsList(List<Point> newList)
    {
        var i = 1;
        Console.WriteLine("Next points were found in given area:");
        foreach (var stop in newList)
        {
            if (stop.name != "")
            {
                Console.WriteLine(i.ToString() + ". " + stop.name + " " + stop.lat.ToString() + " " +
                                  stop.lon.ToString());
            }
            else
            {
                Console.WriteLine(i.ToString() + ". " + stop.type[0] + " " + stop.lat.ToString() + " " +
                                  stop.lon.ToString());
            }

            i++;
        }
    }
    
}

public class LatComparer : IComparer
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