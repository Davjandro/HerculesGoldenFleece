using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Start is called before the first frame update
   public string LevelName;

   public void LoadLevel()
   {
    SceneManager.LoadScene(LevelName);
   }
}
