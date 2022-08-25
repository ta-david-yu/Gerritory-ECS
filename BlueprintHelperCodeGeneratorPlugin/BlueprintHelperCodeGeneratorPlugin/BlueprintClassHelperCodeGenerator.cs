/*

MIT License

Copyright (c) 2020 Jeff Campbell

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using EntitasRedux.Core.Plugins;
using Genesis.Plugin;

namespace BlueprintHelperClassGeneratorPlugin
{
	internal sealed class BlueprintHelperClassGenerator : ICodeGenerator
	{
		public string Name => NAME;

		public int Priority => DEFAULT_PRIORITY;

		public bool RunInDryMode => DEFAULT_DRY_RUN_MODE;

		private const string NAME = "Blueprint Helper Class Generator";
		private const int DEFAULT_PRIORITY = 0;
		private const bool DEFAULT_DRY_RUN_MODE = true;

		// Code-generation format strings
		private const string CREATE_ENTITY_FROM_BLUEPRINT_CLASS_TEMPLATE = @"
using UnityEngine;
using UnityEngine.Events;
using JCMG.EntitasRedux;

/// <summary>
/// Create an entity from the given <see cref=""${ContextName}Entity""/> blueprint component and link it to this gameObject
/// </summary>
[RequireComponent(typeof(${ContextName}BlueprintBehaviour))]
public class CreateEntityFrom${ContextName}Blueprint : MonoBehaviour
{
	[System.Serializable]
	public class ExecuteEvents
	{
		[Tooltip(""Invoked when the entity is created"")]
		public UnityEvent<IEntity> OnEntityCreated;

		[Tooltip(""Invoked when the given blueprint is applied to the created entity"")]
		public UnityEvent<IEntity> OnBlueprintApplied;
	}

	public enum ExecutionTime
	{
		Never,
		Awake,
		Start,
	}

	[SerializeField]
	private ExecutionTime m_ExecutionTime = ExecutionTime.Never;

	[SerializeField]
	private ${ContextName}BlueprintBehaviour m_Blueprint;

	public ExecuteEvents Events = new ExecuteEvents();

	private EntityLink m_LinkedEntity = null;

	private void Awake()
	{
		if (m_ExecutionTime == ExecutionTime.Awake)
		{
			Execute();
		}
	}

	// Start is called before the first frame update
	private void Start()
	{
		if (m_ExecutionTime == ExecutionTime.Start)
		{
			Execute();
		}
	}

	private void OnValidate()
	{
		if (m_Blueprint == null)
		{
			m_Blueprint = GetComponent<${ContextName}BlueprintBehaviour>();
		}
	}

	public void Execute()
	{
		if (m_LinkedEntity != null)
		{
			Debug.LogWarning($""There is already an entity ({m_LinkedEntity.Entity.CreationIndex}) created from this behaveiour attached to the gameObject ${gameObject.name}."");
			return;
		}

		var entity = Contexts.SharedInstance.${ContextName}.CreateEntity();
		Events.OnEntityCreated?.Invoke(entity);

		m_Blueprint.ApplyToEntity(entity);
		Events.OnBlueprintApplied?.Invoke(entity);

		m_LinkedEntity = gameObject.Link(entity);
	}
}
";

		// Find and replace tokens
		private const string CONTEXT_NAME_TOKEN = "${ContextName}";

		private const string CREATE_ENTITY_FROM_BLUEPRINT_FILENAME_FORMAT = "CreateEntityFrom${ContextName}Blueprint.cs";

		public CodeGenFile[] Generate(CodeGeneratorData[] data)
		{
			var codeGenFilesResult = new List<CodeGenFile>();

			// Create a blueprint class per context
			var allContextData = data.OfType<ContextData>().ToArray();
			for (var i = 0; i < allContextData.Length; i++)
			{
				var contextData = allContextData[i];
				var contextName = contextData.GetContextName();

				// Create Blueprint MonoBehaviour
				var behaviourFilename = CREATE_ENTITY_FROM_BLUEPRINT_FILENAME_FORMAT.Replace(CONTEXT_NAME_TOKEN, contextName);
				var behaviourAbsoluteFilename = Path.Combine(contextName, behaviourFilename);
				var behaviourFileContents = CREATE_ENTITY_FROM_BLUEPRINT_CLASS_TEMPLATE.Replace(CONTEXT_NAME_TOKEN, contextName);
				codeGenFilesResult.Add(new CodeGenFile(behaviourAbsoluteFilename, behaviourFileContents, NAME));
			}

			return codeGenFilesResult.ToArray();
		}
	}
}