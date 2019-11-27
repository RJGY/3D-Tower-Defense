using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopScrollbar : MonoBehaviour
{
    private BufferPanel bufferPanel;
    private Scrollbar scrollbar;
    private Button[] buttons;
    private ShopScrollBuffer scrollBuffer;
    private float size;
    private void Awake()
    {
        bufferPanel = FindObjectOfType<BufferPanel>();
        scrollbar = GetComponent<Scrollbar>();
        buttons = bufferPanel.GetComponentsInChildren<Button>();
        scrollBuffer = FindObjectOfType<ShopScrollBuffer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GetSize();
        UpdateScrollBarSize();
        scrollbar.onValueChanged.AddListener(UpdateScrollValue);
    }

    void GetSize()
    {
        size = buttons.Length * 100f;
    }

    void UpdateScrollBarSize()
    {
        scrollbar.size = 400f / size;
    }

    void UpdateScrollValue(float value)
    {
        scrollBuffer.transform.localPosition = new Vector3(0, value * (size - 400f), 0);
    }
}
