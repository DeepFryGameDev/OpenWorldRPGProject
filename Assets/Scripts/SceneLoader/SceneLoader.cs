using Prime31.TransitionKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public float crossfadeTransitionTime = 1f;
    public Animator crossfadeTransition;


    /// <summary>
    /// Begins scene transition animation when changing scenes
    /// </summary>
    /// <param name="sceneName">Name of scene to be loaded</param>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(SceneTransition(sceneName));
    }

    /// <summary>
    /// Coroutine.  Facilitates processing scene transition animation
    /// </summary>
    /// <param name="sceneName">Name of scene to be loaded after animation</param>
    IEnumerator SceneTransition(string sceneName)
    {
        //GameObject.Find("Player").GetComponent<PlayerController2D>().enabled = false;

        crossfadeTransition.SetTrigger("Start");
        
        yield return new WaitForSeconds(crossfadeTransitionTime);
        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(.1f);
        //GameObject.Find("Player").GetComponent<PlayerController2D>().enabled = false;

        yield return new WaitForSeconds(1.5f);

        //GameObject.Find("Player").GetComponent<PlayerController2D>().enabled = true;
    }


    //-----Tools for above methods-----
    
    /// <summary>
    /// Returns index from given scene name
    /// </summary>
    /// <param name="sceneName">Name of scene to gather index</param>
    private int sceneIndexFromName(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string testedScreen = NameFromIndex(i);
            //print("sceneIndexFromName: i: " + i + " sceneName = " + testedScreen);
            if (testedScreen == sceneName)
                return i;
        }
        return -1;
    }

    /// <summary>
    /// Returns name of scene from given index - checks scenes from path so the loaded scenes issue can be worked around
    /// </summary>
    /// <param name="BuildIndex">Index of scene to gather name</param>
    private static string NameFromIndex(int BuildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }

}
