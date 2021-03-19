using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get { return _instance; }
        private set { }
    }

    public virtual void Start()
    {
        if (_instance == null)
        {
            _instance = (T)this;
        }
        else
        {
            Debug.LogError("Multiple instances attempted to be created for " + _instance.name);
        }
    }
}
