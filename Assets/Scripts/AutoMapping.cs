using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMapping : MonoBehaviour
{
    public GameObject roads;
    public GameObject enemies;
    public GameObject items;
    public GameObject roadImage;

    private float pw, ph;
    private int[,] minimap;

    private void Start()
    {
        RectTransform rect = roadImage.GetComponent<RectTransform>();
        pw = rect.sizeDelta.x;
        ph = rect.sizeDelta.y;
    }

    /*
    * 指定した位置をマッピングする
    */
    public void Mapping(int x, int y, int value)
    {
        minimap[x, y] = value;
        if (value == 1)
        {
            GameObject road = Instantiate(roadImage, roads.transform);
            road.GetComponent<RectTransform>().anchoredPosition = new Vector2(pw * ToMirrorX(x), ph * -y);
        }
    }

    /*
    * 表示をリセットする
    */
    public void Reset(int width, int height)
    {
        for (int i = 0; i < roads.transform.childCount; i++)
            Destroy(roads.transform.GetChild(i).gameObject);
        for (int i = 0; i < enemies.transform.childCount; i++)
            Destroy(enemies.transform.GetChild(i).gameObject);
        for (int i = 0; i < items.transform.childCount; i++)
            Destroy(items.transform.GetChild(i).gameObject);
        minimap = new int[width, height];
        GetComponent<RectTransform>().sizeDelta = new Vector2(width * pw, height * ph);
    }

    /**
    * Z軸に対して反対の値を返す
    */
    private int ToMirrorX(int xgrid)
    {
        return minimap.Length - xgrid - 1;
    }
}
