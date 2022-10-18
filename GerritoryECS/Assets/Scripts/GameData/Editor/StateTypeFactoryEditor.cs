using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.Linq;
using System.IO;
using JCMG.EntitasRedux.Editor;

[CustomEditor(typeof(StateTypeFactory))]	
public class StateTypeFactoryEditor : Editor
{
	private StateTypeFactory m_Target;

	private const string k_EnumFileName = "StateTypeEnum.cs";

	private const string k_EnumFileTemplate = @"
/* Generated file. You should avoid modifying this manually */

public enum StateTypeEnum
{
	${EnumElementsList}
}
";

	private const string k_EnumElementsListToken = "${EnumElementsList}";

	private string m_VerificationResult = "";

	private void OnEnable()
	{
		m_Target = target as StateTypeFactory;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button(new GUIContent("Verify StateType Data", "Make sure all the state type data are legal/valid")))
		{
			checkIfStateTypeDataIsLegal(m_Target, out m_VerificationResult);
		}

		if (!m_VerificationResult.IsNullOrEmpty())
		{
			EditorGUILayout.HelpBox(m_VerificationResult, MessageType.Error);
		}

		GUILayout.Space(3);

		if (GUILayout.Button(new GUIContent("Generate StateTypeEnum", "Generate StateTypeEnum with a list of state type names in it. The filename is StateTypeEnum.cs")))
		{
			if (!checkIfStateTypeDataIsLegal(m_Target, out m_VerificationResult))
			{
				Debug.LogError("Failed to generate StateTypeEnum because some of the StateTypes are invalid. See more details in the StateTypeFactory inspector.");
				return;
			}

			generateStateTypeEnumFromFactory(m_Target);
		}
	}

	private bool checkIfStateTypeDataIsLegal(StateTypeFactory factory, out string result)
	{
		result = "";

		var duplicateStateTypeNames = factory.StateTypes.GroupBy(stateType => stateType.Name).Where(group => group.Count() > 1).Select(group => group.Key).ToList();
		foreach (var duplicateStateTypeName in duplicateStateTypeNames)
		{
			result += $"More than one state types have the name of '{duplicateStateTypeName}'.";
		}

		foreach (var stateType in factory.StateTypes)
		{
			var stateTypeName = stateType.Name;
			bool isValidName = System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(stateTypeName);
			if (!isValidName)
			{
				result += $"'{stateTypeName}' is not a valid identifier state type name.";
			}
		}

		return result.IsNullOrEmpty();
	}

	private void generateStateTypeEnumFromFactory(StateTypeFactory factory)
	{
		var stateTypeEnumElementsList = string.Join
		(
			",\n\t",
			factory.StateTypes.Select((stateType, index) => $"{stateType.Name} = {index}")
		);

		string generatedEnumText = k_EnumFileTemplate.Replace(k_EnumElementsListToken, stateTypeEnumElementsList);
		string path = Path.GetDirectoryName(AssetDatabase.GetAssetPath(factory)) + "\\" + k_EnumFileName;
		Debug.Log(generatedEnumText);

		TextAsset scriptAsset = new TextAsset();
		AssetDatabase.CreateAsset(scriptAsset, path);

		AssetDatabase.Refresh();
		System.IO.File.WriteAllText(System.IO.Path.GetDirectoryName(Application.dataPath) + "\\" + path, generatedEnumText);

		AssetDatabase.Refresh();
		AssetDatabase.SaveAssets();
	}
}
