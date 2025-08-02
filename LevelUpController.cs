using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelUpController : MonoBehaviour
{
    [SerializeField] private Button[] buttons;
    [SerializeField] private Wpn[] wpns;
    [SerializeField] private Slider XPBar;
    [SerializeField] private GameObject userInterface;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private Image lvlUpImage;
    private bool pressedE;
    public void Update()
    {
        CharacterMovement[] chara = FindObjectsOfType<CharacterMovement>();
        CharacterMovement myChara = chara[0];
        for (int i = 0; i < chara.Length; i++)
        {
            if (chara[i].getView().IsMine)
            {
                myChara = chara[i];
            }
        }
        if(myChara.GetLvl() != 0)
        XPBar.minValue = myChara.GetLvlReq(myChara.GetLvl() - 1);

        XPBar.maxValue = myChara.GetLvlReq(myChara.GetLvl() );
        XPBar.value = myChara.GetXP();


        if (myChara.CharacterLevelUpReady())
        {
            lvlUpImage.enabled = true;

            if (Input.GetKeyDown(KeyCode.E))
            {

                userInterface.SetActive(!userInterface.activeInHierarchy);
                if (!pressedE)
                {
                    RefreshButtons();
                    pressedE = true;
                }
            } 
        }
        else
        {
            lvlUpImage.enabled = false;
        }
    }

    public void RefreshButtons()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Wpn wpn;
            int randomIndex = Random.Range(0, wpns.Length);
            wpn = wpns[randomIndex];
            buttons[i].GetComponentInChildren<TMP_Text>().text = wpns[randomIndex].weapon.ToString() +": Level " + wpns[randomIndex].weaponLvl;
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].onClick.AddListener(() => Upgrade(wpn));
        }
    }

    private void Upgrade(Wpn wpn)
    {
        CharacterMovement[] chara = FindObjectsOfType<CharacterMovement>();
        CharacterMovement myChara = chara[0];
        for (int i = 0; i < chara.Length; i++)
        {
            if(chara[i].getView().IsMine)
            {
                myChara = chara[i];
            }
        }
        myChara.EquipWeapon(wpn);
        myChara.levelActualLevel();
        RefreshButtons();
        HideUI();
    }

    public void updateTimer(string s)
    {
        timer.text = s;
    }

    public void HideUI()
    {
        userInterface.SetActive(!userInterface.activeInHierarchy);
    }
}



[System.Serializable]
public class Wpn
{
    public Weapons weapon;
    public float weaponLvl;
}
