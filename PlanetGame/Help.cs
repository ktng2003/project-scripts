using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour
{
    public List<GameObject> list_Content = new List<GameObject>();
    public List<GameObject> list_Page = new List<GameObject>();

    public List<Button> list_Menu = new List<Button>();

    public Color32 selectedColor;
    public Color32 unSelectedColor;
    public Color32 enabledPageColor;
    public Color32 disabledPageColor;

    public Button btPrev;
    public Button btNext;

    private int index = 0;

    private void OnEnable()
    {
        OnPlanetOrder();
        index = 0;
    }

    public void OnPlanetOrder()
    {
        list_Menu[0].image.color = selectedColor;
        list_Menu[1].image.color = unSelectedColor;
        list_Menu[0].interactable = false;
        list_Menu[1].interactable = true;
        list_Content[0].SetActive(true);
        list_Content[1].SetActive(false);
    }

    public void OnHowToPlay()
    {
        list_Menu[0].image.color = unSelectedColor;
        list_Menu[1].image.color = selectedColor;
        list_Menu[0].interactable = true;
        list_Menu[1].interactable = false;
        list_Content[0].SetActive(false);
        list_Content[1].SetActive(true);

        list_Page[0].SetActive(true);
        list_Page[1].SetActive(false);
        list_Page[2].SetActive(false);
        btPrev.image.color = disabledPageColor;
        btNext.image.color = enabledPageColor;
        btPrev.interactable = false;
        btNext.interactable = true;
    }

    public void NextPage()
    {
        if (index == 0)
        {
            list_Page[0].SetActive(false);
            list_Page[1].SetActive(true);
            btPrev.image.color = enabledPageColor;
            btPrev.interactable = true;
            index += 1;
        }
        else if (index == 1)
        {
            list_Page[1].SetActive(false);
            list_Page[2].SetActive(true);
            btNext.image.color = disabledPageColor;
            btNext.interactable = false;
            index += 1;
        }
    }

    public void PrevPage()
    {
        if (index == 2)
        {
            list_Page[2].SetActive(false);
            list_Page[1].SetActive(true);
            btNext.image.color = enabledPageColor;
            btNext.interactable = true;
            index -= 1;
        }
        else if (index == 1)
        {
            list_Page[1].SetActive(false);
            list_Page[0].SetActive(true);
            btPrev.image.color = disabledPageColor;
            btPrev.interactable = false;
            index -= 1;
        }
    }
}