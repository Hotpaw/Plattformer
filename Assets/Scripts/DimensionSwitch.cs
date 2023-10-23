using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DimensionSwitch : MonoBehaviour
{
    [SerializeField] private GameObject[] dimensions;

    private int currentDimension = 0;
    private float time;

    private void Update()
    {
        time += Time.deltaTime;
        if (time > 3f)
        {
            SwitchDimension();
            time = 0f;
        }

        //if (Gamepad.current.leftShoulder.wasPressedThisFrame)
        //    SwitchDimension();
    }

    public void SwitchDimension()
    {
        currentDimension++;
        if (currentDimension > dimensions.Length-1)
            currentDimension = 0;

        dimensions[currentDimension].SetActive(true);

        for (int i = 0; i < dimensions.Length; i++)
            if(i != currentDimension) dimensions[i].SetActive(false);
    }
}
