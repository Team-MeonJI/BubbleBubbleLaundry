namespace Utils.EnumTypes
{
    // 손님 상태
    public enum CustomerState
    {
        Idle,  // 기본
        Move,  // 이동
        Wait,  // 기다림
        Leave  // 퇴장
    }

    // 기계 타입
    public enum MachineType
    {
        WashingMachine, // 세탁기
        Dryer,          // 건조기
        IroningBoard    // 다리미
    }
}