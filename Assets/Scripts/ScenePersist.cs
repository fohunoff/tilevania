using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    void Awake()
    {
        int numScenePersist = FindObjectsOfType<ScenePersist>().Length;
        
        if (numScenePersist > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Нужен для того, чтобы, к примеру, при переходе на новый уровень текущий ScenePersist удалялся
    // и использовался только тот, который принадлежит к уровню
    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }
}
