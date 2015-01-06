/* Interfaces.cs - High-level C# interfaces to describe various types data and behaviour
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

using System.Drawing;
using System.Collections.Generic;

namespace SigmaDC.Interfaces
{
    public interface IBaseObject
    {
        [System.ComponentModel.Category( "Base properties" )]
        int Id
        {
            get;
        }

        [System.ComponentModel.Category( "Base properties" )]
        string Name
        {
            get;
        }

        [System.ComponentModel.Category( "Base properties" )]
        int Type
        {
            get;
        }
    }

    public interface ICuboid
    {
        [System.ComponentModel.Category( "Placement" )]
        Point3F NearLeft
        {
            get;
        }

        [System.ComponentModel.Category( "Placement" )]
        Point3F FarRight
        {
            get;
        }
    }

    public interface IExtentOwner
    {
        [System.ComponentModel.Category( "Placement" )]
        RectangleF Extents
        {
            get;
        }
    }

    public interface IVisualisable
    {
        void SetDrawOptions( Dictionary<string, object> options );
        void Draw( Graphics g );
    }

    public interface IEvacuationModel
    {
        void SetupParameters( Dictionary<string, object> modelParams );
    }
}
