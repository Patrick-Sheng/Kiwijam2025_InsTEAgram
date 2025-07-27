using UnityEngine;
using UnityEngine.UI;

public class RelationshipButton : MonoBehaviour
{
    [SerializeField] private Image buttonImage;
    [SerializeField] private Sprite disabledSprite;
    [SerializeField] private Sprite enabledSprite;

    [field: SerializeField] public RelationshipType relationshipType { get; private set; }

    private bool isEnabled = false;
    
    public void DisableSprite()
    {
        isEnabled = false;
        UpdateSprite();
    }

    public void EnableSprite()
    {
        isEnabled = true;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (isEnabled)
        {
            buttonImage.sprite = enabledSprite;
        }
        else
        {
            buttonImage.sprite = disabledSprite;
        }
    }
}
