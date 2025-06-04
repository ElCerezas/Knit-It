using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationIdleBreak : MonoBehaviour
{
    [Header("Configuración")]
    public int maxBreaks = 5;
    public int triggerChance = 3;

    private Animator animator;
    [SerializeField] float idleDuration = 1f;
    private bool isInBreak = false;

    private HashSet<string> existingBreaks = new HashSet<string>();

    void Start()
    {
        animator = GetComponent<Animator>();

        // Obtener duración del clip Idle
        var clips = animator.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (clip.name == "Idle")
            {
                idleDuration = clip.length;
                break;
            }
        }

        // Detectar qué triggers BreakX existen en el Animator
        for (int i = 1; i <= maxBreaks; i++)
        {
            string triggerName = $"Break{i}";
            if (HasTrigger(triggerName))
            {
                existingBreaks.Add(triggerName);
            }
        }

        StartCoroutine(IdleLoopChecker());
    }

    bool HasTrigger(string paramName)
    {
        foreach (var param in animator.parameters)
        {
            if (param.name == paramName && param.type == AnimatorControllerParameterType.Trigger)
                return true;
        }
        return false;
    }

    IEnumerator IdleLoopChecker()
    {
        while (true)
        {
            yield return new WaitForSeconds(idleDuration);

            if (!isInBreak && Random.Range(0, triggerChance) == 0)
            {
                string selectedBreak = GetRandomBreakTrigger();
                if (!string.IsNullOrEmpty(selectedBreak))
                {
                    isInBreak = true;
                    animator.SetTrigger(selectedBreak);

                    // Esperar a que vuelva al estado Idle
                    yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
                    isInBreak = false;
                }
            }
        }
    }

    string GetRandomBreakTrigger()
    {
        if (existingBreaks.Count == 0)
            return null;

        int index = Random.Range(0, existingBreaks.Count);
        foreach (string name in existingBreaks)
        {
            if (index-- == 0)
                return name;
        }

        return null;
    }
}
