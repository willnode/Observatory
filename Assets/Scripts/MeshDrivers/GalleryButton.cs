
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(LayoutElement))]
public class GalleryButton : MonoBehaviourEX
{

    public string detail;
    public string head;
    public string body;
    
    [HideInInspector]
    public Toggle toggle;

    public void Init (string imagePath, string hd, string bd, string desc, ToggleGroup group) {
        StartCoroutine(LoadAsync(imagePath));
        detail = desc;
        body = bd;
        head = hd;
        toggle.group = group;
    }

    public void OnClick(bool check) {
        if (check)
            GM.Main<Mode3Gallery>().SetGalleryText(this);
    }
    
    IEnumerator LoadAsync (string path) {
        if (string.IsNullOrEmpty(path))
        {
            GO.SetActive(false);
            yield break;
        }
        var r = Resources.LoadAsync<Sprite>(path);
        yield return r;
        var image = (Sprite)r.asset;
        if (image) {
            IM.sprite = image;
            var width = RT.rect.width;
            LE.preferredHeight = width / Mathf.Max(0.5f, image.rect.size.GetAspect());            
        }
    }
}