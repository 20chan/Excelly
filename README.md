# Excelly
간단한 통계 연산을 지원하는 스프레드 시트 프로그램입니다.


# 사용법
## 매개변수

    =SUM(A0 A1 A2)
    : A0셀 A1셀 A2셀의 합을 구합니다.

    =SUM(A)
    : A열 모든 셀의 합을 구합니다.

    =SUM('100' '200' A0)
    : 100 200 A0셀의 합을 구합니다.

    =SUM({list1})
    : list1 배열의 합을 구합니다.

## 함수

    =SUM(args)
    : 매개변수의 합을 구합니다.

    =AVERAGE(args)
    : 매개변수의 평균값을 구합니다.

    =VARS(args)
    : 매개변수의 분산을 구합니다.

    =DEV(args)
    : 매개변수의 표준편차를 구합니다.

    =RAND()
    : 음수가 아닌 임의의 정수를 반환합니다.

    =RAND(MAX)
    : MAX보다 작은 임의의 정수를 반환합니다.

    =RAND(MIN MAX)
    : MIN이상이고 MAX보다 작은 임의의 정수를 반환합니다.

    =RANDDOUBLE()
    : 0과 1사이의 실수를 반환합니다.

    =RANDDOUBLE(MAX)
    : MAX보다 작은 임의의 실수를 반환합니다.

    =RANDDOUBLE(MIN MAX)
    : MIN이상이고 MAX보다 작은 임의의 실수를 반환합니다.

## 리스트
'Type(Argument)' 라는 값을 Length를 길이로 하는 리스트를 만들 수 있습니다.
셀에서 호출은 {Name}으로 가능합니다.
