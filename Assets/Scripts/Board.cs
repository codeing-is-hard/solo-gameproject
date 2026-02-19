using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    [SerializeField] private GameObject cardPrefab;     //보드에 게임오브젝트 카드프리팹을 나중에 적용할것.

    [SerializeField] private Sprite[] cardSprites;      //사진 속 동물들 담아둘 배열

    private List<int> cardIDList= new List<int>();

    private List<Card> cardList= new List<Card>();

    void Start()
    {
        GenerateCardID();           //카드ID를 만들어서 1쌍으로 출력되도록 호출
        ShuffleCardID();            //카드섞는메소드
        InitBoard();
    }

    void ShuffleCardID()
    {
        int cardCount = cardIDList.Count;
        for (int i = 0; i < cardCount; i++)
        {
            int randomIndex = Random.Range(i, cardCount);   //랜덤한 값을 뽑는데 i범위~cardcount의 범위 내에서 뽑음    
            int temp=cardIDList[randomIndex];
            cardIDList[randomIndex] = cardIDList[i];        //카드id리스트의랜덤한값을 카드아이디리스트의 i번째로교체.
            cardIDList[i] = temp;
            //스왑 하는 과정,랜덤한 값을 뽑은걸 i번째의 숫자하고 바꾸는 과정,랜덤한 동물카드가 게임뷰에서 표시하기 위함..

            
        }
    }





    void GenerateCardID()       //카드에들어갈 ID를지정하는메소드.
    {
        for (int i = 0; i < cardSprites.Length; i++)        //사진10개를 가로 세로 형태로 넣을꺼니까 0~9까지 반복..
                                                            //(0,0)~(9,9)까지.
        {
            cardIDList.Add(i);
            cardIDList.Add(i);
        }
    }





    void InitBoard()
    {
        float spaceY = 1.8f;        //카드y의간격

        float spaceX = 1.3f;           //카드 x의 간격

        int rowCount = 5;       //세로길이
        int colCount = 4;       //가로 길이

        int cardIndex = 0;      //카드순서

        for(int row=0; row<rowCount; row++)     
        {
            for(int col=0; col<colCount; col++)
            {
                float posX = (col - (colCount / 2)) * spaceX + (spaceX / 2);
                //우리가 원하는 가로 정렬된 x값=가로길이-(전체 가로 길이/2))*x만큼의 간격+(x만큼의 간격/2)를 더한 값


                float posY = (row - (int)(rowCount / 2)) * spaceY;      // 우리가 원하는 row y값=(세로길이-(정수형 변환)(가로길이/2))*y간격을 하면
                                                                        // 원하는y값이되고,그값울 3차원 백터의y값으로 넣는다.(가로 길이를 2로 나눈 나머지에서 반올림 함)...
                                                                        //형변환안하면 결과가이상해질수있음,int형으로 강제 형변환시 0.5보다 커도 소숫점 자리를 그냥 없애고 정수부분만 가져다가 씀..
                        //가운데를 기준으로 rowCount만큼 카드가 퍼짐.

                Vector3 pos=new Vector3(posX,posY,0f);          //3차원백터의 포지션=새로운 3차원백터의 0,0,0의좌표값을가짐.
                GameObject cardObject=Instantiate(cardPrefab, pos, Quaternion.identity);         //회전값이 필요없음,새로운오브젝트Instantiate가 만들어짐,
                Card card = cardObject.GetComponent<Card>();

                int cardID = cardIDList[cardIndex++];       //카드id리스트의 인덱스안에있는값을 카드id로설정하고 반복문에 의해 0번째부터 순회.
                card.SetCardID(cardID);         //카드를 한쌍씩 똑같은 그림으로 출력하도록 하기 위한  id값    

                card.SetAnimalSprite(cardSprites[cardID]);       //처음에는 0번이므로 0번째동물사진이 setanimalsprite에들어감.
                //cardID에의해 0,0부터시작..총20장의카드가 1쌍씩 출력됨.(현재는 정해진 순서로만 나옴.)
                //5*4번수행됨

                cardList.Add(card);     //카드리스트에 카드를추가해서
            }
        }
    }

    public List<Card> GetCards()    //리턴해서 리스트를순회하면서 모든카드를 한번에뒤집는다
    {
        return cardList;
    }
}
