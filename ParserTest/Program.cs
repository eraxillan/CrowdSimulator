using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserTest
{
    using PeopleMap = Dictionary<int, List<PeopleTypes.TMan>>;
    using ApertureMap = Dictionary<int, List<ApertureTypes.TAperture>>;
    using FurnitureMap = Dictionary<int, List<FurnitureTypes.TFurnitureItem>>;
    using GeometryMap = Dictionary<int, List<GeometryTypes.TGeometryItem>>;

    class Program
    {
        static void Main(string[] args)
        {
            InputDataParser.Parser inputParser = new InputDataParser.Parser();
  //          peopleParser.ValidateXML(@"..\..\KinderGarten\садик17_people.xsd", @"..\..\KinderGarten\садик17_people.xml");
            /*PeopleMap people = inputParser.LoadPeopleXML(@"..\..\KinderGarten\садик17_people.xml");
            List<int> ids = new List<int>();
            foreach (var manPair in people)
            {
                Console.WriteLine("Floor number: " + manPair.Key);
                Console.WriteLine("-------------------------------");
                foreach( var man in manPair.Value )
                {
                    Console.WriteLine("Man Id: " + man.Id);
                    if (ids.Contains(man.Id))
                        Console.WriteLine("!!!!");
                    else
                        ids.Add(man.Id);
                }
            }*/

            /*ApertureMap apertures = inputParser.LoadApertureXML(@"..\..\KinderGarten\садик17_door.xml");
            List<int> ids = new List<int>();
            foreach (var aperturePair in apertures)
            {
                Console.WriteLine("Floor number: " + aperturePair.Key);
                Console.WriteLine("-------------------------------");
                foreach (var aperture in aperturePair.Value)
                {
                    Console.WriteLine("Aperture Id: " + aperture.Id);
                    if (ids.Contains(aperture.Id))
                        Console.WriteLine("!!!!");
                    else
                        ids.Add(aperture.Id);
                }
            }*/

            /*FurnitureMap furniture = inputParser.LoadFurnitureXML(@"..\..\KinderGarten\садик17_furniture.xml");
            foreach (var furniturePair in furniture)
            {
                Console.WriteLine("Floor number: " + furniturePair.Key);
                Console.WriteLine("-------------------------------");
                foreach (var furnitureItem in furniturePair.Value)
                {
                    Console.WriteLine("FurnitureItem Id: " + furnitureItem.Id);
                }
            }*/

            /*GeometryMap geometry = inputParser.LoadGeometryXML(@"..\..\KinderGarten\садик17_geometry.xml");
            foreach (var geometryPair in geometry)
            {
                Console.WriteLine("Floor number: " + geometryPair.Key);
                Console.WriteLine("-------------------------------");
                foreach (var geometryItem in geometryPair.Value)
                {
                    Console.WriteLine("GeometryItem Id: " + geometryItem.Id);
                }
            }*/

            Console.ReadKey(false);
        }
    }
}
