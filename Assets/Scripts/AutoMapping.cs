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
    * �w�肵���ʒu���}�b�s���O����
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
    * �\�������Z�b�g����
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
    * Z���ɑ΂��Ĕ��΂̒l��Ԃ�
    */
    private int ToMirrorX(int xgrid)
    {
        return minimap.Length - xgrid - 1;
    }
}
