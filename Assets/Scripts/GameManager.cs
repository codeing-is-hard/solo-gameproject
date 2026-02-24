using System.Collections;
using System.Collections.Generic;
using TMPro;        //ÅÃ½ºÆ®ÆÄÀÏ ¼öÁ¤ÇÏ·Á¸é Àû¾î¾ßÇÔ.
using UnityEngine;
using UnityEngine.UI;           //½Ç¸°´õ¾µ°Å¸éÀû¾î¾ßÇÔ
using UnityEngine.SceneManagement;      //Àç½ÃÀÛ ¹öÆ°À» ´©¸£¸é ¾ÀÀ» ´Ù½Ã ºÒ·¯¿À±â À§ÇÔ

public class GameManager : MonoBehaviour
{

    public static GameManager instance;     //½Ì±ÛÅæÀ¸·Î ±¸¼º,Ä«µå¸¦ Àá±ñµ¿¾È º¸¿©ÁÖ´Â ¿ªÇÒ,°ÔÀÓÀÇ Á¶ÀÛ ¹× Àü¹İÀûÀÎ ¼öÁ¤À» ÇØÁÙ °÷

    private List<Card> allCards;    //¸®½ºÆ®¿¡ ÀÕ´Â ¸ğµç Ä«µåµé¿¡ ÇØ´çÇÏ´Â °´Ã¼

    private Card flippedCard;       //Ä«µå°¡ ¼­·Î °°Àº Â¦ÀÌ µÚÁıÇô º´ÂÁö È®ÀÎÇÒ °´Ã¼

    private bool isFlipping = false;        //ÇöÀç Ä«µå°¡ µÚÁıÇôÁö°í ÀÖ´ÂÁö È®ÀÎ,ÇöÀç´Â ¾Æ´Ï¹Ç·Î °ÅÁşÀ¸·Î ±âº» ¼³Á¤

    [SerializeField] private TextMeshProUGUI timeoutText;       //Á¦ÇÑ½Ã°£À» º¸¿©ÁÙ ÅÃ½ºÆ®

    [SerializeField] private TextMeshProUGUI gameOverText;      //°ÔÀÓÀ» Å¬¸®¾î ¶Ç´Â ½ÇÆĞ½Ã È­¸é¿¡ º¸¿©ÁÙ ÅÃ½ºÆ®


    [SerializeField] private GameObject gameOverPanel;      //°ÔÀÓÀ» Å¬¸®¾îÇß°Å³ª ½ÇÆĞÇßÀ»¶§ º¸¿©ÁÙ ÆĞ³Î

    private bool isGameOver=false;      //Ã³À½¿¡´Â °ÔÀÓÁøÇàÁßÀÏ °ÍÀÌ±â ¶§¹®¿¡ °ÅÁş

    [SerializeField] private Slider timeoutSlider;      //³²Àº ½Ã°£À» ³ªÅ¸³¾ ½Ç¸°´õ º¯¼ö
    [SerializeField] private float timeLimit = 60f;      //½Ã°£ÃÖ´ë°ª 60ÃÊ

    private float currentTime;      //ÇöÀç ³²¾ÆÀÖ´Â ½Ã°£À» ³ªÅ¸³¾ º¯¼ö

    private int totalMatches = 10;      //20ÀåÀÇ Ä«µåÁß ¿Ã¹Ù¸¥ Â¦À» Ã£À» ¼ö ÀÖ´Â ÃÖ´ë¼ıÀÚ´Â 10¹øÀÌ¹Ç·Î ÃÊ±â°ª10ÁöÁ¤
    private int matchesFound = 0;       //¸îÀåÀÇ Ä«µå¸¦ Ã£¾Ñ´ÂÁö¸¦ ¾Ë¾Æº¼ º¯¼ö ÃÊ±â°ª0Àå

