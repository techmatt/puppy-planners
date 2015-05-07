using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    static class GameFunctions
    {
        //
        // most units are in (modified) worker-seconds. So a task with a cost of 60 would take 1 minute
        // (but much less with research, skill, etc.)
        //

        public static double scoutCost(double xDelta, double yDelta)
        {
            double dist = Math.Sqrt(xDelta * xDelta + yDelta * yDelta);

            return -10.0 + 15.0 * Math.Sqrt(dist);
        }

        internal static double skillTrainingToEffectiveness(double trainingTime)
        {
            return Math.Sqrt(trainingTime) / 10.0 + 0.5;
        }
    }
}


public static class Extensions
{
	public static int ArgMax (this List<double> list)
	{
		if (list.Count==0)
			return 0;

		var max = list[0];
		int index = 0;
		for (int i = 1; i < list.Count; i++)
			if (list[i]>max)
			{
				max = list[i];
				index = i;
			}
			
		return index;
	}
	public static int ArgMin (this List<double> list)
	{
		if (list.Count==0)
			return 0;

		var max = list[0];
		int index = 0;
		for (int i = 1; i < list.Count; i++)
			if (list[i]<max)
			{
				max = list[i];
				index = i;
			}

		return index;
	}
}