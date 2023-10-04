using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonsAssaginment : Player
{
    [SerializeField] Interstitial interstitial;
    private void Update()
    {
        Application.targetFrameRate = 60;
    }

    public void ShowAd()
    {
        interstitial.ShowInterstitialAd();
    }
    public void ResetScene()
    {
        playerHealth.PlayerHealthJsonSave();
        SceneManager.LoadScene(0);
    }
    public void ReturnHome()
    {
        playerHealth.PlayerReturnHomeDown();
    }
    public void BlockDown()
    {
        playerAttack.PlayerBlockDown();
    }
    public void BlockUp()
    {
        playerAttack.PlayerBlockUp();
    }
    public void AttackDown()
    {
        playerAttack.PlayerAttackDown();
    }
    public void AttackUp()
    {
        playerAttack.PlayerAttackUp();
    }
    public void MoveDownLeft()
    {
        playerMovement.PlayerMoveDownLeft();
    }
    public void MoveDownRight()
    {
        playerMovement.PlayerMoveDownRight();
    }
    public void MoveUp()
    {
        playerMovement.PlayerMoveUp();
    }
    public void JumpUp()
    {
        playerMovement.PlayerJumpUp();
    }
    public void JumpDown()
    {
        playerMovement.PlayerJumpDown();
    }
    public void DashDown()
    {
        playerDash.PlayerDownDash();
    }
    public void CoinWeaponUpgradeDown()
    {
        playerExp.WeaponUpgrade();
    }
    public void CoinPlayerUpgradeDown()
    {
        playerExp.PlayerUpgrade();
    }
}
