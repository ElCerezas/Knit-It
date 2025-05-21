using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuestraStats : MonoBehaviour
{
    [SerializeField] private GameObject dropdownPanel1;
    [SerializeField] private GameObject dropdownPanel2;
    [SerializeField] private GameObject dropdownPanel3;

    private bool isVisible1 = false;
    private bool isVisible2 = false;
    private bool isVisible3 = false;

    void Start()
    {
        if (dropdownPanel1 != null)
            dropdownPanel1.SetActive(isVisible1);
        if (dropdownPanel2 != null)
            dropdownPanel2.SetActive(isVisible2);
        if (dropdownPanel3 != null)
            dropdownPanel3.SetActive(isVisible3);
    }

    public void ToggleDropdown1()
    {
        isVisible1 = !isVisible1;
        dropdownPanel1.SetActive(isVisible1);
        isVisible2 = false ;
        isVisible3 = false ;
        dropdownPanel2.SetActive(isVisible2);
        dropdownPanel3.SetActive(isVisible3);
    }
    public void ToggleDropdown2()
    {
        isVisible2 = !isVisible2;
        dropdownPanel2.SetActive(isVisible2);
        isVisible1 = false;
        isVisible3 = false;
        dropdownPanel1.SetActive(isVisible1);
        dropdownPanel3.SetActive(isVisible3);
    }
    public void ToggleDropdown3()
    {
        isVisible3 = !isVisible3;
        dropdownPanel3.SetActive(isVisible3);
        isVisible2 = false;
        isVisible1 = false;
        dropdownPanel2.SetActive(isVisible2);
        dropdownPanel1.SetActive(isVisible1);
    }
}
