using UnityEngine;
using UnityEngine.UI;

public class VolumeButtonSetting : MonoBehaviour
{
    [SerializeField] private Sprite soundOnIcon;
    [SerializeField] private Sprite soundOffIcon;

    private Button button;
    private Image buttonImage;

    private void Start()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();

        button.onClick.AddListener(ToggleSound);

        UpdateIcon();
    }

    private void ToggleSound()
    {
        SoundManager.Instance.ToggleMute();
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (SoundManager.Instance.IsMuted())
        {
            buttonImage.sprite = soundOffIcon;
        }
        else
        {
            buttonImage.sprite = soundOnIcon;
        }
    }
}
