using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using UnityEditor;

[InitializeOnLoad]
#endif
public sealed class AnalogStickInGameMovementProcessor : InputProcessor<Vector2>
{
	public float RotationAngle = -40;

#if UNITY_EDITOR
	static AnalogStickInGameMovementProcessor()
	{
		initialize();
	}
#endif

	[RuntimeInitializeOnLoadMethod]
	private static void initialize()
	{
		InputSystem.RegisterProcessor<AnalogStickInGameMovementProcessor>();
	}

	public override Vector2 Process(Vector2 value, InputControl control)
	{
		Vector2 mappedValue;
		float cos = Mathf.Cos(RotationAngle * Mathf.Deg2Rad);
		float sin = Mathf.Sin(RotationAngle * Mathf.Deg2Rad);
		mappedValue.x = cos * value.x - sin * value.y;
		mappedValue.y = sin * value.x + cos * value.y;

		return mappedValue;
	}
}
