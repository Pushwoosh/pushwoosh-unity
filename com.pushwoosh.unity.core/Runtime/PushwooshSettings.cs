using UnityEngine;

public class PushwooshSettings : ScriptableObject
{
    private const string ResourcePath = "PushwooshSettings";

    [SerializeField] private string _applicationCode = "";

    public string ApplicationCode => _applicationCode;

    private static PushwooshSettings _instance;

    public static PushwooshSettings Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<PushwooshSettings>(ResourcePath);
            return _instance;
        }
    }
}
