using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace InputDataParser
{
    using PeopleMap = Dictionary<int, List<PeopleTypes.TMan>>;
    using ApertureMap = Dictionary<int, List<ApertureTypes.TAperture>>;
    using FurnitureMap = Dictionary<int, List<FurnitureTypes.TFurnitureItem>>;
    using GeometryMap = Dictionary<int, List<GeometryTypes.TGeometryItem>>;

    public class Parser
    {
        /*public void ValidateXML(string schemaFileName, string xmlFileName)
        {
            var schemas = new XmlSchemaSet();
            schemas.Add("", schemaFileName);

            Console.WriteLine("Attempting to validate {0}", xmlFileName);
            var xmlDoc = XDocument.Load(xmlFileName);
            bool errors = false;
            xmlDoc.Validate(schemas, (o, e) =>
            {
                Console.WriteLine("{0}", e.Message);
                errors = true;
            });
            Console.WriteLine("{0}: {1}", xmlFileName, errors ? "did not validate" : "validated");
            Console.WriteLine();

            IEnumerable<XElement> people =
                from seg in xmlDoc.Descendants()
                where (string)seg.Element("ClassName") == "TMan"
                select seg;

            int c = people.Count(); // 46 + 92 = 138
            foreach( var el in people)
            {
                Console.WriteLine( el.Element("Color").Value );
            }

            Console.WriteLine();
        }*/

        public PeopleMap LoadPeopleXML( string fileName )
        {
            var result = new PeopleMap();
            var serializer = new XmlSerializer( typeof( PeopleTypes.TBuilding ) );
            var reader = new FileStream( fileName, FileMode.Open );
            var building = serializer.Deserialize( reader ) as PeopleTypes.TBuilding;
            foreach ( var floor in building.FloorList )
            {
                var peopleList = new List<PeopleTypes.TMan>();
                foreach ( var man in floor.People )
                {
                    peopleList.Add( man );
                }

                result.Add( floor.Number, peopleList );
            }
            return result;
        }

        public GeometryTypes.TBuilding LoadGeometryXMLRoot( string fileName )
        {
            if ( !File.Exists( fileName ) ) throw new InvalidOperationException( "Geometry file was not found" );

            var serializer = new XmlSerializer( typeof( GeometryTypes.TBuilding ) );
            var reader = new FileStream( fileName, FileMode.Open );
            var building = serializer.Deserialize( reader ) as GeometryTypes.TBuilding;
            return building;
        }

        public ApertureTypes.TBuilding LoadAperturesXMLRoot( string fileName )
        {
            if ( !File.Exists( fileName ) ) throw new InvalidOperationException( "Apertures file was not found" );

            var serializer = new XmlSerializer( typeof( ApertureTypes.TBuilding ) );
            var reader = new FileStream( fileName, FileMode.Open );
            var building = serializer.Deserialize( reader ) as ApertureTypes.TBuilding;
            return building;
        }

        public FurnitureTypes.TBuilding LoadFurnitureXMLRoot( string fileName )
        {
            if ( !File.Exists( fileName ) ) throw new InvalidOperationException( "Furniture file was not found" );

            var serializer = new XmlSerializer( typeof( FurnitureTypes.TBuilding ) );
            var reader = new FileStream( fileName, FileMode.Open );
            var building = serializer.Deserialize( reader ) as FurnitureTypes.TBuilding;
            return building;
        }

        public PeopleTypes.TBuilding LoadPeopleXMLRoot( string fileName )
        {
            if ( !File.Exists( fileName ) ) throw new InvalidOperationException( "People file was not found" );

            var serializer = new XmlSerializer( typeof( PeopleTypes.TBuilding ) );
            var reader = new FileStream( fileName, FileMode.Open );
            var building = serializer.Deserialize( reader ) as PeopleTypes.TBuilding;
            return building;
        }
    }
}
