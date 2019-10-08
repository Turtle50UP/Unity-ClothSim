using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public SimManager sm;
    public MeshSpawner ms;
    public float epsilon;
    const string arrow = "<=";

    public Text uiText;
    public Text controlsText;
    bool showText;

    // Start is called before the first frame update
    void Start()
    {
        showText = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            showText = !showText;
            if (showText)
            {
                uiText.canvasRenderer.SetAlpha(1);
                controlsText.canvasRenderer.SetAlpha(1);
            }
            else
            {
                uiText.canvasRenderer.SetAlpha(0);
                controlsText.canvasRenderer.SetAlpha(0);
            }
        }
        if (showText)
        {
            MakeText();
        }
    }

    void MakeText()
    {
        uiText.text = sm.isEulerian ? "Eulerian Cloth (Spring Mass [Semi Implicit Optimal])\n" : "Lagrangian Cloth (Constraint Based [Verlet Optimal])\n";
        switch (sm.selectedIntegrator)
        {
            case 0:
                uiText.text += "Forward Euler Integration\n";
                break;
            case 1:
                uiText.text += "Semi-Implicit Euler Integration\n";
                break;
            case 2:
                uiText.text += "Verlet Integration\n";
                break;
            case 3:
                uiText.text += "Leapfrog Integration\n";
                break;
            default:
                uiText.text += "WTF\n";
                break;
        }
        uiText.text += "Total Vertices = " + (ms.dimNX * ms.dimNY).ToString() + " (" + ms.dimNX.ToString() + ")\n";
        uiText.text += "Node Mass = " + sm.mass.ToString() + (sm.selectedQuality == 0 ? " " + arrow : "") + "\n";
        uiText.text += "Timestep = " + sm.dt.ToString() + " Seconds per Frame" + (sm.selectedQuality == 7 ? " " + arrow : "") + "\n";
        uiText.text += "Gravity = " + sm.g.ToString() + (sm.selectedQuality == 8 ? " " + arrow : "") + "\n";
        uiText.text += "Kinetic Friction Coefficient = " + sm.kmu.ToString() + (sm.selectedQuality == 9 ? " " + arrow : "") + "\n";
        uiText.text += "Static Friction Coefficient = " + sm.smu.ToString() + (sm.selectedQuality == 10 ? " " + arrow : "") + "\n";
        uiText.text += "---------- Eulerian Cloth Constants ----------\n";
        uiText.text += "Structure K1 = " + sm.structurek1.ToString() + (sm.selectedQuality == 1 ? " " + arrow : "") + "\n";
        uiText.text += "Structure K2 = " + sm.structurek2.ToString() + (sm.selectedQuality == 2 ? " " + arrow : "") + "\n";
        uiText.text += "Shear K1 = " + sm.sheark1.ToString() + (sm.selectedQuality == 3 ? " " + arrow : "") + "\n";
        uiText.text += "Shear K2 = " + sm.sheark2.ToString() + (sm.selectedQuality == 4 ? " " + arrow : "") + "\n";
        uiText.text += "Bend K1 = " + sm.bendk1.ToString() + (sm.selectedQuality == 5 ? " " + arrow : "") + "\n";
        uiText.text += "Bend K2 = " + sm.bendk2.ToString() + (sm.selectedQuality == 6 ? " " + arrow : "") + "\n";
        uiText.text += "---------- Lagrangian Cloth Constants ----------\n";
        uiText.text += "Number of Constraint Loops = " + sm.simLoops.ToString() + (sm.selectedQuality == 11 ? " " + arrow : "") + "\n";
        uiText.text += "Weight of Constraint = " + sm.weightBound.ToString() + (sm.selectedQuality == 12 ? " " + arrow : "") + "\n";
    }
}
