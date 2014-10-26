/* WrapperTypes.cs - Private wrapper definitions for XSD-generated data types;
 * they are needed to increase abstraction level, i.e. have ability to regenerate XSD code
 * 
 * Copyright (C) 2014 Alexander Kamyshnikov
 *
 * This file is part of CrowdSimulator.
 *
 * CrowdSimulator is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as
 * published by the Free Software Foundation; either version 2.1 of
 * the License, or (at your option) any later version.
 *
 * CrowdSimulator is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this program; if not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace SigmaDC.Types
{
    [System.ComponentModel.DefaultProperty( "Id" )]
    public abstract class CashableExtentOwner : IExtentOwner
    {
        private RectangleF m_extent;

        protected abstract RectangleF GetExtents();

        [System.ComponentModel.Category( "Placement" ),
        System.ComponentModel.Description( "The 2D extent of the geometry object" )]
        public RectangleF Extents
        {
            get
            {
                if ( !m_extent.IsEmpty )
                    return m_extent;

                m_extent = GetExtents();
                return m_extent;
            }
        }
    }

    [System.ComponentModel.DefaultProperty( "Id" )]
    public abstract class CashableCuboid : CashableExtentOwner
    {
        private Point3F m_nearLeft;
        private Point3F m_farRight;

        [System.ComponentModel.Category( "Placement" )]
        public Point3F NearLeft
        {
            get { return m_nearLeft; }
            set { m_nearLeft = value; }
        }

        [System.ComponentModel.Category( "Placement" )]
        public Point3F FarRight
        {
            get { return m_farRight; }
            set { m_farRight = value; }
        }
    }

    class ApertureWrapper : CashableCuboid, IBaseObject, IVisualisable
    {
        GeometryTypes.TAperture m_aperture = null;

        public ApertureWrapper( GeometryTypes.TAperture aperture )
        {
            m_aperture = aperture;

            NearLeft = new Point3F( aperture.X1, aperture.Y1, aperture.Z1 );
            FarRight = new Point3F( aperture.X2, aperture.Y2, aperture.Z2 );
        }

        // FIXME: Implement loading from the custom Aperture file
        void UpdateWith( ApertureTypes.TAperture other )
        {
            throw new NotImplementedException();
        }

        public void Draw( Graphics g )
        {
            // FIXME: aperture have only one valid size: width or height; another is null
            var extent = Extents;
            switch ( m_aperture.Type )
            {
                case 0: // Inner door
                {
                    using ( var whitePen = new Pen( Color.Gray, 3.0f / g.DpiX ) )
                    {
                        //whitePen.DashStyle = DashStyle.Dash;
                        whitePen.StartCap = LineCap.SquareAnchor;
                        whitePen.EndCap = LineCap.SquareAnchor;
                        g.DrawLine( whitePen, extent.Left, extent.Top, extent.Right, extent.Bottom );
                    }
                    break;
                }
                case 1: // Window
                {
                    using ( var aquaPen = new Pen( Color.Aqua, 1.0f / g.DpiX ) )
                    {
                        g.DrawLine( aquaPen, extent.Left, extent.Top, extent.Right, extent.Bottom );
                    }
                    break;
                }
                case 2: // Fake ("clue")
                {
                    if ( m_aperture.BoxId1 == m_aperture.BoxId2 )
                    {
                        using ( var whitePen = new Pen( Color.White, 1.0f / g.DpiX ) )
                        {
                            g.DrawLine( whitePen, extent.Left, extent.Top, extent.Right, extent.Bottom );
                        }
                    }
                    else
                    {
                        using ( var brownPen = new Pen( Color.Brown, 1.0f / g.DpiX ) )
                        {
                            g.DrawLine( brownPen, extent.Left, extent.Top, extent.Right, extent.Bottom );
                        }
                    }
                    break;
                }
                case 3: // Exit from building door
                {
                    using ( var pen = new Pen( Color.Black, 3.0f / g.DpiX ) )
                    {
                        pen.StartCap = LineCap.SquareAnchor;
                        pen.EndCap = LineCap.SquareAnchor;
                        g.DrawLine( pen, extent.Left, extent.Top, extent.Right, extent.Bottom );
                    }
                    break;
                }
                default:
                    System.Diagnostics.Debug.Assert( false, "Invalid TBox type" );
                    break;
            }
        }

        protected override RectangleF GetExtents()
        {
            float x1 = m_aperture.X1;
            float y1 = m_aperture.Y1;
            float x2 = m_aperture.X2;
            float y2 = m_aperture.Y2;

            float w = x2 - x1;
            float h = y2 - y1;
            if ( w < 0 ) w *= -1.0f;
            if ( h < 0 ) h *= -1.0f;

            return new RectangleF( x1, y1, w, h );
        }

        public int Id
        {
            get { return m_aperture.Id; }
        }

        public string Name
        {
            get { return m_aperture.Name; }
        }

        public int Type
        {
            get { return m_aperture.Type; }
        }
    }

    class BoxWrapper : CashableCuboid, IBaseObject, IVisualisable
    {
        GeometryTypes.TBox m_box = null;
        Font m_textFont = null;
        RectangleF m_textRect;

        public BoxWrapper( GeometryTypes.TBox box )
        {
            m_box = box;

            NearLeft = new Point3F( m_box.X1, m_box.Y1, m_box.Z1 );
            FarRight = new Point3F( m_box.X2, m_box.Y2, m_box.Z2 );
        }

        [System.ComponentModel.Category( "Base properties" )]
        public int Id
        {
            get { return m_box.Id; }
        }

        [System.ComponentModel.Category( "Base properties" )]
        public string Name
        {
            get { return m_box.Name; }
        }

        [System.ComponentModel.Category( "Base properties" )]
        public int Type
        {
            get { return m_box.Type; }
        }

        protected override RectangleF GetExtents()
        {
            float x1 = m_box.X1;
            float y1 = m_box.Y1;
            float x2 = m_box.X2;
            float y2 = m_box.Y2;

            float w = x2 - x1;
            float h = y2 - y1;

            if ( w <= 0 || h <= 0 ) System.Diagnostics.Debugger.Break();
            System.Diagnostics.Debug.Assert( w > 0, "Box extent have negative width" );
            System.Diagnostics.Debug.Assert( h > 0, "Box extent have negative height" );

            return new RectangleF( x1, y1, w, h );
        }

        public void Draw( Graphics g )
        {
            switch ( m_box.Type )
            {
                case 0:
                {
                    // Draw box rectangle
                    var extent = Extents;
                    using ( var brownPen = new Pen( Color.Brown, 1.0f / g.DpiX ) )
                    {
                        g.DrawRectangle( brownPen, extent.X, extent.Y, extent.Width, extent.Height );
                    }

                    // Draw box properties as text on it
                    var extentCenter = new PointF( ( extent.Left + extent.Right ) / 2, ( extent.Bottom + extent.Top ) / 2 );
                    DrawText( g, "ID: " + m_box.Id + "\n" + "Height: " + ( m_box.Z2 - m_box.Z1 ), extentCenter, extent );
                    break;
                }
                default:
                    System.Diagnostics.Debug.Assert( false, "Invalid TBox type" );
                    break;
            }
        }

        private void DrawText( Graphics g, string text, PointF ptStart, RectangleF extent )
        {
            var gs = g.Save();
            // Inverse Y axis again - now it grow down;
            // if we don't do this, text will be drawn inverted
            g.ScaleTransform( 1.0f, -1.0f, MatrixOrder.Prepend );

            if ( m_textFont == null )
            {
                // Find the maximum appropriate text size to fix the extent
                float fontSize = 100.0f;
                Font fnt;
                SizeF textSize;
                do
                {
                    fnt = new Font( "Arial", fontSize / g.DpiX, FontStyle.Bold, GraphicsUnit.Pixel );
                    textSize = g.MeasureString( text, fnt );
                    m_textRect = new RectangleF( new PointF( ptStart.X - textSize.Width / 2.0f, -ptStart.Y - textSize.Height / 2.0f ), textSize );

                    var textRectInv = new RectangleF( m_textRect.X, -m_textRect.Y, m_textRect.Width, m_textRect.Height );
                    if ( extent.Contains( textRectInv ) )
                        break;

                    fontSize -= 1.0f;
                    if ( fontSize <= 0 )
                    {
                        fontSize = 1.0f;
                        break;
                    }
                } while ( true );

                m_textFont = fnt;
            }

            // Create a StringFormat object with the each line of text, and the block of text centered on the page
            var stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            g.DrawString( text, m_textFont, Brushes.Black, m_textRect, stringFormat );
            stringFormat.Dispose();

            g.Restore( gs );
        }
    }

    class RoomWrapper : CashableExtentOwner, IVisualisable
    {
        GeometryTypes.TRoom m_room = null;
        BoxWrapper[] m_boxes = null;

        public RoomWrapper( GeometryTypes.TRoom room )
        {
            m_room = room;

            m_boxes = new BoxWrapper[ room.Geometry.Count() ];
            for ( int i = 0; i < room.Geometry.Count(); ++i )
            {
                m_boxes[ i ] = new BoxWrapper( room.Geometry[ i ] );
            }
        }

        public BoxWrapper[] Boxes
        {
            get { return m_boxes; }
        }

        public void Draw( Graphics g )
        {
            switch ( m_room.Type )
            {
                case 0: // Room itself
                {
                    foreach ( var box in m_boxes )
                    {
                        box.Draw( g );
                    }
                    break;
                }
                case 1: // Corridor
                {
                    using ( var grayBrush = new HatchBrush( HatchStyle.DiagonalCross, Color.LightGray, Color.White ) )
                    {
                        g.FillRectangle( grayBrush,Extents.X, Extents.Y, Extents.Width, Extents.Height );
                    }
                    break;
                }
                default:
                System.Diagnostics.Debug.Assert( false, "Invalid TRoom type" );
                break;
            }
        }

        protected override RectangleF GetExtents()
        {
            RectangleF extent = new RectangleF();
            if ( m_boxes.Count() >= 1 )
            {
                extent = m_boxes[ 0 ].Extents;
            }

            for ( int i = 1; i < m_boxes.Count(); ++i )
            {
                extent = RectangleF.Union( extent, m_boxes[ i ].Extents );
            }

            return extent;
        }
    }


    class FloorWrapper : CashableExtentOwner, IVisualisable
    {
        GeometryTypes.TFloor m_floor = null;
        RoomWrapper[] m_rooms = null;
        ApertureWrapper[] m_apertures = null;
        // FIXME: implement
        //FurnitureWrapper[] m_furniture = null;
        //StairwayWrapper[] m_stairways = null;

        //PeopleWrapper[] m_people = null;

        public FloorWrapper( GeometryTypes.TFloor floor )
        {
            m_floor = floor;

            m_rooms = new RoomWrapper[ floor.RoomList.Count() ];
            for ( int i = 0; i < floor.RoomList.Count(); ++i )
            {
                m_rooms[ i ] = new RoomWrapper( floor.RoomList[ i ] );
            }

            m_apertures = new ApertureWrapper[ floor.ApertureList.Count() ];
            for ( int i = 0; i < floor.ApertureList.Count(); ++i )
            {
                m_apertures[ i ] = new ApertureWrapper( floor.ApertureList[ i ] );
            }
        }

        public string Name
        {
            get { return m_floor.Name; }
        }

        public int Number
        {
            get { return m_floor.Number; }
        }

        public ApertureWrapper[] Apertures
        {
            get { return m_apertures; }
            set { m_apertures = value; }
        }

        public ApertureWrapper FindAperture( int id )
        {
            foreach ( var aperture in Apertures )
            {
                if ( aperture.Id == id )
                    return aperture;
            }

            return null;
        }

        public Dictionary<RectangleF, BoxWrapper> GetBoxMap()
        {
            Dictionary<RectangleF, BoxWrapper> boxes = new Dictionary<RectangleF, BoxWrapper>();
            foreach ( var room in m_rooms )
            {
                foreach ( var box in room.Boxes )
                {
                    boxes.Add( box.Extents, box );
                }
            }
            return boxes;
        }

        public void Draw( Graphics g )
        {
            switch ( m_floor.Type )
            {
                case 0:
                {
                    // FIXME: draw other stuff
                    foreach ( var room in m_rooms ) room.Draw( g );
                    foreach ( var aperture in m_apertures ) aperture.Draw( g );
                    break;
                }
                default:
                System.Diagnostics.Debug.Assert( false, "Invalid TFloor type" );
                break;
            }
        }

        protected override RectangleF GetExtents()
        {
            RectangleF extent = new RectangleF();
            if ( m_rooms.Count() >= 1 )
            {
                extent = m_rooms[ 0 ].Extents;
            }

            for ( int i = 1; i < m_rooms.Count(); ++i )
            {
                extent = RectangleF.Union( extent, m_rooms[ i ].Extents );
            }

            return extent;
        }
    }

    class BuildingWrapper : CashableExtentOwner, IVisualisable
    {
        GeometryTypes.TBuilding m_building = null;
        int m_floorNumber = -1;
        FloorWrapper[] m_floors = null;

        static readonly string GEOMETRY_FILE_NAME = @"geometry.xml";
        static readonly string APERTURES_FILE_NAME = @"apertures.xml";
        static readonly string FURNITURE_FILE_NAME = @"furniture.xml";
        static readonly string PEOPLE_FILE_NAME = @"people.xml";

        string m_lastError;

        #region Floor sorter class (by numbers of floor)
        // FIXME: inherit all floors from one class to remove switch(type) ugly construction
        class FloorComparer : IComparer
        {
            public int Compare( object x, object y )
            {
                if ( x == null || y == null )
                    return 1;
                if ( x.GetType() != y.GetType() )
                    throw new ArgumentException( "Floor types are different" );

                if ( x is GeometryTypes.TFloor )
                {
                    var xx = x as GeometryTypes.TFloor;
                    var yy = y as GeometryTypes.TFloor;
                    if ( xx == null || yy == null )
                        throw new ArgumentException( "Object is not a Geometry TFloor" );

                    if ( xx.Number == yy.Number )
                        return 0;
                    if ( xx.Number > yy.Number )
                        return 1;
                    return -1;
                }
                else if ( x is ApertureTypes.TFloor )
                {
                    var xx = x as ApertureTypes.TFloor;
                    var yy = y as ApertureTypes.TFloor;
                    if ( xx == null || yy == null )
                        throw new ArgumentException( "Object is not a Apertures TFloor" );

                    if ( xx.Number == yy.Number )
                        return 0;
                    if ( xx.Number > yy.Number )
                        return 1;
                    return -1;
                }
                else if ( x is FurnitureTypes.TFloor )
                {
                    var xx = x as FurnitureTypes.TFloor;
                    var yy = y as FurnitureTypes.TFloor;
                    if ( xx == null || yy == null )
                        throw new ArgumentException( "Object is not a Furniture TFloor" );

                    if ( xx.Number == yy.Number )
                        return 0;
                    if ( xx.Number > yy.Number )
                        return 1;
                    return -1;
                }
                else if ( x is PeopleTypes.TFloor )
                {
                    var xx = x as PeopleTypes.TFloor;
                    var yy = y as PeopleTypes.TFloor;
                    if ( xx == null || yy == null )
                        throw new ArgumentException( "Object is not a People TFloor" );

                    if ( xx.Number == yy.Number )
                        return 0;
                    if ( xx.Number > yy.Number )
                        return 1;
                    return -1;
                }
                else
                {
                    System.Diagnostics.Debug.Assert( false, "Invalid Floor type" );
                }

                return 1;
            }
        }

        void SortFloors( Array arr )
        {
            Array.Sort( arr, new FloorComparer() );
        }
        #endregion

        /*public BuildingWrapper( GeometryTypes.TBuilding building )
        {
            m_building = building;
            SortFloors( building );

            m_floorNumber = building.FloorList.Count() > 0 ? building.FloorList[ 0 ].Number : -1;
            m_floors = new FloorWrapper[ FloorCount ];
            for( int i = 0; i < building.FloorList.Count(); ++i )
            {
                m_floors[ i ] = new FloorWrapper( building.FloorList[ i ] );
            }
        }*/

        public BuildingWrapper( string dataDir )
        {
            System.Diagnostics.Debug.Assert( Directory.Exists( dataDir ) );

            var geometryFileNameAbs = dataDir + Path.DirectorySeparatorChar + GEOMETRY_FILE_NAME;
            var aperturesFileNameAbs = dataDir + Path.DirectorySeparatorChar + APERTURES_FILE_NAME;
            var furnitureFileNameAbs = dataDir + Path.DirectorySeparatorChar + FURNITURE_FILE_NAME;
            var peopleFileNameAbs = dataDir + Path.DirectorySeparatorChar + PEOPLE_FILE_NAME;

            var parser = new InputDataParser.Parser();
            LoadGeometry( parser, geometryFileNameAbs );
            LoadApertures( parser, aperturesFileNameAbs );
            LoadFurniture( parser, furnitureFileNameAbs );
            LoadPeople( parser, peopleFileNameAbs );

        }

        public bool LoadGeometry( InputDataParser.Parser parser, string fileName )
        {
            m_building = parser.LoadGeometryXMLRoot( fileName );
            if ( m_building.FloorList.Count() == 0 )
            {
                m_lastError = "Building has no floors";
                return false;
            }

            SortFloors( m_building.FloorList );

            m_floorNumber = m_building.FloorList.Count() > 0 ? m_building.FloorList[ 0 ].Number : -1;
            m_floors = new FloorWrapper[ FloorCount ];
            for ( int i = 0; i < m_building.FloorList.Count(); ++i )
            {
                m_floors[ i ] = new FloorWrapper( m_building.FloorList[ i ] );
            }

            return true;
        }

        public bool LoadApertures( InputDataParser.Parser parser, string fileName )
        {
            var building = parser.LoadAperturesXMLRoot( fileName );
            if ( building.FloorList.Count() == 0 )
            {
                m_lastError = "Building has no floors";
                return false;
            }

            System.Diagnostics.Debug.Assert( building.FloorList.Count() == m_floors.Count() );

            SortFloors( building.FloorList );

            foreach(var floor in building.FloorList)
            {
                FloorWrapper fw = GetSpecifiedFloor( floor.Number );
                System.Diagnostics.Debug.Assert( fw.Number == floor.Number );

                
                for ( int i = 0; i < floor.ApertureList.Count(); ++i )
                {
                    //fw.Apertures[ i ] = new ApertureWrapper( floor.ApertureList[ i ] );
                    
                    // Search for specified aperuture in the geometry file ones
                    //for (int )
                }
            }

            return true;
        }

        public bool LoadFurniture( InputDataParser.Parser parser, string fileName )
        {
            //            throw new NotImplementedException();
            return true;
        }

        public bool LoadPeople( InputDataParser.Parser parser, string fileName )
        {
            //            throw new NotImplementedException();
            return true;
        }

        public string GetGeometryFileName()
        {
            return GEOMETRY_FILE_NAME;
        }

        public string GetAperturesFileName()
        {
            return APERTURES_FILE_NAME;
        }

        public string GetFurnitureFileName()
        {
            return FURNITURE_FILE_NAME;
        }

        public string GetPeopleFileName()
        {
            return PEOPLE_FILE_NAME;
        }

        public FloorWrapper[] Floors
        {
            get { return m_floors; }
        }

        public int CurrentFloorNumber
        {
            get { return m_floorNumber; }
            set
            {
                if ( value >= FirstFloorNumber && value <= FloorCount )
                    m_floorNumber = value;
                else
                    throw new ArgumentException( "Floor number must be in range " + FirstFloorNumber + " - " + FloorCount );
            }
        }

        public int FirstFloorNumber
        {
            get
            {
                return m_building.FloorList[ 0 ].Number;
            }
        }

        public int FloorCount
        {
            get
            {
                // FIXME: think about negative floor numbers
                return m_building.FloorList.Count();
            }
        }

        FloorWrapper GetSpecifiedFloor( int number )
        {
            foreach ( var floor in m_floors )
            {
                if ( floor.Number == number )
                    return floor;
            }

            return null;
        }

        public FloorWrapper CurrentFloor
        {
            get { return GetSpecifiedFloor( m_floorNumber ); }
        }

        public void Draw( Graphics g )
        {
            CurrentFloor.Draw( g );
        }

        protected override RectangleF GetExtents()
        {
            return CurrentFloor.Extents;
        }
    }
}
