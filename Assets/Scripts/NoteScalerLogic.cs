using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScalerLogic : MonoBehaviour
{
    float topHeight = 2.36f;
    float botHeight = -2.70f;
    float finalScale = 0.8f;
    float startScale = 0.3f;
    float exponent = 2f;
    void Update()
    {
        float y = transform.position.y;

        // Porcentaje invertido: 0 = arriba, 1 = abajo
        float linearPercent = Mathf.InverseLerp(topHeight, botHeight, y);

        // Aplicar curva exponencial
        float curvedPercent = Mathf.Pow(linearPercent, exponent);

        // Interpolar entre escalas
        float scale = Mathf.Lerp(startScale, finalScale, curvedPercent);

        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
