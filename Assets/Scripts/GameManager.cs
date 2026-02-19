using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;     //싱글톤으로 구성,카드를 잠깐동안 보여주는 역할,게임의 조작 및 전반적인 수정을 해줄 곳

    private List<Card> allCards;    //리스트에 잇는 모든 카드들에 해당하는 객체

    private Card flippedCard;       //카드가 서로 같은 짝이 뒤집혀졋는지 확인할 객체

    private bool isFlipping = false;        //현재 카드가 뒤집혀지고 있는지 확인,현재는 아니므로 거짓으로 기본 설정




    void Awake()
    {
        if(instance == null)
        {
            instance = this;        //값이 없다면 지금 게임매니저가 가지고 있는 인스턴스 값으로 대체
        }
    }

    void Start()
    {
        Board board = FindObjectOfType<Board>();        //찾으려고하는 오브젝트타입을 적는다.
        allCards = board.GetCards();

        StartCoroutine("FlipAllCardsRoutine");       //코루틴이벤트시작,코루틴을 문자열로 호출해야만 멈출 수 있다.
                                                     //지금은 실행하고 인터페이스가 진행되면 자동으로 멈춤(반복을 위해선 코루틴 내에 반복문 설정->yield리턴을 반복문 내에 작성하면 가능)
                                                     

    }

    IEnumerator FlipAllCardsRoutine()
    {
        isFlipping = true;      //수행되기 전이므로 true설정

        yield return new WaitForSeconds(0.5f);      //0.50초 동안 대기
        FlipAllCards();     //대기 끝나면 실행,처음 뒤집어서 보여주기
        yield return new WaitForSeconds(3f);        //3초 대기후에
        FlipAllCards();     //뒤집어서 가리기,

        //카드를 뒤집는데 약간의 시간이 필요함으로 지연시간0.5초를 추가(doscale에 의해서 0.2초에 한번 뒤집히고,0.2초에한번 뒤집어짐)
        yield return new WaitForSeconds(0.5f);
        //한번 실행된 뒤에는 코루틴이 자동으로 멈춤.

        isFlipping = false;     //수행되고 나면 뒤집고 잇는 상태가 아니므로 기본값 설정
    }


    void FlipAllCards() //한번에 카드를 뒤집을 메소드
    {
        foreach(Card card in allCards)
        {
            card.FlipCard();        //순차적으로 1번에 뒤집힐것.
        }
    }

    public void CardClicked(Card card)      //카드가 짝이 맞게 클릭되었는지 확인할 메소드
    {
        if (isFlipping == true)     //뒤집혀지고 있는 동안에는 클릭 무시
        {
            return;
        }




        card.FlipCard();

        if (flippedCard == null)            //현재 뒤집힌 카드가 하나도 없으면
        {
           flippedCard= card;       //현재 카드를 넣기
        }

        else
        {
            StartCoroutine(CheckMatchRoutine(flippedCard,card));       //2개의 전달값(flippedCard,card)을 보내기 위해 ""를 쓸 수 없다.
        }

    }

    IEnumerator CheckMatchRoutine(Card card1, Card card2)       //2개의 인자값을 가지므로
    {
        if(card1.cardID == card2.cardID)        //카드의 id가같으면 같은카드로 취급함
        {
            card1.SetMatched();
            card2.SetMatched();
            Debug.Log("같은 카드입니다");      
        }
        else
        {
            Debug.Log("다른 카드입니다,다시 시도해주세요.");

            yield return new WaitForSeconds(1f);        //다르면 1초기다린뒤에 카드2장을 다시뒤집어서 원상복구한다.

            card1.FlipCard();
            card2.FlipCard();

            yield return new WaitForSeconds(0.5f);      //뒤집는 여유 시간 확보0.5초
        }

        flippedCard= null;      // 초기화하지 않으면 한장만 뒤집어도 코루틴으로 바로 넘어갈 수 있어서 초기화해야함.
    }



}
