using UnityEngine;
using UnityEngine.UI;

public class Train : MonoBehaviour
{
    public Image Left_Image;
    public Image Right_Image;
    public Button Button;

    public string Left_ImagePath = "Image/Heaven/Train(Size)/LeftSide";
    public string Right_ImagePath = "Image/Heaven/Train(Size)/RightSide";

    private Sprite[] loadedLeftSprites;
    private Sprite[] loadedRightSprites;

    private void Start()
    {
        Button.onClick.AddListener(OnButtonClick);
        loadedLeftSprites = Resources.LoadAll<Sprite>(Left_ImagePath);
        loadedRightSprites = Resources.LoadAll<Sprite>(Right_ImagePath);

        AssignImageToObject(Left_Image, loadedLeftSprites);
        AssignImageToObject(Right_Image, loadedRightSprites);
    }

    private void AssignImageToObject(Image uiImage, Sprite[] sprites)
    {
        if (sprites.Length == 0)
        {
            Debug.LogError("스프라이트를 찾을 수 없습니다.");
            return;
        }

        int randomIndex = Random.Range(0, sprites.Length);
        Sprite selectedSprite = sprites[randomIndex];

        if (uiImage != null)
        {
            if (uiImage.sprite == null || uiImage.sprite != selectedSprite)
            {
                uiImage.sprite = selectedSprite;
            }
            else
            {
                // 이미 같은 이미지가 할당되어 있을 경우 다른 이미지를 선택합니다.
                int newIndex = (randomIndex + 1) % sprites.Length;
                selectedSprite = sprites[newIndex];
                uiImage.sprite = selectedSprite;
            }
        }
        else
        {
            Debug.LogError("UI 이미지 컴포넌트를 찾을 수 없습니다.");
        }
        Debug.Log("이미지가 할당되었습니다.");
    }

    private void OnButtonClick()
    {
        AssignImageToObject(Left_Image, loadedLeftSprites);
        AssignImageToObject(Right_Image, loadedRightSprites);
    }
}
