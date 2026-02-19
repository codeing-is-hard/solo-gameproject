using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;     //싱글톤으로 구성,카드를 잠깐동안 보여주는 역할,게임의 조작 및 전반적인 수정을 해줄 곳

    private List<Card> allCards;    //리스트에 잇는 모든 카드들에 해당하는 객체

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
        yield return new WaitForSeconds(0.5f);      //0.50초 동안 대기
        FlipAllCards();     //대기 끝나면 실행,처음 뒤집어서 보여주기
        yield return new WaitForSeconds(3f);        //3초 대기후에
        FlipAllCards();     //뒤집어서 가리기,

        //카드를 뒤집는데 약간의 시간이 필요함으로 지연시간0.5초를 추가(doscale에 의해서 0.2초에 한번 뒤집히고,0.2초에한번 뒤집어짐)
        yield return new WaitForSeconds(0.5f);
        //한번 실행된 뒤에는 코루틴이 자동으로 멈춤.


    }


    void FlipAllCards() //한번에 카드를 뒤집을 메소드
    {
        foreach(Card card in allCards)
        {
            card.FlipCard();        //순차적으로 1번에 뒤집힐것.
        }
    }
}
