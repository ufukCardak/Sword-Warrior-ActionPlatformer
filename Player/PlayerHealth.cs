using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class PlayerHealth : Player
{
    //[SerializeField] private PlayerScriptableObject playerScriptableObject;
    [SerializeField] Interstitial interstitial;
    [SerializeField] private JsonController playerJson;
    [SerializeField] private Slider playerHealthSlider;
    public bool isAlive = true;
    public int playerDefans = 1;
    [SerializeField] float knockback;
    [SerializeField] Transform cam;

    [SerializeField] Volume volume;
    [SerializeField] GameObject resetGameobject;
    [SerializeField] Stage stage;
    private void Awake()
    {
        Time.timeScale = 1;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerHealth = this;
    }
    private void Start()
    {
        playerHealthSlider.maxValue = playerJson.player.healthSliderMax;

        SetHealthSliderSize();
    }
    public void PlayerStun()
    {

    }
    public void PlayerHealthJsonSave()
    {
        playerJson.JsonSave();
    }
    public void PlayerReturnHomeDown()
    {
        playerJson.JsonSave();

        Vector3 returnPosition = new Vector3(-7.5f, -3.5f);
        transform.position = returnPosition;
        cam.transform.position = returnPosition + new Vector3(0,3f);
        stage.ActiveStage();
    }
    public void EnemyStunBegun()
    {
        StartCoroutine(BegunAnim());
    }
    
    IEnumerator BegunAnim()
    {
        volume.profile.TryGet(out Bloom bloom);
        bloom.threshold.value = 0.4f;
        cam.transform.position += new Vector3(0.05f, 0.05f);
        yield return new WaitForSeconds(0.2f);
        bloom.threshold.value = 0.9f;
    }
    public void AddHealth()
    {
        playerHealthSlider.value += Random.Range(10,40);
    }
    public void FillHealth()
    {
        playerHealthSlider.value = playerHealthSlider.maxValue;
    }
    void SetHealthSliderSize() 
    {
        float addedHealth = playerHealthSlider.maxValue - 100;
        if (addedHealth == 0)
        {
            return;
        }
        RectTransform rect = playerHealthSlider.GetComponent<RectTransform>();
        while (addedHealth >= 50)
        {
            playerHealthSlider.transform.localPosition = new Vector3(playerHealthSlider.transform.localPosition.x + 10, playerHealthSlider.transform.localPosition.y);
            rect.sizeDelta = new Vector2(rect.sizeDelta.x + 5, rect.sizeDelta.y);
            addedHealth -= 50;
        }
    }
    public void DieInstanlty()
    {
        TakeDamage((int)playerHealthSlider.maxValue, transform);
    }
    public void UpgradeHealth()
    {
        StartCoroutine(UpgradeHealthAnim());
    }
    IEnumerator UpgradeHealthAnim()
    {
        RectTransform rect = playerHealthSlider.GetComponent<RectTransform>();
        for (int i = 0; i < 5; i++)
        {
            playerHealthSlider.transform.localPosition = new Vector3(playerHealthSlider.transform.localPosition.x + 2, playerHealthSlider.transform.localPosition.y);
            playerHealthSlider.maxValue += 10;
            rect.sizeDelta = new Vector2(rect.sizeDelta.x + 1, rect.sizeDelta.y);
            yield return new WaitForSeconds(0.03f);
        }
        playerJson.player.healthSliderMax = playerHealthSlider.maxValue;
    }
    public void TakeDamage(int dmg,Transform enemy)
    {
        if (!isAlive)
        {
            return;
        }
        takedDmg = true;
        canGrabLedge = false;
        ledgeDetected = false;
        canClimb = false;
        canMove = true;
        isAttacking = false;

        cam.transform.position += new Vector3(0.025f, 0.025f);

        StopCoroutine(StopRigidbody());
        StartCoroutine(StopRigidbody());

        Vector2 direction = (enemy.position - transform.position).normalized;
        if (direction.x < 0)
        {
            rb.AddForce(new Vector2(0.1f, 0.25f) * knockback);
        }
        else
        {
            rb.AddForce(new Vector2(-0.1f, 0.25f) * knockback);
        }

        playerHealthSlider.value -= dmg;
        if (playerHealthSlider.value <= 0)
        {
            playerJson.JsonSave();
            isAlive = false;
            
            animator.speed = 0;
            animator.enabled = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
            playerHealthSlider.transform.gameObject.SetActive(false);
            interstitial.ShowInterstitialAd();
            playerAttack.Die();
            StartCoroutine(GetComponent<ChangeColor>().DyingAnim(1f));
            StartCoroutine(WaitDeath());
            /*Destroy(gameObject,1.5f);
            resetGameobject.SetActive(true);
            takedDmg = false;
            Time.timeScale = 0;*/
        }
    }

    public bool CheckHealthFull()
    {
        if (playerHealthSlider.value == playerHealthSlider.maxValue)
        {
            return false;
        }
        return true;
    }
    IEnumerator WaitDeath()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        resetGameobject.SetActive(true);
        takedDmg = false;
        Time.timeScale = 0;      
    }
    IEnumerator StopRigidbody()
    {
        ChangeColor changeColor = GetComponent<ChangeColor>();
        string color = changeColor.hexCodeSword;
        
        GetComponent<ChangeColor>().SetColor("000000");
        animator.speed = 0;
        animator.SetBool("CanClimb", false);
        playerAttack.enabled = false;

        yield return new WaitForSeconds(0.25f);
        rb.velocity = Vector2.zero;
        takedDmg = false;
        playerAttack.enabled = true;
        animator.SetBool("CanClimb", false);
        animator.speed = 1;
        GetComponent<ChangeColor>().SetColor("FFFFFF");
    }
}
