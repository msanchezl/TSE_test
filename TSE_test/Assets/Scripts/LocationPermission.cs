using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;
using TMPro;

public class LocationPermission : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _locationStatusText;
    [SerializeField] private Button _addManualPermissionBtn;

#if UNITY_ANDROID
    private void Start()
    {
        _addManualPermissionBtn.gameObject.SetActive(false);
        SetText("Verificando permiso de ubicación...", Color.white);

        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            SetText("Permiso de ubicación concedido.", Color.green);
        }
        else
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            InvokeRepeating("CheckPermissionStatus", 1f, 1f);
        }
    }

    private void CheckPermissionStatus()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            SetText("Permiso de ubicación concedido.", Color.green);
        }
        else if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            SetText("Permiso de ubicación denegado permanentemente.", Color.red);
            _addManualPermissionBtn.gameObject.SetActive(true);
        }

        CancelInvoke("CheckPermissionStatus");
    }

    public void OpenAppSettings()
    {

        try
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", "android.settings.APPLICATION_DETAILS_SETTINGS");
            AndroidJavaObject uri = new AndroidJavaClass("android.net.Uri").CallStatic<AndroidJavaObject>("parse", "package:" + Application.identifier);
            intent.Call<AndroidJavaObject>("setData", uri);
            currentActivity.Call("startActivity", intent);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al intentar abrir la configuración: " + e.Message);
        }
    }
#endif

    private void SetText(string text, Color color)
    {
        _locationStatusText.text = text;
        _locationStatusText.color = color;
    }
}