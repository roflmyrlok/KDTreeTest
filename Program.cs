using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

    var newCentre = new Point {lat = latitude, lon = longtitude};
    var currentKdTree = new KdTree();
    var kdTreeRoot = currentKdTree.kdTreeBuilder(points.ToArray(), 0);
    var listFromKdtree = currentKdTree.kdTreeSearch(newCentre, size, kdTreeRoot);


    readFromPointsList(allInRange(newCentre, size, points));
    readFromPointsList(allInRange(newCentre, size, listFromKdtree));


    List<Point> allInRange(Point centre, double range, List<Point> points)
    {
        var k = 1;
        var sw = new Stopwatch();
        sw.Start();
        var localPointsList = points.Where(curr => Haversine(curr, centre, range)).ToList();
        sw.Stop();
        return localPointsList;
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
}