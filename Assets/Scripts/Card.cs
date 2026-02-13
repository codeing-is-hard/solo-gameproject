using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;      //유니티 에셋에서 받은 dotween 기능을 쓰기 위해 적음.
public class Card : MonoBehaviour
{

    [SerializeField] private Sprite animalSprite;       //카드 앞면

    [SerializeField] private SpriteRenderer cardRenderer;

    [SerializeField] private Sprite backSprite;     //뒤집힌 카드가 다시 뒤집힘==앞면으로 전환

    private bool isFlipped = false;     //ture이면 카드가 뒤집힌거고 false이면 안뒤집힌것.
    private bool isFlipping=false;      //처음엔 뒤집고있지않으니까false.

    public int cardID;          //카드 id설정에사용할변수.


    public void SetCardID(int id)       //카드 id설정
    {
        cardID=id;

    }





    public void SetAnimalSprite(Sprite sprite)
    {
        animalSprite = sprite;
    }


    public void FlipCard()      //이미지 교체
    {
        isFlipping=true;

        Vector3 originalScale = transform.localScale;       //3차원백터,인스펙터의 스케일값을 기본값으로 설정.
        Vector3 targetScale=new Vector3(0f,originalScale.y,originalScale.z);    //3차원백터는 3개의 실수(더블이 아닌 float값)좌표값을 갖는다.(x,y,z축)

        transform.DOScale(targetScale, 0.2f).OnComplete(() =>
        {
            isFlipped = !isFlipped;     //앞이면뒤로 뒤면앞으로바꿔줌

            if (isFlipped)
            {
                cardRenderer.sprite = animalSprite;
            }
            else
            {
                cardRenderer.sprite = backSprite;
            }

            transform.DOScale(originalScale, 0.2f).OnComplete(() =>
            {
                isFlipping = false;     //완료되면 뒤집고 있는 상태가 아니게끔 변경
            });


        });

            //dotween의 스케일기능을이용,0.2초의 시간에 걸쳐 원래 스케일X값에서 타겟 스케일의 X값으로 바꿔줌.
            //람다식이용,스케일 기능이 끝나면 람다식 안의 명령을 수행.

        
        
    }



    void OnMouseDown()
    {
        if(!isFlipping)
        {
            FlipCard();         //메소드호출(위에서 구현해 둔 메소드 가져와서 호출)
        }
        
    }
}
