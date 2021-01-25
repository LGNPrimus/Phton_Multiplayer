using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManag : MonoBehaviour
{
    [Header("Pannels")]
    public GameObject MainpNL;
    public static GameObject GameModePNL;

    [Header("Buttons")]
    public GameObject BackBTNGameMode;

    public void BackBTNFunGameMode()
    {
        MainpNL.SetActive(true);
        GameModePNL.SetActive(false);
    }
  
    public static void LoadGameMode()
    {
        GameModePNL.SetActive(true);
    }
}
