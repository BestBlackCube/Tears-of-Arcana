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
    - ### [캐릭터](https://github.com/BestBlackCube/Tears-of-Arcana/commit/907cd865b1257523659bd85fd23426fcc2efee61#diff-ed27c78d099f327dd8af86f3ff53ca03e0d98bab8ac0cd11b82cd98c91985248)
        - [Charater_Name](https://github.com/BestBlackCube/Tears-of-Arcana/commit/907cd865b1257523659bd85fd23426fcc2efee61#diff-1987f9ff1eb50f50d94e8db0fc4e558e3ddfa4aceb09324456f6487810af7212)에서 선택된 이름을 전달받아 switch문에서 status값을 대입받는다.
    - ### [카드](https://github.com/BestBlackCube/Tears-of-Arcana/commit/907cd865b1257523659bd85fd23426fcc2efee61#diff-d8b248cc33b37fa6eee65eded04022910af1d03294581582088a1eff187f9d6c)
        - [Card_Name](https://github.com/BestBlackCube/Tears-of-Arcana/commit/907cd865b1257523659bd85fd23426fcc2efee61#diff-17f9f9a7fc3450dc2d223eabfbf8855dee5da1f18ae13481dfd3a7256bd5700a)에서 선택된 이름을 전달 받아 switch문에서 status값을 대입받는다.

    - 빈 생성자는 밑에있는 생성자의 변수값이 있어, public으로 호출 할때 값을 넣어야 하지만,  
    빈 생성자를 만들어 값을 입력하지 않아도 되는 덮어씌우기 용 입니다
- ## 카드
    - ### [카드 선택](https://github.com/BestBlackCube/Tears-of-Arcana/commit/907cd865b1257523659bd85fd23426fcc2efee61#diff-92345aeeb268808ac129e75f7c3b81d309855288e0bb96101e0fc9e257b648fa#L89-L109)
        - 마우스로 카드위로 카드를 올릴 때 마우스 커서가 카드에 있는지 감지하고, 클릭 시 선택 가능하다.
    - ### [카드 정보](https://github.com/BestBlackCube/Tears-of-Arcana/blob/907cd865b1257523659bd85fd23426fcc2efee61/Tears%20of%20Arcana/Assets/2D%20Card%20Project/card/Script/Card_Script.cs#L115-L161)
        - 마우스로 카드를 선택 할 시 CardDeckField_Script.cs에 선택한 카드의 정보가 대입되고,  
        플레이어 캐릭터와 적 캐릭터에 사용이 가능한 카드대로 나뉘며, 선택한 카드는 캐릭터들에게  
        사용 할 시 대입된 정보가 캐릭터에게 전달 된다.
    - ### [캐릭터 인식](https://github.com/BestBlackCube/Tears-of-Arcana/commit/907cd865b1257523659bd85fd23426fcc2efee61#diff-92345aeeb268808ac129e75f7c3b81d309855288e0bb96101e0fc9e257b648fa#L175-L201)
        - 카드를 사용 하기 전 해당 캐릭터에 마우스 커서를 올릴경우 해당 오브젝트의 이름데이터를  
        전달 받아 해당하는 조건이 실행되는 기능이다.

- ## 카드 덱
    - ### [카드덱 뽑기](https://github.com/BestBlackCube/Tears-of-Arcana/commit/907cd865b1257523659bd85fd23426fcc2efee61#diff-d2598a2e0df734b4d3c9f0e7df52a6be6f501bc41352db769fbc11d44f7f3d09#L71-L143)
        - 카드 데이터를 새로운 객체로 생성하고, 카드덱에서 뽑기를 시작할때 5개가 랜덤함수를 통해  
        다양한 카드가 생성된다.
    - ### [카드 숨기기](https://github.com/BestBlackCube/Tears-of-Arcana/commit/907cd865b1257523659bd85fd23426fcc2efee61#diff-65c3cbfa0e5a7263ff7ce50b4c1162e5c1af0b494f4e54cd3a6f1fe31ed19600#L49-L63)
        - 플레이어가 카드를 선택 할 경우 다른 카드가 선택 되는걸 막기위해 시야에서 제외 시킨다.
    - ### [카드 생성 회전](https://github.com/BestBlackCube/Tears-of-Arcana/commit/907cd865b1257523659bd85fd23426fcc2efee61#diff-65c3cbfa0e5a7263ff7ce50b4c1162e5c1af0b494f4e54cd3a6f1fe31ed19600#L76-L103)
        - 카드가 생성 되었을때 이동하는 동안 Rotation를 수정하여 카드가 회전하는 이펙트 기능이다.
    - ### [카드 빈 배열](https://github.com/BestBlackCube/Tears-of-Arcana/commit/907cd865b1257523659bd85fd23426fcc2efee61#diff-65c3cbfa0e5a7263ff7ce50b4c1162e5c1af0b494f4e54cd3a6f1fe31ed19600#L131-L158)
        - 생성된 5개중 하나라도 사용하게 된다면 중간에 공백이 생겨 카드가 생성 될때 빈공간이 존재하여  
        카드간의 거리가 멀찍히 생긴다 그걸 방지하기 위해 빈 공간을 채워준다.
## 버그 리포트

## 버전 표기법 (Semantic Versioning)
```
[주 버전].[부 버전].[수 버전]
   0   .   0   .   0

- 주 버전: 하위 호환성이 깨지는 변경
- 부 버전: 하위 호환성 유지하며 기능 추가
- 수 버전: 하위 호환성 유지하며 버그 수정
```