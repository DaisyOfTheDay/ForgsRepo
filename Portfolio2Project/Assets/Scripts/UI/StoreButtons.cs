using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
//using UnityEditorInternal;
using UnityEngine;

public class StoreButtons : MonoBehaviour
{
    public AudioSource buttonAudio;
    int cost = 0;
    [SerializeField] TextMeshProUGUI[] texts;
    public enum UpgradeType 
    { 
    Atk,
    Hp,
    Spd
    }

    public void BuyAttack(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.transactionClick, AudioManager.instance.volumeScale);

        CalculateCost(amount, UpgradeType.Atk);

        if (gameManager.instance.playerStats.Exp >= cost && gameManager.instance.playerStats.GetAttack() < 100)
        {
            for (int i = 0; i < amount; ++i)
            {
                gameManager.instance.playerStats.GainExp(-((gameManager.instance.playerStats.GetAttack() + 1) * 10));
                gameManager.instance.playerStats.AttackUp(amount);
            }
            gameManager.instance.playerController.UpdatePlayerStats();
            DisplayCosts();
        }
    }
    public void BuyHealth(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.transactionClick, AudioManager.instance.volumeScale);

        CalculateCost(amount, UpgradeType.Hp);

        if (gameManager.instance.playerStats.Exp >= cost && gameManager.instance.playerStats.Health < 100)
        {
            for (int i = 0; i < amount; ++i)
            {
                gameManager.instance.playerStats.GainExp(-((gameManager.instance.playerStats.Health + 1) * 10));
                gameManager.instance.playerStats.HealthUp(1);
            }
            gameManager.instance.playerController.UpdatePlayerStats();
            gameManager.instance.playerController.UpdateHealthBar();
            DisplayCosts();
        }
    }
    public void BuySpeed(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.transactionClick, AudioManager.instance.volumeScale);

        CalculateCost(amount, UpgradeType.Spd);

        if (gameManager.instance.playerStats.Exp >= cost && gameManager.instance.playerStats.GetSpeed() < 100)
        {
            for (int i = 0; i < amount; ++i)
            {
                gameManager.instance.playerStats.GainExp(-((gameManager.instance.playerStats.GetSpeed() + 1) * 10));
                gameManager.instance.playerStats.SpeedUp(1);
            }
            gameManager.instance.playerController.UpdatePlayerStats();
            DisplayCosts();
        }
    }


    public void SellAttack(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.transactionClick, AudioManager.instance.volumeScale);

        if (gameManager.instance.playerStats.GetAttack() > 0)
        {
            for (int i = 0; i < amount; ++i)
            {
                gameManager.instance.playerStats.GainExp(((gameManager.instance.playerStats.GetAttack() + 1) * 5));
                gameManager.instance.playerStats.AttackUp(-1);
            }
            gameManager.instance.playerController.UpdatePlayerStats();
            DisplayCosts();
        }
    }
    public void SellHealth(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.transactionClick, AudioManager.instance.volumeScale);

        if (gameManager.instance.playerStats.Health > 0)
        {
            for (int i = 0; i < amount; ++i)
            {
                gameManager.instance.playerStats.GainExp(((gameManager.instance.playerStats.Health + 1) * 5));
                gameManager.instance.playerStats.HealthUp(-1);
            }
            gameManager.instance.playerController.UpdatePlayerStats();
            gameManager.instance.playerController.UpdateHealthBar();
            DisplayCosts();
        }
    }
    public void SellSpeed(int amount)
    {
        buttonAudio.PlayOneShot(AudioManager.instance.transactionClick, AudioManager.instance.volumeScale);

        if (gameManager.instance.playerStats.GetSpeed() > 0)
        {
            for (int i = 0; i < amount; ++i)
            {
                gameManager.instance.playerStats.GainExp(((int)(gameManager.instance.playerStats.GetSpeed() + 1) * 5));
                gameManager.instance.playerStats.SpeedUp(-1);
            }
            gameManager.instance.playerController.UpdatePlayerStats();
            DisplayCosts();
        }
    }


    public void Exit()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);

        gameManager.instance.UnpauseState();
        UIManager.instance.HideActiveMenu();
    }

    public void BuyScreen()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);

        UIManager.instance.buyScreen.SetActive(true);
        UIManager.instance.sellScreen.SetActive(false);
        DisplayCosts();
    }

    public void SellScreen()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);

        UIManager.instance.buyScreen.SetActive(false);
        UIManager.instance.sellScreen.SetActive(true);
    }

    public void InventoryScreen()
    {
        buttonAudio.PlayOneShot(AudioManager.instance.buttonClick, AudioManager.instance.volumeScale);

        UIManager.instance.buyScreen.SetActive(false);
        UIManager.instance.sellScreen.SetActive(false);
    }

    void CalculateCost(int amount, UpgradeType upgrade)
    {
        cost = 0;
        buttonAudio.PlayOneShot(AudioManager.instance.transactionClick, AudioManager.instance.volumeScale);
        switch (upgrade)
        {
            case UpgradeType.Atk:
        for (int i = 0; i < amount; i++)
        {
            cost += (gameManager.instance.playerStats.GetAttack() +1) * 10 + (10 * i);
                }
                break;
            case UpgradeType.Hp:
                for (int i = 0; i < amount; i++)
                {
                    cost += (gameManager.instance.playerStats.GetHealth() +1)* 10 + (10 * i);
                }
                break;
            case UpgradeType.Spd:
                for (int i = 0; i < amount; i++)
                {
                    cost += (gameManager.instance.playerStats.GetSpeed() + 1) * 10 + (10 * i);
                }
                break;
        }
    }

    public void SetAllToZero()
    {
        SellAttack(gameManager.instance.playerStats.GetAttack());
        SellHealth(gameManager.instance.playerStats.GetHealth());
        SellSpeed(gameManager.instance.playerStats.GetSpeed());
        DisplayCosts();
    }

    public int getCost(int amount, UpgradeType upgrade) { CalculateCost(amount, upgrade); return cost; }

    public void DisplayCosts()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            switch (i) 
            {
                case 0:
                    texts[i].text = "COST: " + (gameManager.instance.playerStats.GetAttack() * 10 + getCost(1, UpgradeType.Atk)).ToString();
                    break;
                case 1:
                    texts[i].text = "COST: " + (gameManager.instance.playerStats.GetAttack() * 10 + getCost(5, UpgradeType.Atk)).ToString();
                    break;
                case 2:
                    texts[i].text = "COST: " + (gameManager.instance.playerStats.GetAttack() * 10 + getCost(10, UpgradeType.Atk)).ToString();
                    break;
                case 3:
                    texts[i].text = "COST: " + (gameManager.instance.playerStats.GetHealth() * 10 + getCost(1, UpgradeType.Hp)).ToString();
                    break;
                case 4:
                    texts[i].text = "COST: " + (gameManager.instance.playerStats.GetHealth() * 10 + getCost(5, UpgradeType.Hp)).ToString();
                    break;
                case 5:
                    texts[i].text = "COST: " + (gameManager.instance.playerStats.GetHealth() * 10 + getCost(10, UpgradeType.Hp)).ToString();
                    break;
                case 6:
                    texts[i].text = "COST: " + (gameManager.instance.playerStats.GetSpeed() * 10 + getCost(1, UpgradeType.Spd)).ToString();
                    break;
                case 7:
                    texts[i].text = "COST: " + (gameManager.instance.playerStats.GetSpeed() * 10 + getCost(5, UpgradeType.Spd)).ToString();
                    break;
                case 8:
                    texts[i].text = "COST: " + (gameManager.instance.playerStats.GetSpeed() * 10 + getCost(10, UpgradeType.Spd)).ToString();
                    break;
            }
        }
    }
}
