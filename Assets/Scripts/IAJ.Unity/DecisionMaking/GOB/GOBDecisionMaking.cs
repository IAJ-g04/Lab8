using System.Collections.Generic;

namespace Assets.Scripts.IAJ.Unity.DecisionMaking.GOB
{
    public static class GOBDecisionMaking
    {
        public static float CalculateDiscontentment(Action action, List<Goal> goals)
        {
            var discontentment = 0.0f;
            var duration = action.GetDuration();

            foreach (var goal in goals)
            {
                var newValue = goal.InsistenceValue + action.GetGoalChange(goal);
                newValue += duration*goal.ChangeRate;
                if (newValue > 10.0f)
                {
                    newValue = 10.0f;
                }
                else if (newValue < 0.0f)
                {
                    newValue = 0.0f;
                }
                discontentment += goal.GetDiscontentment(newValue);
            }

            return discontentment;
        }

        public static Action ChooseAction(List<Action> actions, List<Goal> goals)
        {
            Action bestAction = null;
            var bestValue = float.PositiveInfinity;

            foreach (var action in actions)
            {
                if (action.CanExecute())
                {
                    var value = CalculateDiscontentment(action, goals);
                    if (value < bestValue)
                    {
                        bestAction = action;
                        bestValue = value;
                    }
                }
                
            }

            return bestAction;
        }
    }
}
