using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CreditsScript : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag your Video Player GameObject here, or leave empty to auto-find on this object.")]
    public VideoPlayer videoPlayer;

    [Header("Scene Settings")]
    [Tooltip("Exact name of the scene you want to load next.")]
    public string menuSceneName = "Menu";

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoFinished;
        }
        else
        {
            Debug.LogError("VideoSceneSwitcher: No Video Player found!", this);
        }
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        if (!string.IsNullOrEmpty(menuSceneName))
        {
            SceneManager.LoadScene(menuSceneName);
        }
        else
        {
            Debug.LogError("VideoSceneSwitcher: Next Scene Name is not set!", this);
        }
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoFinished;
        }
    }
}
