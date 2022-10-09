using JCMG.EntitasRedux;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

/// <summary>
/// To indicate the <see cref="InputEntity"/> is waiting for the result of the AI movement evaulation.
/// </summary>
[Input]
public sealed class EvaluatingForMovementInputComponent : IComponent
{
	public SearchBestActionWithMinimaxJob Job;
	public JobHandle JobHandle;
}
