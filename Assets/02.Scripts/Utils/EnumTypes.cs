namespace Utils.EnumTypes
{
    // �մ� Ÿ��
    public enum CustomerType
    {
        LaundryCustomer,
        EventCustomer
    }

    // �մ� ����
    public enum CustomerState
    {
        Idle,         // �⺻
        CounterZone,  // ī���ͷ� �̵�
        CompleteZone, // ��������� �̵�
        Wait,         // �ֹ� ��ٸ�
        LaundryWait,  // ���� ��ٸ�
        MiniGame,     // �̴ϰ��� ��
        Happy,        // ��� ����
        Angry,        // ȭ�� ����
        Leave         // ���� ����
    }

    // ��� Ÿ��
    public enum MachineType
    {
        Basket,         // �����ٱ���
        WashingMachine, // ��Ź��
        Dryer,          // ������
        IroningBoard    // �ٸ���
    }

    // ��� ����
    public enum MachineState
    {
        Idle,    // �⺻
        Working, // �۵� ��
        Complete // �Ϸ�
    }

    // ������ ����
    public enum LaundryState
    {
        Idle,     // �⺻
        Washing,  // ��Ź ��
        Dry,      // ���� ��
        Ironing,  // �ٸ��� ��
        Complete  // ��Ź �Ϸ�
    }

    // ȭ��ǥ Ÿ��
    public enum ArrowType
    {
        Up,
        Down,
        Right,
        Left
    }

    // ����� Ÿ��
    public enum BGMType
    {
        Title,
        Main,
        HappyEnding,
        NormalEnding,
        BadEnding,
        Credit
    }

    // ȿ���� Ÿ��
    public enum SFXType
    {
        Click,
        ClickError,
        CustomerHappy,
        CustomerAngry,
        MiniGame1_click,
        MiniGame1_Remove,
        MiniGame2_Input,
        MiniGame2_Sewing,
        MiniGame2_Error,
        MiniGame_Clear,
        MiniGame_Over
    }
}