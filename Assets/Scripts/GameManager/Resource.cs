using Assets.Scripts.IAJ.Unity.TacticalAnalysis;
using RAIN.Navigation.Graph;

namespace Assets.Scripts.GameManager
{
    public class Resource : IInfluenceUnit
    {
        public NavigationGraphNode Location { get; private set; }
        public float DirectInfluence { get; private set; }

        public Resource(NavigationGraphNode location, float influence)
        {
            this.DirectInfluence = influence;
            this.Location = location;
        }
    }
}
