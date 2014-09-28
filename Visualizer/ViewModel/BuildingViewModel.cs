using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visualizer.ViewModel
{
    public class BuildingViewModel : INotifyPropertyChanged
    {
        #region Private Variables
        GeometryTypes.TBuilding m_building;
        GeometryTypes.TFloor m_selectedFloor;
        #endregion

        #region Constructor
        public BuildingViewModel()
        {
            InputDataParser.Parser inputParser = new InputDataParser.Parser();
            System.Diagnostics.Debug.Assert(System.IO.File.Exists(@"..\..\..\Data\KinderGarten\садик17_geometry.xml"));
            m_building = inputParser.LoadGeometryXMLRoot(@"..\..\..\Data\KinderGarten\садик17_geometry.xml");
            if (m_building.FloorList.Count() == 0) throw new InvalidOperationException("Building has no floors");
            m_selectedFloor = m_building.FloorList[0];
        }
        #endregion

        public ObservableCollection<GeometryTypes.TFloor> Floors
        {
            get
            {
                return new ObservableCollection<GeometryTypes.TFloor>(m_building.FloorList);
            }
            /*set
            {
                _books = value;
                OnPropertyChanged("Books");
            }*/
        }

        public GeometryTypes.TFloor SelectedFloor
        {
            get
            {
                return m_selectedFloor;
            }
            set
            {
                m_selectedFloor = value;
                OnPropertyChanged("SelectedFloor");
            }
        }

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Command
        #endregion

        #region Private Methods
        private void OnPropertyChanged(string propertyChanged)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyChanged));
        }
        #endregion
    }
}
