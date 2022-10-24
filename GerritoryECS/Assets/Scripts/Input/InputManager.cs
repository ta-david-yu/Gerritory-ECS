using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	private List<InputDevice> m_UnpairedGamepadDevices = new List<InputDevice>();

	private InputManager()
	{
		m_SystemInput = new InputActionManager();

		var unpairedInputDevices = InputUser.GetUnpairedInputDevices();
		m_UnpairedGamepadDevices = unpairedInputDevices.Where((inputDevice) => inputDevice is Gamepad).ToList();

		InputSystem.onDeviceChange += handleOnDeviceChange;
	}

	private void handleOnDeviceChange(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
	{
		bool isGamepad = inputDevice is Gamepad;
		if (!isGamepad)
		{
			// For now, we are only interested in gamepad/controll devices. Therefore if it's not, we skip it!
			return;
		}

		Debug.Log("Gamepad Change Event: " + inputDeviceChange);

		// TODO: do pairing with previously diconnected user
		// ...

		switch (inputDeviceChange)
		{
			case InputDeviceChange.Added:
				m_UnpairedGamepadDevices.Add(inputDevice);
				break;
			case InputDeviceChange.Removed:
				m_UnpairedGamepadDevices.Remove(inputDevice);
				break;
			case InputDeviceChange.Disconnected:
				break;
			case InputDeviceChange.Reconnected:
				break;
		}
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
		
		if (m_UnpairedGamepadDevices.Count > 0)
		{
			// Pair a gamepad to the user if there is an available device
			InputDevice inputDevice = m_UnpairedGamepadDevices[0];
			InputUser.PerformPairingWithDevice(inputDevice, newUser);

			// TODO: do pairing with previously diconnected user
			// ...

			m_UnpairedGamepadDevices.RemoveAt(0);
		}

		// Associate input actions mapping with the user according to the UserIndex
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
