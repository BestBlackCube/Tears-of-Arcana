# Tears-of-Arcana 게임 개발 1.1.1v
## 기능설명
로딩창을 만들어 float 타이머가 끝날 시 검은 화면의 투명도 값을 수정하여 게임 화면으로 전환 됨.  
턴 종료 버튼을 통해 플레이어턴이 끝날 시 적 캐릭터들의 공격이 시작한다.  
적 캐릭터 고블린, 눈알 괴물, 버섯 괴물 3종류 추가

## 부가 기능
- ### [로딩창](https://github.com/BestBlackCube/Tears-of-Arcana/blob/51ee00d9ed7011e13e61be085bb7024ccecf55a7/Tears%20of%20Arcana/Assets/2D%20Card%20Project/public%20Script/BlackScreen_Script.cs)
    - 게임이 시작 될때, 스테이지를 클리어 했을때 검은화면의 투명도가 점차 100으로 바뀌며, 일정 시간  
    이후에 투명도가 다시 0으로 바뀌어 스테이지 팻말과 함께 카드가 생성되는 명령을 보낸다.
- ### [턴종료 버튼](https://github.com/BestBlackCube/Tears-of-Arcana/blob/51ee00d9ed7011e13e61be085bb7024ccecf55a7/Tears%20of%20Arcana/Assets/2D%20Card%20Project/public%20Script/turnEnd_Script.cs)
    - 플레이어가 카드 소진 시 UI가 활성화 되며 버튼을 클릭 하게 되면 턴이 넘어가며 적 캐릭터들이  
    플레이어를 공격하고 적의 연속적인 공격이 끝난 후 플레이어에게 다시 5개의 카드가 지급된다.
- ### [적 체력바](https://github.com/BestBlackCube/Tears-of-Arcana/blob/51ee00d9ed7011e13e61be085bb7024ccecf55a7/Tears%20of%20Arcana/Assets/2D%20Card%20Project/Charater/Skeleton/Script/Skeleton_Script.cs#L170-L200)
    - 적 캐릭터의 Hp 데이터를 SliderBar에 till값으로 적용시켜 체력이 잃게 될 경우 실시간으로  
    UI에 적용 된다.

## 캐릭터
- ## 적 캐릭터 (모든 적 캐릭터는 소스코드가 똑같습니다. Skeleton을 기준으로 설명)
    - ### 캐릭터 추가
        - prefab으로만 추가 되어있던 눈알 괴물, 고블린, 버섯 괴물의 소스코드 추가

## 카드
- ## 카드 종류
    - ### 카드 추가
        - 속성 카드(불, 물, 훍, 바람), 회복 카드(소, 중, 대) 추가
## 버그 수정
- ### 1.0.0v
    - 강제로 턴 종료시 다음 턴의 카드 생성 안됨
## 버전 표기법 (Semantic Versioning)
```
[주 버전].[부 버전].[수 버전]
   0   .   0   .   0

- 주 버전: 하위 호환성이 깨지는 변경
- 부 버전: 하위 호환성 유지하며 기능 추가
- 수 버전: 하위 호환성 유지하며 버그 수정
```

