using UnityEngine;

public enum NPC_status
{
    Rest,//����Ϣ����Ϣ
    TouchFish,//����
    GoToWork,//ȥ������·��
    Work,//������
    GoToEat,//ȥ�Զ�����·��
    Eat,//�Զ���
    Transport//����
}
public enum Map_Target_Kind
{
    TouchFish_Area,//����ĵط�
    Rest_Area,//��Ϣ��
    Farm_Machine,//�������
    Eat_Area,//�Զ����ĵط�
    WareHouse_Area,//�ֿ�
    Freight_Target//���˵ĵط�
}
public enum Plant_State
{
    Empty,//�յ�,ɶҲû�У��ȴ������ֲ
    [Tooltip("��ѿ�׶�")]
    Germinate,//��ѿ�׶�

    plant,//����

    Grown,//�ɳ��׶�

    water,//��ˮ
    [Tooltip("����׶�")]
    Mature,//����׶�
    [Tooltip("ʩ�ʽ׶�")]
    fertilize,//ʩ��
    bug_control,//����

    harvest//�ջ�
}




