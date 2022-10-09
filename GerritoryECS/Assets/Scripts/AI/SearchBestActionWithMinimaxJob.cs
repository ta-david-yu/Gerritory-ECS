using Unity.Collections;
using Unity.Jobs;

public struct SearchBestActionWithMinimaxJob : IJob
{
	public AIHelper.SearchSimulationState SimulationState;
	public AIHelper.MinimaxInput Input;
	public int RandomSeedIndex;

	public NativeArray<AIHelper.MinimaxResult> ResultContainer;

	public void Execute()
	{
		ResultContainer[0] = AIHelper.minimax(Input, ref SimulationState, RandomSeedIndex);
	}
}
