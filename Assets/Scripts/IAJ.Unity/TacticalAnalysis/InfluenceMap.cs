using System;
using System.Collections.Generic;
using Assets.Scripts.IAJ.Unity.TacticalAnalysis.DataStructures;
using RAIN.Navigation.NavMesh;

namespace Assets.Scripts.IAJ.Unity.TacticalAnalysis
{
    public class InfluenceMap
    {
        public uint NodesPerFlood { get; set; }
        private NavMeshPathGraph NavMeshGraph { get; set; }
        private List<IInfluenceUnit> Units { get; set; }
        private float InfluenceThreshold { get; set; }
        private IInfluenceFunction InfluenceFunction { get; set; }
        private IOpenLocationRecord Open { get; set; }
        public IClosedLocationRecord Closed { get; set; }
        public bool InProgress { get; set; }

        public InfluenceMap(NavMeshPathGraph navMesh, IOpenLocationRecord open, IClosedLocationRecord closed, IInfluenceFunction influenceFunction, float influenceThreshold)
        {
            this.NavMeshGraph = navMesh;
            this.Open = open;
            this.Closed = closed;
            this.InfluenceFunction = influenceFunction;
            this.InfluenceThreshold = influenceThreshold;
            this.NodesPerFlood = 100;
        }

        public void Initialize(List<IInfluenceUnit> units)
        {
            this.Open.Initialize();
            this.Closed.Initialize();
            this.Units = units;
            
            foreach (var unit in units)
            {
                //I need to do this because in Recast NavMesh graph, the edges of polygons are considered to be nodes and not the connections.
                //Theoretically the Quantize method should then return the appropriate edge, but instead it returns a polygon
                //Therefore, we need to create one explicit connection between the polygon and each edge of the corresponding polygon for the search algorithm to work
                ((NavMeshPoly)unit.Location).AddConnectedPoly(unit.Location.Position);

                var locationRecord = new LocationRecord
                {
                    Influence = unit.DirectInfluence,
                    StrongestInfluenceUnit = unit,
                    Location = unit.Location
                };

                Open.AddToOpen(locationRecord);
            }

            this.InProgress = true;
        }

        //this method should return true if it finished processing, and false if it still needs to continue
        public bool MapFloodDijkstra()
        {
            var processedNodes = 0;

            //TODO implement
            throw new NotImplementedException();

            
            this.InProgress = false;
            this.CleanUp();
            return true;
        }


        public void CleanUp()
        {
            foreach (var unit in this.Units)
            {
                ((NavMeshPoly)unit.Location).RemoveConnectedPoly();
            }
        }
        
    }
}
