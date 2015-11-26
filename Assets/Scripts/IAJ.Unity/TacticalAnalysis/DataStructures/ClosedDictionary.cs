using System.Collections.Generic;
using RAIN.Navigation.Graph;

namespace Assets.Scripts.IAJ.Unity.TacticalAnalysis.DataStructures
{
    public class ClosedLocationRecordDictionary : IClosedLocationRecord
    {
        private Dictionary<NavigationGraphNode,LocationRecord> Closed { get; set; }

        public ClosedLocationRecordDictionary()
        {
            this.Closed = new Dictionary<NavigationGraphNode, LocationRecord>();
        }
        public void Initialize()
        {
            this.Closed.Clear();
        }

        public void AddToClosed(LocationRecord nodeRecord)
        {
            this.Closed.Add(nodeRecord.Location,nodeRecord);
        }

        public void RemoveFromClosed(LocationRecord nodeRecord)
        {
            this.Closed.Remove(nodeRecord.Location);
        }

        public LocationRecord SearchInClosed(LocationRecord nodeRecord)
        {
            if (this.Closed.ContainsKey(nodeRecord.Location))
            {
                return this.Closed[nodeRecord.Location];
            }
            else return null;
        }

        public ICollection<LocationRecord> All()
        {
            return this.Closed.Values;
        }
    }
}
