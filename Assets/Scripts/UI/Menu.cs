using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public int page;
    public int maxpage;
    public int maxlevel;
    
    public GameObject title;
    public GameObject menu1;
    public GameObject menu2;
    
    public SceneController sceneController;
    // Start is called before the first frame update
    void Start()
    {
        sceneController = SceneController.GetInstance();
        page = 0;
        maxlevel = sceneController.GetNumLevels();
        maxpage = (maxlevel + 5)/ 6 + 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PageLeft()
    {
        if(page>0)
            page-=1;
        UpdatePage();
    }

    public void PageRight()
    {
        if(page<maxpage)
            page+=1;
        UpdatePage();
    }

    void UpdatePage(){
        if(page==0){
            menu1.SetActive(false);
            menu2.SetActive(false);
            title.SetActive(true);
        } else {
            menu1.SetActive(true);
            menu2.SetActive(true);
            title.SetActive(false);

            int currlvl;
            if (page == 1) {
                currlvl = 1;
                menu1.transform.Find("Paper").Find("RulesPage").gameObject.SetActive(true);
                menu1.transform.Find("Paper").Find("Levels").gameObject.SetActive(false);
            } else { 
                currlvl = page*6 - 8;
                menu1.transform.Find("Paper").Find("RulesPage").gameObject.SetActive(false);
                menu1.transform.Find("Paper").Find("Levels").gameObject.SetActive(true);
                for(int i=0;i<3;i++){
                    if(currlvl+i <= maxlevel){
                        int level = currlvl + i;
                        menu1.transform.Find("Paper").Find("Levels").GetChild(i).gameObject.SetActive(true);
                        menu1.transform.Find("Paper").Find("Levels").GetChild(i).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                        menu1.transform.Find("Paper").Find("Levels").GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(
                            ()=>sceneController.LoadLevel(level - 1)
                        );
                        menu1.transform.Find("Paper").Find("Levels").GetChild(i).GetChild(0).gameObject.GetComponent<TMP_Text>().text = "Level " + (level);
                    } else {
                        menu1.transform.Find("Paper").Find("Levels").GetChild(i).gameObject.SetActive(false);
                    }
                }
                currlvl += 3;
            }

            if (page == maxpage - 1) {
                menu2.transform.Find("Paper").Find("CreditsPage").gameObject.SetActive(true);
                menu2.transform.Find("Paper").Find("Levels").gameObject.SetActive(false);
                menu2.transform.Find("Paper").Find("NextButton").gameObject.SetActive(false);
            } else {
                menu2.transform.Find("Paper").Find("CreditsPage").gameObject.SetActive(false);
                menu2.transform.Find("Paper").Find("Levels").gameObject.SetActive(true);
                menu2.transform.Find("Paper").Find("NextButton").gameObject.SetActive(true);
                for (int i = 0; i < 3; i++) {
                    if (currlvl + i <= maxlevel) {
                        int level = currlvl + i;
                        menu2.transform.Find("Paper").Find("Levels").GetChild(i).gameObject.SetActive(true);
                        menu2.transform.Find("Paper").Find("Levels").GetChild(i).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
                        menu2.transform.Find("Paper").Find("Levels").GetChild(i).gameObject.GetComponent<Button>().onClick.AddListener(
                            () => sceneController.LoadLevel(level - 1)
                        );
                        menu2.transform.Find("Paper").Find("Levels").GetChild(i).GetChild(0).gameObject.GetComponent<TMP_Text>().text = "Level " + (level);
                    } else {
                        menu2.transform.Find("Paper").Find("Levels").GetChild(i).gameObject.SetActive(false);
                    }
                }
            }

        }
    }
    
}
