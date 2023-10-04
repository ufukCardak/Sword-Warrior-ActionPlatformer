using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    public SpriteRenderer[] swordColor;
    public SpriteRenderer[] slashColor;
    public SpriteRenderer[] characterColor;
    public Material[] swordMaterial;
    public Material[] slashMaterial;
    public string hexCodeSword,hexCodeSlash,hexCodeCharacter;
    float alpha = 1f;
    public void SetColor(string colorHexCode)
    {
        Color color;
        for (int i = 0; i < characterColor.Length; i++)
        {
            ColorUtility.TryParseHtmlString("#" + colorHexCode, out color);
            characterColor[i].color = color;
        }
        if (colorHexCode == "FFFFFF")
        {
            colorHexCode = hexCodeSword;
        }
        for (int i = 0; i < swordColor.Length; i++)
        {
            ColorUtility.TryParseHtmlString("#" + colorHexCode, out color);
            swordColor[i].color = color;
        }
    }
    public void SetWeaponColor(string colorHexCode,int glowPower)
    {
        hexCodeSword = colorHexCode;
        Color color;
        for (int i = 0; i < swordColor.Length; i++)
        {
            ColorUtility.TryParseHtmlString("#" + colorHexCode, out color);
            swordColor[i].color = color;  
        }
        for (int i = 0; i < slashColor.Length; i++)
        {
            ColorUtility.TryParseHtmlString("#" + colorHexCode, out color);
            slashColor[i].color = color;
        }
        for (int i = 0; i < swordMaterial.Length; i++)
        {
            swordMaterial[i].SetVector("_GlowColor", Color.white * glowPower);
        }
        for (int i = 0; i < slashMaterial.Length; i++)
        {
            slashMaterial[i].SetVector("_GlowColor", Color.white * glowPower);
        }

    }
    public void SetEnemyWeaponColor(string colorSwordHexCode) 
    {
        Color color;
        if (colorSwordHexCode == "FFFFFF")
        {
            colorSwordHexCode = hexCodeSword;
        }
        for (int i = 0; i < swordColor.Length; i++)
        {
            ColorUtility.TryParseHtmlString("#" + colorSwordHexCode, out color);
            swordColor[i].color = color;
        }
    }
    public void StartColor()
    {
        Color color;
        for (int i = 0; i < swordColor.Length; i++)
        {
            ColorUtility.TryParseHtmlString("#" + hexCodeSword, out color);
            swordColor[i].color = color;
        }
        for (int i = 0; i < slashColor.Length; i++)
        {
            ColorUtility.TryParseHtmlString("#" + hexCodeSlash, out color);
            slashColor[i].color = color;
        }
    }
    public void PlayerStartColor(string colorHexCode,int glowPower)
    {
        Color color;
        hexCodeSword = colorHexCode;
        hexCodeSlash = colorHexCode;

        for (int i = 0; i < swordColor.Length; i++)
        {
            ColorUtility.TryParseHtmlString("#" + colorHexCode, out color);
            swordColor[i].color = color;
        }
        for (int i = 0; i < slashColor.Length; i++)
        {
            ColorUtility.TryParseHtmlString("#" + colorHexCode, out color);
            slashColor[i].color = color;
        }

        ResetMaterialGlow();
        for (int i = 0; i < swordMaterial.Length; i++)
        {
            swordMaterial[i].SetVector("_GlowColor", Color.white * glowPower);
        }
        for (int i = 0; i < slashMaterial.Length; i++)
        {
            slashMaterial[i].SetVector("_GlowColor", Color.white * glowPower);
        }
    }
    private void ResetMaterialGlow()
    {
        for (int i = 0; i < swordMaterial.Length; i++)
        {
            swordMaterial[i].SetVector("_GlowColor", Color.white * 0);
        }
        for (int i = 0; i < slashMaterial.Length; i++)
        {
            slashMaterial[i].SetVector("_GlowColor", Color.white * 0);
        }
    }

    public IEnumerator DyingAnim(float waitSec)
    {
        for (int j = 0; j < 4; j++)
        {
            alpha -= 0.25f;
            for (int i = 0; i < swordColor.Length; i++)
            {
                swordColor[i].color = new Color(swordColor[i].color.r, swordColor[i].color.g, swordColor[i].color.b, alpha);
            }
            for (int i = 0; i < slashColor.Length; i++)
            {
                slashColor[i].color = new Color(slashColor[i].color.r, slashColor[i].color.g, slashColor[i].color.b, alpha);
            }
            for (int i = 0; i < characterColor.Length; i++)
            {
                characterColor[i].color = new Color(characterColor[i].color.r, characterColor[i].color.g, characterColor[i].color.b, alpha);
            }
            yield return new WaitForSeconds(waitSec);
        }

    }
}