    void Awake()
    {
        if(instance == null)
        {
            instance = this;        //°ªÀÌ ¾ø´Ù¸é Áö±İ °ÔÀÓ¸Å´ÏÀú°¡ °¡Áö°í ÀÖ´Â ÀÎ½ºÅÏ½º °ªÀ¸·Î ´ëÃ¼
        }
    }

    void Start()
    {
        Board board = FindObjectOfType<Board>();        //Ã£À¸·Á°íÇÏ´Â ¿ÀºêÁ§Æ®Å¸ÀÔÀ» Àû´Â´Ù.
        allCards = board.GetCards();

        currentTime = timeLimit;        //Ã³À½½Ã°£À» 60ÃÊ·Î ¼³Á¤

        SetCurrentTimeText();       //ÅÃ½ºÆ®ÀÇ ÇöÀç½Ã°£À» ¼³Á¤ÇÏ´Â¸Ş¼Òµå

        StartCoroutine("FlipAllCardsRoutine");       //ÄÚ·çÆ¾ÀÌº¥Æ®½ÃÀÛ,ÄÚ·çÆ¾À» ¹®ÀÚ¿­·Î È£ÃâÇØ¾ß¸¸ ¸ØÃâ ¼ö ÀÖ´Ù.
                                                     //Áö±İÀº ½ÇÇàÇÏ°í ÀÎÅÍÆäÀÌ½º°¡ ÁøÇàµÇ¸é ÀÚµ¿À¸·Î ¸ØÃã(¹İº¹À» À§ÇØ¼± ÄÚ·çÆ¾ ³»¿¡ ¹İº¹¹® ¼³Á¤->yield¸®ÅÏÀ» ¹İº¹¹® ³»¿¡ ÀÛ¼ºÇÏ¸é °¡´É)
                                                     

    }

    void SetCurrentTimeText()
    {
        int timeSec = Mathf.CeilToInt(currentTime);                    //ÃÊ ´ÜÀ§ ½Ã°£À¸·Î º¯È¯,¿Ã¸²¼ö·Î ¹İ¿Ã¸²ÇØÁÖ´Â(CeilToInt)
        timeoutText.SetText(timeSec.ToString());                        //¿Ã¸²¼ö·Î ¹İ¿Ã¸²ÇÑ ½Ã°£À» ¹®ÀÚ¿­·Î ¹Ù²ã¼­ Å¸ÀÓ¾Æ¿ô ÅÃ½ºÆ®ÀÇ ÅÃ½ºÆ®·Î ¼³Á¤ÇÑ´Ù.
    }




    IEnumerator FlipAllCardsRoutine()
    {
        isFlipping = true;      //¼öÇàµÇ±â ÀüÀÌ¹Ç·Î true¼³Á¤

        yield return new WaitForSeconds(0.5f);      //0.50ÃÊ µ¿¾È ´ë±â
        FlipAllCards();     //´ë±â ³¡³ª¸é ½ÇÇà,Ã³À½ µÚÁı¾î¼­ º¸¿©ÁÖ±â
        yield return new WaitForSeconds(3f);        //3ÃÊ ´ë±âÈÄ¿¡
        FlipAllCards();     //µÚÁı¾î¼­ °¡¸®±â,

        //Ä«µå¸¦ µÚÁı´Âµ¥ ¾à°£ÀÇ ½Ã°£ÀÌ ÇÊ¿äÇÔÀ¸·Î Áö¿¬½Ã°£0.5ÃÊ¸¦ Ãß°¡(doscale¿¡ ÀÇÇØ¼­ 0.2ÃÊ¿¡ ÇÑ¹ø µÚÁıÈ÷°í,0.2ÃÊ¿¡ÇÑ¹ø µÚÁı¾îÁü)
        yield return new WaitForSeconds(0.5f);
        //ÇÑ¹ø ½ÇÇàµÈ µÚ¿¡´Â ÄÚ·çÆ¾ÀÌ ÀÚµ¿À¸·Î ¸ØÃã.

        isFlipping = false;     //¼öÇàµÇ°í ³ª¸é µÚÁı°í ÀÕ´Â »óÅÂ°¡ ¾Æ´Ï¹Ç·Î ±âº»°ª ¼³Á¤

        yield return StartCoroutine("CountDownTimerRoutine");      //ÄÚ·çÆ¾ ¾È¿¡¼­ »õ·Î¿î ÄÚ·çÆ¾À» ½ÃÀÛÇÏ°í ½ÍÀ» ¶§ ¹İº¹¹® ´ë½Å »ç¿ë°¡´É
    }

