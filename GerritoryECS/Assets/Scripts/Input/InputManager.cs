using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public sealed class InputManager
{
	private static InputManager s_Instance = null;
	public static InputManager Instance 
	{
		get
		{
			if (s_Instance == null)
			{
				s_Instance = new InputManager();
			}

			if (s_Instance == null)
			{
				Debug.LogError("InputManager singleton instance cannot be created.");
			}

			return s_Instance;
		}
	}

	private InputActionManager m_SystemInput;
	public InputActionManager SystemInput { get { return m_SystemInput; } }

	struct UserInputSettings
	{
		public InputUser User;
		public InputActionManager InputActions;
	}
	/// <summary>
	/// Dictionary that maps UserIndex (int) to PlayerInputSettings
	/// </summary>
	private Dictionary<int, UserInputSettings> m_UserInputSettingsCollection = new Dictionary<int, UserInputSettings>();

	private InputManager()
	{
		m_SystemInput = new InputActionManager();
	}

	private UserInputSettings getOrCreateUserInputSettings(int userIndex)
	{
		if (m_UserInputSettingsCollection.TryGetValue(userIndex, out UserInputSettings playerInput))
		{
			return playerInput;
		}

		// Create a new user with the given index
		InputActionManager newInputActions = new InputActionManager();
		InputUser newUser = InputUser.PerformPairingWithDevice(InputSystem.GetDevice<Keyboard>());
		newUser.AssociateActionsWithUser(newInputActions);
		newUser.ActivateControlScheme($"Player{userIndex}");
		newInputActions.Enable();

		m_UserInputSettingsCollection.Add(userIndex, new UserInputSettings() { User = newUser, InputActions = newInputActions });
		return m_UserInputSettingsCollection[userIndex];
	}

	public InputActionManager GetOrCreateUserInputWithIndex(int userIndex)
	{
		return getOrCreateUserInputSettings(userIndex).InputActions;
	}

	public void PairDeviceToUser(InputDevice device, int userIndex)
	{
		if (device == null)
		{
			Debug.LogError("The given device is null.");
			return;
		}

		InputUser user = getOrCreateUserInputSettings(userIndex).User;
		InputUser.PerformPairingWithDevice(device, user: user);
	}
}
