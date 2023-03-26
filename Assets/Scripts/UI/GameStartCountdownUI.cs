using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    private const string NUMBER_POPUP = "NumberPopup";
    [SerializeField] private TextMeshProUGUI countdownText;
    private Animator animator;
    private int previousCountdownNumber;
    private void Start() {
        Hide();

        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        animator = GetComponent<Animator>();
    }

    private void GameManager_OnStateChanged(object sender, System.EventArgs e) {
        if (GameManager.Instance.IsCountdownToStartActive()) 
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Update() {
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTimer());

        if (countdownNumber != previousCountdownNumber)
        {
            previousCountdownNumber = countdownNumber;
            countdownText.text = countdownNumber.ToString();

            animator.SetTrigger(NUMBER_POPUP);

            SoundManager.Instance.PlayCountdownSound();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
