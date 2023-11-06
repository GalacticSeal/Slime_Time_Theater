using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class TextDisplay : MonoBehaviour
{
    private char[] textPull = new char[8];
    private List<GameObject> enemyList = new List<GameObject>();

    public GameObject BatPrefab;

    private string debugText = "This is debug text. The word |bat/ should be red and spawn a bat."; //<color=red>text</color>
    private int indexNum = 0; //used to keep track of next letter to print
    private int length = 0;

    private string pullText = "";
    private string printText;

    private int indexPull = 0; //used in pulling item or enemy name when highlighted text pops up

    public TMP_Text textBox;

    public void StartDialogue(int pullNum) {
        //pullNum will be used to retrieve dialogue by index later in development
        pullText = debugText;
        length = pullText.Length;
        indexNum = 0;
    }

    public void ClearEnemies() { //WILL BE MOVED TO A SEPARATE SCRIPT LATER
        for (int i = enemyList.Count-1; i >= 0; i--) {
            Destroy(enemyList[i]);
            enemyList.RemoveAt(i);
        }
    }

    private void PrintDialogue() {
        if(pullText[indexNum] == '|') {
            indexPull = indexNum+1;
        } else if(pullText[indexNum] == '/') {
            if(pullText.Substring(indexPull,indexNum-indexPull).Equals("bat", StringComparison.OrdinalIgnoreCase)) { //https://www.delftstack.com/howto/csharp/compare-two-strings-ignoring-case-in-csharp/
                GameObject newEnemy = Instantiate(BatPrefab, new Vector3(3,0,0), Quaternion.identity);
                enemyList.Add(newEnemy);
                //Debug.Log(pullText.Substring(indexPull,indexNum-indexPull));
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

    private void Update()
    {
        if(indexNum < pullText.Length) {
            PrintDialogue();
        }
    }
}