# Tears-of-Arcana 게임 개발 1.0.0v
## 기능 설명
해당 게임의 가장 중요한 시스템인 카드 뽑기와 카드를 제어하는 시스템을 개발하고 기본적인 플레이어 캐릭터와  
같은 테스트 몹을 만들어 카드 상호작용이 가능한지 테스트를 진행한다.
- ## 플레이어
    - ### [체력, 정신력 바](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/Charater/Player/Script/Player_Hpbar.cs)
        -  직관적인 플레이어 캐릭터의 캐릭터 체력과 정신력 2가지를 Bar형태로 정수 100을 기준으로  
        fill값을 조절하여 UI를 바꾼다.
    - ### [Player_Script.cs](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/Charater/Player/Script/Player_Script.cs)
        - [카드 상호작용](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/Charater/Player/Script/Player_Script.cs#L71-L82)하기 위해 정해진 카드가 플레이어 대상인지 적캐릭터 대상인지 판별하는 기능을 거쳐 캐릭터  
        위에 사용이 가능하다는 UI가 나오게 되는데 [플레이어 카드](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/Charater/Player/Script/Player_Script.cs#L134-L172) 할 수 있는 마우스 감지 기능을 추가하여  
        카드의 능력치가 전달된다.
        - [피해 확률](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/Charater/Player/Script/Player_Script.cs#L174-L229) 기능을 추가하여 플레이어 캐릭터가 모든 공격을 100%로 피해 받는것이 아닌 회피(무효화),  
        막기(반감), 기본 맞기 3가지 사항을 확률을 통해 정해진다
        - [애니메이션](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/Charater/Player/Script/Player_Script.cs#L83-L131)를 통해 플레이어 캐릭터가 피해 확률의 동작을 변경한다.
- ## 적 캐릭터 (모든 적 캐릭터는 소스코드가 똑같습니다. Skeleton을 기준으로 설명)
    - ### [적 캐릭터 파일](https://github.com/BestBlackCube/Tears-of-Arcana/tree/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/Charater)
        - 각 캐릭터 마다 prefab으로 만들어 각 캐릭터마다 고유속성을 부여한다.
    - ### [카드 상호작용](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/Charater/Skeleton/Script/Skeleton_Script.cs#L81-L92)
        - 해당 기능을 추가하여 정해진 카드가 플레이어 대상인지 적캐릭터 대상인지 판별하는 기능을 거쳐 캐릭터  
        위에 사용이 가능하다는 UI가 나오게 되는데 [카드 사용](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/Charater/Skeleton/Script/Skeleton_Script.cs#L94-L118) 할 수 있는 마우스 감지 기능을 추가하여 사용 할 시  
        카드의 능력치가 전달된다.
    - ### [애니메이션](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/Charater/Skeleton/Script/Skeleton_Script.cs#L120-L202)
        - 플레이어에게 공격을 받게 되면 Hit_Skeleton()이 실행되며 플레이어에게서 bool함수를 받아  
        애니메이션이 실행된다.
        - 플레이어가 턴을 종료시 적 캐릭터의 Skeleton_Attack()이 실행되며 플레이어에게 걸어가 공격을 한다.
    - ### [UI 위치 제어](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/Charater/Skeleton/Script/Skeleton_Script.cs#L203-L229)
        - 적캐릭터의 크기는 일정하지 않기 때문에 알맞는 위치로 이동한다.
    - ### [공격 명령](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/Charater/Skeleton/Script/Skeleton_Script.cs#L230-L275)
        - 플레이어가 턴을 종료하게 되면 적캐릭터(1-4번)이 공격하기 시작하는데 1-4번의 자리마다 오브젝트의  
        값이 달라 해당자리의 이름코드와 공격 순서의 값을 받아 공격을 시작한다.
- ## 정보 데이터
    - ### [캐릭터](https://github.com/BestBlackCube/Tears-of-Arcana/tree/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/Charater)
        - [Charater_Name](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/public%20Script/Charater_Status.cs)에서 [선택된 이름](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/public%20Script/charater_Name.cs)을 전달받아 switch문에서 status값을 대입받는다.
    - ### [카드](https://github.com/BestBlackCube/Tears-of-Arcana/tree/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/card)
        - [Card_Name](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/public%20Script/card_Status.cs)에서 [선택된 이름](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/public%20Script/card_Name.cs)을 전달 받아 switch문에서 status값을 대입받는다.

    - 빈 생성자는 밑에있는 생성자의 변수값이 있어, public으로 호출 할때 값을 넣어야 하지만,  
    빈 생성자를 만들어 값을 입력하지 않아도 되는 덮어씌우기 용 입니다
- ## 카드
    - ### [카드 선택](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/card/Script/Card_Script.cs#L89-L109)
        - 마우스로 카드위로 카드를 올릴 때 마우스 커서가 카드에 있는지 감지하고, 클릭 시 선택 가능하다.
    - ### [카드 정보](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/card/Script/Card_Script.cs#L115-L161)
        - 마우스로 카드를 선택 할 시 CardDeckField_Script.cs에 선택한 카드의 정보가 대입되고,  
        플레이어 캐릭터와 적 캐릭터에 사용이 가능한 카드대로 나뉘며, 선택한 카드는 캐릭터들에게  
        사용 할 시 대입된 정보가 캐릭터에게 전달 된다.
    - ### [캐릭터 인식](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/card/Script/Card_Script.cs#L175-L201)
        - 카드를 사용 하기 전 해당 캐릭터에 마우스 커서를 올릴경우 해당 오브젝트의 이름데이터를  
        전달 받아 해당하는 조건이 실행되는 기능이다.

- ## 카드 덱
    - ### [카드덱 뽑기](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/card/Script/CardDeck_Script.cs#L71-L143)
        - 카드 데이터를 새로운 객체로 생성하고, 카드덱에서 뽑기를 시작할때 5개가 랜덤함수를 통해  
        다양한 카드가 생성된다.
    - ### [카드 숨기기](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/card/Script/CardDeckField_Script.cs#L49-L63)
        - 플레이어가 카드를 선택 할 경우 다른 카드가 선택 되는걸 막기위해 시야에서 제외 시킨다.
    - ### [카드 생성 회전](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/card/Script/CardDeckField_Script.cs#L76-L103)
        - 카드가 생성 되었을때 이동하는 동안 Rotation를 수정하여 카드가 회전하는 이펙트 기능이다.
    - ### [카드 빈 배열](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/card/Script/CardDeckField_Script.cs#L131-L158)
        - 생성된 5개중 하나라도 사용하게 된다면 중간에 공백이 생겨 카드가 생성 될때 빈공간이 존재하여  
        카드간의 거리가 멀찍히 생긴다 그걸 방지하기 위해 빈 공간을 채워준다.
## 버그 수정

## 버전 표기법 (Semantic Versioning)
```
[주 버전].[부 버전].[수 버전]
   0   .   0   .   0

- 주 버전: 하위 호환성이 깨지는 변경
- 부 버전: 하위 호환성 유지하며 기능 추가
- 수 버전: 하위 호환성 유지하며 버그 수정
```