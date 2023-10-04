using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerEXP : Player
{
    //[SerializeField] private PlayerScriptableObject playerScriptableObject;
    [SerializeField] private JsonController playerJson;
    [SerializeField] private TextMeshProUGUI levelText,coinText;
    [SerializeField] private Slider expSlider;
    [SerializeField] private float currentExp;
    [SerializeField] public int currentLevel = 1;
    [SerializeField] private int currentWeaponUpgrade = 1;
    [SerializeField] private int currentPlayerUpgrade = 0;
    [SerializeField] GameObject btnWeaponUpgrade,btnPlayerUpgrade,btnHealthBuy;
    Button btnWeapon, btnPlayer,btnHealth;
    TextMeshProUGUI weaponPriceText, weaponLevelText, playerPriceText, playerLevelText, healthPriceText;
    int weaponReqLevel = 0,playerReqLevel = 0;
    int weaponPriceMultiplier = 1, playerPriceMultiplier = 1;
    int glowPower = 1;
    int nextWeaponUpgradePrice, nextPlayerUpgradePrice;
    ChangeColor changeColor;
    string colorHexCode;

    private void Awake()
    {
        playerExp = this;
        transform.position = new Vector3(transform.position.x, transform.position.y, -0.1f);
        changeColor = GetComponent<ChangeColor>();
        changeColor.PlayerStartColor(colorHexCode, glowPower);

        /*currentExp = playerScriptableObject.currentExp;
        currentLevel = playerScriptableObject.currentLevel;
        currentCoin = playerScriptableObject.currentCoin;
        currentWeaponUpgrade = playerScriptableObject.currentWeaponUpgrade;
        currentPlayerUpgrade = playerScriptableObject.currentPlayerUpgrade;
        weaponReqLevel = playerScriptableObject.weaponReqLevel;
        playerReqLevel = playerScriptableObject.playerReqLevel;
        weaponPriceMultiplier = playerScriptableObject.weaponPriceMultiplier;
        playerPriceMultiplier = playerScriptableObject.playerPriceMultiplier;
        glowPower = playerScriptableObject.glowPower;
        colorHexCode = playerScriptableObject.colorHexCode;
        expSlider.maxValue = playerScriptableObject.expSliderMax;
        damage = playerScriptableObject.damage;*/
    }
    private void Start()
    {
        btnWeapon = btnWeaponUpgrade.GetComponent<Button>();
        btnPlayer = btnPlayerUpgrade.GetComponent<Button>();
        btnHealth = btnHealthBuy.GetComponent<Button>();

        weaponPriceText = btnWeaponUpgrade.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        weaponLevelText = btnWeaponUpgrade.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        playerPriceText = btnPlayerUpgrade.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        playerLevelText = btnPlayerUpgrade.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

        healthPriceText = btnHealthBuy.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        currentExp = playerJson.player.currentExp;
        currentLevel = playerJson.player.currentLevel;
        currentCoin = playerJson.player.currentCoin;
        currentWeaponUpgrade = playerJson.player.currentWeaponUpgrade;
        currentPlayerUpgrade = playerJson.player.currentPlayerUpgrade;
        weaponReqLevel = playerJson.player.weaponReqLevel;
        playerReqLevel = playerJson.player.playerReqLevel;
        weaponPriceMultiplier = playerJson.player.weaponPriceMultiplier;
        playerPriceMultiplier = playerJson.player.playerPriceMultiplier;
        glowPower = playerJson.player.glowPower;
        colorHexCode = playerJson.player.colorHexCode;
        expSlider.maxValue = playerJson.player.expSliderMax;
        damage = playerJson.player.damage;


        levelText.text = currentLevel.ToString();
        coinText.text = currentCoin.ToString();
        
        expSlider.value = currentExp;

        changeColor.PlayerStartColor(colorHexCode, glowPower);
    }
    public void AddExpCoin(float exp,int coin)
    {
        currentExp += exp;
        StartCoroutine(AddCoinAnim(coin));
        if (exp != 0)
        {
            ExpText.Create(transform.position, (int)exp);
        }
        if (currentExp >= expSlider.maxValue)
        {
            currentExp = currentExp - expSlider.maxValue;
            expSlider.value = currentExp;
            LevelUp();
        }
        else
        {
            expSlider.value = currentExp;
        }

        playerJson.player.currentExp = currentExp;
        playerJson.player.expSliderMax = expSlider.maxValue;
    }
    IEnumerator AddCoinAnim(int coin)
    {
        TextMeshProUGUI addedText = coinText.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        addedText.transform.gameObject.SetActive(true);
        addedText.text = "+" + coin.ToString();
        Vector3 oldPosition = coinText.transform.position - addedText.transform.position;
        for (int i = 0; i < 5; i++)
        {
            addedText.transform.position = new Vector3(addedText.transform.position.x - 0.01f, addedText.transform.position.y);
            yield return new WaitForSeconds(0.1f);
        }
        addedText.text = "";
        addedText.transform.position = coinText.transform.position - oldPosition;
        addedText.transform.gameObject.SetActive(false);
        currentCoin += coin;
        coinText.text = currentCoin.ToString();

        playerJson.player.currentCoin = currentCoin;
    }
    public void PlayerUpgrade()
    {
        if (currentPlayerUpgrade > 25 || currentLevel < playerReqLevel)
        {
            playerLevelText.color = Color.red;
            return;
        }

        nextPlayerUpgradePrice = currentPlayerUpgrade * 200 * playerPriceMultiplier;
        if (currentCoin > nextPlayerUpgradePrice)
        {
            currentPlayerUpgrade += 5;
            if (!PlayerUpgradeColor())
            {
                currentPlayerUpgrade -= 5;
                playerLevelText.color = Color.red;
                return;
            }
            playerLevelText.color = Color.white;
            currentCoin -= nextPlayerUpgradePrice;
            ExpText.Create(transform.position, -nextPlayerUpgradePrice);
            coinText.text = currentCoin.ToString();
            nextPlayerUpgradePrice = currentPlayerUpgrade * 200 * playerPriceMultiplier;
            playerPriceText.text = (nextPlayerUpgradePrice).ToString();
            playerLevelText.text = playerReqLevel.ToString();
            playerHealth.UpgradeHealth();

            if (currentLevel < playerReqLevel)
            {
                playerLevelText.color = Color.red;
                btnPlayer.interactable = false;
            }

            playerJson.player.currentCoin = currentCoin;
        }
    }
    private bool PlayerUpgradeColor()
    {

        if (currentPlayerUpgrade <= 5)
        {
            Debug.Log("1");
            playerReqLevel = 5;
            
            playerPriceMultiplier = 1;
            playerHealth.playerDefans = 10;
        }
        else if (currentPlayerUpgrade > 5 && currentPlayerUpgrade <= 10 && currentLevel >= 5)
        {
            Debug.Log("2");
            playerReqLevel = 10;
            playerPriceMultiplier = 2;
            playerHealth.playerDefans = 15;
        }
        else if (currentPlayerUpgrade > 10 && currentPlayerUpgrade <= 15 && currentLevel >= 10)
        {
            Debug.Log("3");
            playerReqLevel = 15;
            playerPriceMultiplier = 3;
            playerHealth.playerDefans = 20;
        }
        else if (currentPlayerUpgrade > 15 && currentPlayerUpgrade <= 20 && currentLevel >= 15)
        {
            Debug.Log("4");
            playerReqLevel = 20;
            playerPriceMultiplier = 4;
            playerHealth.playerDefans = 25;
        }
        else if (currentPlayerUpgrade > 20 && currentPlayerUpgrade <= 25 && currentLevel >= 20)
        {
            Debug.Log("5");
            playerReqLevel = 25;
            playerPriceMultiplier = 5;
            playerHealth.playerDefans = 30;
        }
        else
        {
            btnPlayer.interactable = false;
            if (currentPlayerUpgrade >= 25)
            {
                playerReqLevel = 100;
                btnPlayerUpgrade.SetActive(false);
            }
            Debug.LogWarning("empty");
            playerJson.player.playerReqLevel = playerReqLevel;
            return false;
        }

        playerJson.player.playerReqLevel = playerReqLevel;
        playerJson.player.playerPriceMultiplier = playerPriceMultiplier;
        playerJson.player.currentPlayerUpgrade = currentPlayerUpgrade;

        return true;
    }
    public void HealthBuy()
    {
        int healthBuyPrice = currentLevel * 200;
        if (currentCoin > healthBuyPrice && playerHealth.CheckHealthFull())
        {
            currentCoin -= healthBuyPrice;
            coinText.text = currentCoin.ToString();
            playerHealth.FillHealth();

            playerJson.player.currentCoin = currentCoin;
        }

    }
    public void WeaponUpgrade()
    {
        if (currentWeaponUpgrade > 25 || currentLevel < weaponReqLevel)
        {
            weaponLevelText.color = Color.red;
            return;
        }

        nextWeaponUpgradePrice = currentWeaponUpgrade * 200 * weaponPriceMultiplier;
        if (currentCoin > nextWeaponUpgradePrice)
        {
            currentWeaponUpgrade++;
            if (!WeaponUpgradeColor())
            {
                currentWeaponUpgrade--;
                weaponLevelText.color = Color.red;
                return;
            }
            currentCoin -= nextWeaponUpgradePrice;
            weaponLevelText.color = Color.white;
            ExpText.Create(transform.position, -nextWeaponUpgradePrice);
            coinText.text = currentCoin.ToString();
            nextWeaponUpgradePrice = currentWeaponUpgrade * 200 * weaponPriceMultiplier;
            weaponPriceText.text = (nextWeaponUpgradePrice).ToString();
            weaponLevelText.text = weaponReqLevel.ToString();

            if (currentLevel < weaponReqLevel)
            {
                weaponLevelText.color = Color.red;
                btnWeapon.interactable = false;
            }

            playerJson.player.currentCoin = currentCoin;
        }
    }
    private bool WeaponUpgradeColor()
    {
        if (currentWeaponUpgrade <= 5)
        {
            Debug.Log("1");
            colorHexCode = "FFFFFF";
            changeColor.SetWeaponColor(colorHexCode, glowPower);
            weaponReqLevel = 0;
            damage += 5;
            weaponPriceMultiplier = 1;
            glowPower++;
            if (currentWeaponUpgrade == 5)
            {
                weaponReqLevel = 5;
                glowPower = 1;
            }
        }
        else if (currentWeaponUpgrade >= 5 && currentWeaponUpgrade <= 10 && currentLevel >= 5)
        {

            Debug.Log("2");
            colorHexCode = "00DBFF";
            changeColor.SetWeaponColor(colorHexCode, glowPower);
            weaponReqLevel = 5;
            damage += 10;
            weaponPriceMultiplier = 2;
            glowPower++;
            if (currentWeaponUpgrade == 10)
            {
                weaponReqLevel = 10;
                glowPower = 1;
            }
        }
        else if (currentWeaponUpgrade >= 10 && currentWeaponUpgrade <= 15 && currentLevel >= 10)
        {
            Debug.Log("3");
            colorHexCode = "000CFF";
            changeColor.SetWeaponColor(colorHexCode, glowPower);
            weaponReqLevel = 10;
            damage += 15;
            weaponPriceMultiplier = 3;
            glowPower++;
            if (currentWeaponUpgrade == 15)
            {
                weaponReqLevel = 15;
                glowPower = 1;
            }
        }
        else if (currentWeaponUpgrade >= 15 && currentWeaponUpgrade <= 20 && currentLevel >= 15)
        {
            Debug.Log("4");
            colorHexCode = "FFF000";
            changeColor.SetWeaponColor(colorHexCode, glowPower);
            weaponReqLevel = 15;
            damage += 20;
            weaponPriceMultiplier = 4;
            glowPower++;
            if (currentWeaponUpgrade == 20)
            {
                weaponReqLevel = 20;
                glowPower = 1;
            }
        }
        else if (currentWeaponUpgrade >= 20 && currentWeaponUpgrade <= 25 && currentLevel >= 20)
        {
            Debug.Log("5");
            colorHexCode = "FFF000";
            changeColor.SetWeaponColor(colorHexCode, glowPower);
            weaponReqLevel = 20;
            damage += 25;
            weaponPriceMultiplier = 5;
            glowPower++;
            if (currentWeaponUpgrade == 25)
            {
                weaponReqLevel = 25;
                glowPower = 1;
            }
        }
        else
        {
            btnWeapon.interactable = false;
            if (currentWeaponUpgrade >= 25)
            {
                weaponReqLevel = 100;
                btnWeaponUpgrade.SetActive(false);
            }
            Debug.LogWarning("empty");
            playerJson.player.playerReqLevel = playerReqLevel;
            return false;
        }

        playerJson.player.weaponReqLevel = weaponReqLevel;
        playerJson.player.glowPower = glowPower;
        playerJson.player.weaponPriceMultiplier = weaponPriceMultiplier;
        playerJson.player.damage = damage;
        playerJson.player.currentWeaponUpgrade = currentWeaponUpgrade;
        playerJson.player.colorHexCode = colorHexCode;

        return true;
    }

    private void LevelUp()
    {
        currentLevel++;
        playerHealth.FillHealth();
        expSlider.maxValue = currentLevel * 48;
        levelText.text = currentLevel.ToString();

        playerJson.player.currentLevel = currentLevel;
        playerJson.player.expSliderMax = expSlider.maxValue;

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Apple") && playerHealth.CheckHealthFull())
        {
            playerHealth.AddHealth();
            Destroy(collision.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Trap"))
        {
            playerHealth.DieInstanlty();
        }
        if (collision.gameObject.tag == "Chest")
        {
            collision.GetComponent<ChestOpen>().OpenChest(this);
        }
        if (collision.name == "WeaponUpgrade")
        {
            if (weaponReqLevel != 100)
            {
                btnWeaponUpgrade.SetActive(true);
                btnWeapon.interactable = true;
                nextWeaponUpgradePrice = currentWeaponUpgrade * 200 * weaponPriceMultiplier;
                weaponPriceText.text = nextWeaponUpgradePrice.ToString();
                weaponLevelText.text = weaponReqLevel.ToString();
                weaponPriceText.color = Color.white;
                weaponLevelText.color = Color.white;

                if (currentLevel < weaponReqLevel)
                {
                    weaponLevelText.color = Color.red;
                    btnWeapon.interactable = false;
                }
                if (nextWeaponUpgradePrice > currentCoin)
                {
                    weaponPriceText.color = Color.red;
                    btnWeapon.interactable = false;
                }
            }

        }
        else if (collision.name == "PlayerUpgrade")
        {
            if (playerReqLevel != 100)
            {
                btnPlayerUpgrade.SetActive(true);
                btnPlayer.interactable = true;
                nextPlayerUpgradePrice = currentPlayerUpgrade * 200 * playerPriceMultiplier;
                playerPriceText.text = nextPlayerUpgradePrice.ToString();
                playerLevelText.text = playerReqLevel.ToString();
                playerPriceText.color = Color.white;
                playerLevelText.color = Color.white;
                if (currentLevel < playerReqLevel)
                {
                    playerLevelText.color = Color.red;
                    btnPlayer.interactable = false;
                }
                if (nextPlayerUpgradePrice > currentCoin)
                {
                    playerPriceText.color = Color.red;
                    btnPlayer.interactable = false;
                }
            }
        }
        else if (collision.name == "HealthBuy")
        {
            btnHealthBuy.SetActive(true);
            btnHealth.interactable = true;
            healthPriceText.text = (currentLevel * 200).ToString();
            healthPriceText.color = Color.white;
            if (!playerHealth.CheckHealthFull())
            {
                healthPriceText.color = Color.red;
                btnHealth.interactable = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "WeaponUpgrade")
        {
            btnWeaponUpgrade.SetActive(false);
        }
        else if(collision.name == "PlayerUpgrade")
        {
            btnPlayerUpgrade.SetActive(false);
        }
        else if (collision.name == "HealthBuy")
        {
            btnHealthBuy.SetActive(false);
        }
    }

}