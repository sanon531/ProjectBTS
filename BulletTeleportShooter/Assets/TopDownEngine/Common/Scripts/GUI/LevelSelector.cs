using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// This component allows the definition of a level that can then be accessed and loaded. Used mostly in the level map scene.
    /// </summary>
    [AddComponentMenu("TopDown Engine/GUI/LevelSelector")]
    public class LevelSelector : MonoBehaviour
	{
		/// the exact name of the target level
		[Tooltip("the exact name of the target level")]
		public string LevelName;

		/// if this is true, GoToLevel will ignore the LevelManager and do a direct call
		[Tooltip("if this is true, GoToLevel will ignore the LevelManager and do a direct call")]
		public bool DoNotUseLevelManager = false;

		/// <summary>
		/// Loads the level specified in the inspector
		/// </summary>
	    public virtual void GoToLevel()
	    {
		    if (DoNotUseLevelManager)
		    {
				MMAdditiveSceneLoadingManager.LoadScene(LevelName);    
		    }
		    else
		    {
			    LevelManager.Instance.GotoLevel(LevelName);   
		    }
	    }

		public void GotoLevel_NoLoading()
        {
			GameManager.Instance.Reset();
			GameManager.Instance.UnPause();
			SceneManager.LoadScene(LevelName);
        }


        /// <summary>
        /// Restarts the current level, without reloading the whole scene
        /// </summary>
        public virtual void ReviveLevel()
        {
			int PointSave = GameManager.Instance.Points;

			if (GameManager.Instance.Paused)
            {
                TopDownEngineEvent.Trigger(TopDownEngineEventTypes.UnPause, null);
            }
            TopDownEngineEvent.Trigger(TopDownEngineEventTypes.RespawnStarted, null);

			GameManager.Instance.SetPoints(PointSave);
		}

		/// <summary>
		/// Reloads the current level
		/// </summary>
		public virtual void ReloadLevel()
		{
			// we trigger an unPause event for the GameManager (and potentially other classes)
			TopDownEngineEvent.Trigger(TopDownEngineEventTypes.UnPause, null);
			MMSceneLoadingManager.LoadScene(SceneManager.GetActiveScene().name);
	    }

		public virtual void ReloadLevel_NoLoading()
        {
			GameManager.Instance.Reset();
			TopDownEngineEvent.Trigger(TopDownEngineEventTypes.UnPause, null);
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		
	}
}