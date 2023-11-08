using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    private char[] textPull = new char[8];

    private string debugText = "This is debug text. The word |bat/ should be red and spawn a bat."; //<color=red>text</color>
    private int indexNum = 0; //used to keep track of next letter to print
    private int length = 0;

    private string pullText = "";
    private string printText;
    private bool framePause = false; //used to pause text scroll by one frame

    private int indexPull = 0; //used in pulling item or enemy name when highlighted text pops up

    public TMP_Text textBox;
    public EnemySpawner mobs;

    public void StartDialogue(int pullNum) {
        //pullNum will be used to retrieve dialogue by index later in development
        pullText = debugText;
        length = pullText.Length;
        indexNum = 0;
        mobs.ClearEnemies();
    }

    private void PrintDialogue() {
        if(pullText[indexNum] == '|') {
            indexPull = indexNum+1;
        } else if(pullText[indexNum] == '/') {
            if(!mobs.SpawnEnemy(new Vector3(3,0,0), pullText.Substring(indexPull,indexNum-indexPull))) {
                Debug.Log("Mob \""+pullText.Substring(indexPull,indexNum-indexPull)+"\" could not be pulled from memory.");
            }
        }

        //process split for coherence and to ensure sequential execution - no workaround I know of for creating new Strings repeatedly to process this data
        printText = pullText.Substring(0,indexNum+1);
        printText = printText.Replace("|", "<color=red><b>");
        printText = printText.Replace("/", "</b></color>");

        textBox.text = printText;
        indexNum++;
    }

    private void Start() {
        StartDialogue(0);
    }

    private void FixedUpdate()
    {
        if(indexNum < pullText.Length) {
            if(!framePause) {
                PrintDialogue();
            }
            framePause = !framePause;
        } else {
            framePause = false;
        }
    }
}