    IEnumerator CountDownTimerRoutine()     //ÃÖ´ë½Ã°£¿¡¼­ÇöÀç
    {
        while (currentTime > 0)     //ÇöÀç½Ã°£ÀÌ0ÃÊº¸´ÙÅ¬¶§
        {
            currentTime -= Time.deltaTime;      //¼­·Î ´Ù¸¥ È¯°æ¿¡¼­µµ µ¿ÀÏÇÑ ½Ã°£À» »©±â À§ÇØ ÇÁ·¹ÀÓ º¸Á¤ÇØ¾ßÇÔ
            timeoutSlider.value = currentTime / timeLimit;          //½Ç¸°´õÀÇ °ª=ÇöÀç ½Ã°£À» ÃÖ´ë ½Ã°£À¸·Î ³ª´« °ª

            SetCurrentTimeText();       //ÅÃ½ºÆ® ½Ã°£¾÷µ¥ÀÌÆ®

            yield return null;      //Áö¿¬¾øÀÌ ¹Ù·Î ´ÙÀ½ ÄÚ·çÆ¾À¸·Î ÁøÇà
        }

        GameOver(false);

    }



    void FlipAllCards()         //ÇÑ¹ø¿¡ Ä«µå¸¦ µÚÁıÀ» ¸Ş¼Òµå
    {
        foreach(Card card in allCards)
        {
            card.FlipCard();        //¼øÂ÷ÀûÀ¸·Î 1¹ø¾¿ µÚÁıÈú°Í.
        }
    }

    public void CardClicked(Card card)      //Ä«µå°¡ Â¦ÀÌ ¸Â°Ô Å¬¸¯µÇ¾ú´ÂÁö È®ÀÎÇÒ ¸Ş¼Òµå
    {
        if (isFlipping || isGameOver)     //µÚÁıÇôÁö°í ÀÖ´Â µ¿¾È¿¡´Â Å¬¸¯ ¹«½ÃÇÏ°Å³ª °ÔÀÓÀÌ ³¡³´À» ¶§¿¡´Â Å¬¸¯ ¸øÇÏµµ·Ï
        {
            return;
        }




        card.FlipCard();

        if (flippedCard == null)            //ÇöÀç µÚÁıÈù Ä«µå°¡ ÇÏ³ªµµ ¾øÀ¸¸é
        {
           flippedCard= card;       //ÇöÀç Ä«µå¸¦ ³Ö±â
        }

        else
        {
            StartCoroutine(CheckMatchRoutine(flippedCard,card));       //2°³ÀÇ Àü´Ş°ª(flippedCard,card)À» º¸³»±â À§ÇØ ""¸¦ ¾µ ¼ö ¾ø´Ù.
        }

    }

