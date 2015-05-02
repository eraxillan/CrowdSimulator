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
using SigmaDC.Common.MathEx;

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

    public interface IDistanceField
    {
        float Get( float x, float y );
    }

    public enum HunanMobilityGroup { First = 1, Second = 2, Third = 3, Fourth = 4 };
    public enum HumanAgeGroup { First = 1, Second = 2, Third = 3, Fourth = 4, Fifth = 5 };
    public enum HumanEmotionState { Custom = 0, Comfort = 1, Calm = 2, Active = 3, VeryActive = 4 };

    public interface IHuman
    {
        int Id
        {
            get;
        }

        Vector2 ProjectionCenter
        {
            get;
        }

        float ProjectionDiameter
        {
            get;
        }

        RectangleF ProjectionExtent
        {
            get;
        }

        int ExitId
        {
            get;
        }

        HunanMobilityGroup MobilityGroup
        {
            get;
        }

        HumanAgeGroup AgeGroup
        {
            get;
        }

        HumanEmotionState EmotionState
        {
            get;
        }
    }

    public class HumanRuntimeInfo : IHuman
    {
        IHuman m_human;

        public List<float> RotateAngles { get; set; }
        public List<SdcRectangle> VisibilityAreas { get; set; }
        public List<Vector2> MoveDirections { get; set; }

        public List<float> MinDistToObstacle { get; set; }

        //public List<float> MoveProbabilites{get;set;}

        public HumanRuntimeInfo(IHuman h)
        {
            m_human = h;

            RotateAngles = new List<float>();
            VisibilityAreas = new List<SdcRectangle>();
            MoveDirections = new List<Vector2>();
            MinDistToObstacle = new List<float>();
        }

        #region IHuman interface implementation

        public int Id
        {
            get { return m_human.Id; }
        }

        public Vector2 ProjectionCenter
        {
            get { return m_human.ProjectionCenter; }
        }

        public float ProjectionDiameter
        {
            get { return m_human.ProjectionDiameter; }
        }

        public RectangleF ProjectionExtent
        {
            get { return m_human.ProjectionExtent; }
        }

        public int ExitId
        {
            get { return m_human.ExitId; }
        }

        public HunanMobilityGroup MobilityGroup
        {
            get { return m_human.MobilityGroup; }
        }

        public HumanAgeGroup AgeGroup
        {
            get { return m_human.AgeGroup; }
        }

        public HumanEmotionState EmotionState
        {
            get { return m_human.EmotionState; }
        }
        #endregion
    }

    public interface IEvacuationModel
    {
        void SetupParameters( Dictionary<string, object> modelParams );

        void NextStepAll( IDistanceField S, ref List<HumanRuntimeInfo> hi );

        Vector2 NextStep( IHuman human, IDistanceField S, ref HumanRuntimeInfo humanInfo );
    }
}
