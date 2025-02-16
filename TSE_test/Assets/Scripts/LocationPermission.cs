using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class LocationPermission : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _locationStatusText;
    [SerializeField] private Button _addMaunalPermissionBtn;

    private void Start()
    {
        _addMaunalPermissionBtn.gameObject.SetActive(false);
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
            CancelInvoke("CheckPermissionStatus");
        }
        else if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            SetText("Permiso de ubicación denegado permanentemente.", Color.red);
            _addMaunalPermissionBtn.gameObject.SetActive(true);
            CancelInvoke("CheckPermissionStatus");
        }
    }

    //TODO: is not working
    public void OpenAppSettings()
    {
#if UNITY_ANDROID
        Application.OpenURL("package:" + Application.identifier);
#endif
    }

    private void SetText(string text, Color color)
    {
        _locationStatusText.text = text;
        _locationStatusText.color = color;

    }
}
