using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Tutorial;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject statusPanel;
    [SerializeField] private GameObject starScore;
    [SerializeField] private Animator[] shineAnimator;
    [SerializeField] private Vector2 finalStarPos;
    [SerializeField] private float timeToFinalPos;
    [SerializeField] private float finalScale;
    private SequenceController controller;

    private void Start()
    {
        if (statusPanel)
            statusPanel.SetActive(false);
        SetUpStarScore();
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void RestartCurrentScene()
    {
        SceneManager.LoadSceneAsync(gameObject.scene.name);
    }

    public void ShowLevelStatus()
    {
        if (statusPanel)
            statusPanel.SetActive(true);
        controller.ManualStart();
    }

    private void SetUpStarScore()
    {
        controller = starScore.GetComponent<SequenceController>();
        if (!controller) Debug.LogWarning("No Controller Component");

        controller.AddSequence(new CustomSequence(controller, (sequence, o) =>
        {
            var rect = o.GetComponent<RectTransform>();

            foreach (Animator a in shineAnimator)
            {
                if (a.isActiveAndEnabled)
                    a.SetBool("Game Is Done", true);
            }

            StartCoroutine(_StarMove(sequence, rect, finalStarPos));
        }));
    }

    private IEnumerator _StarMove(CustomSequence sequence, RectTransform transform, Vector2 final)
    {
        var velocity = Vector2.zero;
        var scaleVelocity = Vector3.zero;
        while (true)
        {
            transform.anchoredPosition = Vector2.SmoothDamp(transform.anchoredPosition, final, ref velocity, timeToFinalPos);
            transform.localScale = Vector3.SmoothDamp(transform.localScale, new Vector3(finalScale, finalScale, finalScale), ref scaleVelocity, timeToFinalPos);

            if (Vector2.Distance(transform.anchoredPosition, final) <= 0.2f)
            {
                foreach (Animator a in shineAnimator)
                {
                    if (a.isActiveAndEnabled)
                        a.SetTrigger("Move Is Done");
                }

                sequence.SetStatus(true);
                yield break;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}
