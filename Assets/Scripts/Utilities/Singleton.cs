using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; }

    public virtual void Start()
    {
        if (Instance == null)
        {
            Instance = (T)this;
        }
        else
        {
            Debug.LogError("Multiple instances attempted to be created for " + Instance.name);
        }
    }
}
