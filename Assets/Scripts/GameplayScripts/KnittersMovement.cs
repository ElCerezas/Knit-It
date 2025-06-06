using System.Collections;
using UnityEngine;

public class KnittersMovement : MonoBehaviour
{
    [SerializeField] private float moveDuration = 0.6f;
    [SerializeField] private float centerX = 2.3f;
    [SerializeField] private float pos1, pos2, pos3, pos4;

    private Coroutine currentMoveCoroutine;
    private Coroutine returnToCenterCoroutine;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
            MoveToPosition(pos1);
        else if (Input.GetKeyDown(KeyCode.F))
            MoveToPosition(pos2);
        else if (Input.GetKeyDown(KeyCode.J))
            MoveToPosition(pos3);
        else if (Input.GetKeyDown(KeyCode.K))
            MoveToPosition(pos4);
    }

    private void MoveToPosition(float targetX)
    {
        // Si se estaba yendo al centro, detenerlo
        if (returnToCenterCoroutine != null)
        {
            StopCoroutine(returnToCenterCoroutine);
            returnToCenterCoroutine = null;
        }

        // Si ya hay un movimiento en curso, detenerlo
        if (currentMoveCoroutine != null)
            StopCoroutine(currentMoveCoroutine);

        currentMoveCoroutine = StartCoroutine(MoveRoutine(targetX));
    }

    private IEnumerator MoveRoutine(float targetX)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(targetX, startPos.y, startPos.z);
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.position = endPos;
        currentMoveCoroutine = null;

        // Si fue un movimiento a una tecla, iniciar retorno al centro
        returnToCenterCoroutine = StartCoroutine(MoveRoutine(centerX));
    }
}
