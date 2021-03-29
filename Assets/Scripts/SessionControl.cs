using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/Settings/Session Control")]
public class SessionControl : ScriptableObject
{
	bool _isPaused = false;
	public event Action<bool> onPaused;
	public void SetPaused(bool value)
	{
		if (value)
		{
			Time.timeScale = 0;
		}
		else
		{
			Time.timeScale = 1;
		}
		_isPaused = value;
		onPaused?.Invoke(_isPaused);
	}

	public void TogglePaused()
	{
		SetPaused(!_isPaused);
	}

	public void SetTimeScale(float value)
	{
		Time.timeScale = Mathf.Clamp01(value);
		_isPaused = Time.timeScale <= 0;
	}

	public void ReloadActiveScene()
	{
		SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void LockCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void ConfineCursor()
	{
		Cursor.lockState = CursorLockMode.Confined;
	}

	public void FreeCursor()
	{
		Cursor.lockState = CursorLockMode.None;
	}

	public void SetCursorVisibility(bool value)
	{
		Cursor.visible = value;
	}
}
