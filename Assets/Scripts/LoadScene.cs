using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
	public class LoadScene : MonoBehaviour
	{

		public void LoadScenByIndex(int index)
		{
			SceneManager.LoadSceneAsync(index);
			SceneManager.sceneLoaded += SceneManager_sceneLoaded;
		}

		private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
		{
			LightmapSettings.lightProbes = null;
			LightProbes.TetrahedralizeAsync();
			SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
		}
	}
}