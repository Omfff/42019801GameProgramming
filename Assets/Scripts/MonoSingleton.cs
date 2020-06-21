using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{

    // 单例
    private static T instance;

    /// <summary>获取单例</summary>
    public static T GetInstance()
    {
        if (instance == null)
        {
            // 在已存在的脚本中查找单例
            instance = (T)FindObjectOfType(typeof(T));

            // 创建单例
            if (instance == null) new GameObject(typeof(T).Name).AddComponent<T>();
        }
        return instance;
    }

    // 获取单例/销毁重复对象
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else if (instance != this)
        {
            if (instance.gameObject != gameObject)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(this);
            }
        }
    }

    // 重空单例
    protected virtual void OnDestroy()
    {
        if (instance == this) instance = null;
    }

    // 初始化
    protected virtual void Init()
    {

    }

}