namespace Utils.EnumTypes
{
    // 손님 타입
    public enum CustomerType
    {
        LaundryCustomer,
        EventCustomer
    }

    // 손님 상태
    public enum CustomerState
    {
        Idle,         // 기본
        CounterZone,  // 카운터로 이동
        CompleteZone, // 대기존으로 이동
        Wait,         // 주문 기다림
        LaundryWait,  // 빨래 기다림
        MiniGame,     // 미니게임 중
        Happy,        // 기쁨 상태
        Angry,        // 화남 상태
        Leave         // 가게 퇴장
    }

    // 기계 타입
    public enum MachineType
    {
        Basket,         // 빨랫바구니
        WashingMachine, // 세탁기
        Dryer,          // 건조기
        IroningBoard    // 다리미
    }

    // 기계 상태
    public enum MachineState
    {
        Idle,    // 기본
        Working, // 작동 중
        Complete // 완료
    }

    // 빨랫감 상태
    public enum LaundryState
    {
        Idle,     // 기본
        Washing,  // 세탁 중
        Dry,      // 건조 중
        Ironing,  // 다림질 중
        Complete  // 세탁 완료
    }

    // 화살표 타입
    public enum ArrowType
    {
        Up,
        Down,
        Right,
        Left
    }
}