    IEnumerator CheckMatchRoutine(Card card1, Card card2)       //2°³ÀÇ ÀÎÀÚ°ªÀ» °¡Áö¹Ç·Î
    {
        isFlipping = true;      //Ã³À½¿£ µÚÁı°í ÀÖ´Â ÁßÀÎ »óÅÂ

        if(card1.cardID == card2.cardID)        //Ä«µåÀÇ id°¡°°À¸¸é °°ÀºÄ«µå·Î Ãë±ŞÇÔ
        {
            card1.SetMatched();
            card2.SetMatched();

            matchesFound++;         //Â¦ÀÌ ¸ÂÀ¸¸é ¸ÂÀº¸¸Å­ ¿Ã¹Ù¸¥Â¦À» ¸Â­Ÿ´Ù´ÂÀÇ¹Ì·Î 1°³¾¿´Ã·ÁÁÖ°í

            Debug.Log("°°Àº Ä«µåÀÔ´Ï´Ù");

            if (matchesFound == totalMatches)
            {
                GameOver(true);     //Å¬¸®¾î Çß´Ù°í Ç¥½ÃÇØÁÖ±â
            }

                  
        }
        else
        {
            Debug.Log("´Ù¸¥ Ä«µåÀÔ´Ï´Ù,´Ù½Ã ½ÃµµÇØÁÖ¼¼¿ä.");

            yield return new WaitForSeconds(1f);        //´Ù¸£¸é 1ÃÊ±â´Ù¸°µÚ¿¡ Ä«µå2ÀåÀ» ´Ù½ÃµÚÁı¾î¼­ ¿ø»óº¹±¸ÇÑ´Ù.

            card1.FlipCard();
            card2.FlipCard();

            yield return new WaitForSeconds(0.5f);      //µÚÁı´Â ¿©À¯ ½Ã°£ È®º¸0.5ÃÊ
        }


        isFlipping=false;       //Á¶°Ç¹®ÀÌ¼öÇàµÈµÚ¿¡´Â µÚÁı´ÂÁßÀÌ¾Æ´Ïµµ·Ï¼³Á¤

        flippedCard= null;      // ÃÊ±âÈ­ÇÏÁö ¾ÊÀ¸¸é ÇÑÀå¸¸ µÚÁı¾îµµ ÄÚ·çÆ¾À¸·Î ¹Ù·Î ³Ñ¾î°¥ ¼ö ÀÖ¾î¼­ ÃÊ±âÈ­ÇØ¾ßÇÔ.

        
    }


    void GameOver(bool success)     //¿©±â¿¡ µµ´ŞÇŞ´Ù´Â°Ç ÀÌ¹Ì ¼º°øÇß°Å³ª ½ÇÆĞÇßÀ½À» ÀÇ¹ÌÇÔ
    {

        if(!isGameOver)
        {
            isGameOver = true;      //Áßº¹ÇØ¼­ °ÔÀÓ¿À¹ö°¡ µÇ¸é ¾ÈµÇ±â ¶§¹®¿¡ ÇöÀç °ÔÀÓ¿À¹ö»óÅÂ°¡ ¾Æ´Ò ¶§¿¡¸¸ µÇµµ·Ï ¼³Á¤
            StopCoroutine("CountDownTimerRoutine");     //½Ã°£ÀÌ Èå¸£´Â°É ¸ØÃçÁÖ°í

            if (success)
            {
                gameOverText.SetText("Game clear!!");      //¼º°øÇßÀ»½Ã ÆĞ³Î¿¡ º¸¿©ÁÙ °Í,ÇÑ±Û¾²·Á¸é tmpÀÛ¾÷ÇØ¾ßµÇ¼­ Àç¼öÁ¤
            }
            else
            {
                gameOverText.SetText("Game Over..");
            }

            Invoke("ShowGameOverPanel", 2f);                //Áö¿¬½ÃÅ°´Â ¹æ¹ıÁß ÄÚ·çÆ¾¸»°íµµ ¾µ ¼ö ÀÕ´Â ¹æ¹ı,(¸Ş¼Òµå¸í,Áö¿¬½ÃÅ³½Ã°£:2.0ÃÊ)¼øÀ¸·Î ÀÛ¼ºÇÑ´Ù

        }

    }

    void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);          //ÆĞÅÏ º¸¿©ÁÖ±â
    }

    public void Restart()
    {
        SceneManager.LoadScene("SampleScene");      // Àç½ÃÀÛ ¹öÆ°À» ´©¸£¸é ¾ÀÀ» ´Ù½Ã ·ÎµåÇØ¿À±â
    }


}
